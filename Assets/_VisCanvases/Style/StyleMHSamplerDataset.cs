using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

using UnityEngine;
using VTK;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StyleMHSamplerDataset : StyleCustomDataset
    {
		public override string GetTypeTag() {
			return "MH_SAMPLER";
		}

        float map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s-a1)*(b2-b1)/(a2-a1);
        }

        vtkDataSet _outputVTKDataset;
        double mean = 0.0;

        public float GaussianRandomNumber(double stdDev)
        {
            return (float)(mean + stdDev * Mathf.Sqrt(-2.0f * Mathf.Log(Random.Range(0.0f, 1.0f))) * Mathf.Sin(2.0f * Mathf.PI * Random.Range(0.0f, 1.0f)));
        }


        public Vector3 GaussianRandomVector(double stdDev)
        {
            return new Vector3(GaussianRandomNumber(stdDev), GaussianRandomNumber(stdDev), GaussianRandomNumber(stdDev));
        }


        class VTIInterpolator
        {
            protected double[] origin;
            protected double[] deltas;
            protected double[] topright;
            protected long[] incs;
            protected int[] counts;

            public VTIInterpolator(vtkDataSet dataset)
            {
                vtkImageData vti = vtkImageData.SafeDownCast(dataset);

                int[] extent = new int[6];
                vti.GetExtent(extent);

                origin = new double[3];
                vti.GetOrigin(origin);

                counts = new int[3];
                vti.GetDimensions(counts);

                deltas = new double[3];
                vti.GetSpacing(deltas);

                incs = new long[3];
                vti.GetIncrements(incs);

                topright = new double[3];
                for (int i = 0; i < 3; i++)
                {
                    origin[i] = origin[i] + extent[2 * i] * deltas[i];
                    topright[i] = origin[i] + (counts[i] - 1) * deltas[i];
                }
            }

            public Vector3 center()
            {
                Vector3 v = new Vector3();
                for (int i = 0; i < 3; i++)
                    v[i] = (float)(origin[i] + ((counts[i] - 1.0) / 2.0) * deltas[i]);
                return v;
            }

            public void interpolant(Vector3 point, long[] indx, double[] dxyz)
            {
                double[] lxyz = new double[3];
                long[] ijk = new long[3];

                for (int i = 0; i < 3; i++)
                {
                    lxyz[i] = (point[i] - origin[i]) / deltas[i];
                    ijk[i] = (int)(lxyz[i]);
                    dxyz[i] = lxyz[i] - ijk[i];
                }

                for (int i = 0; i < 8; i++)
                {
                    int offset = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        bool isSet = 1 == (i & (1 << j));
                        //bool isSet = 1 == (i & (1 << (2 - j)));
                        offset = (int)(offset + ((ijk[j] + (isSet ? 1 : 0)) * incs[j]));
                    }
                    indx[i] = offset;
                }
            }

            public double interpolate_scalar(vtkDataArray scalars, long[] indx, double[] dxyz)
            {
                double[] corner_scalars = new double[8];
                for (int i = 0; i < 8; i++)
                    corner_scalars[i] = scalars.GetTuple1(indx[i]);

                for (int i = 0; i < 4; i++)
                    corner_scalars[i] = corner_scalars[i] * (1.0 - dxyz[0]) + corner_scalars[i + 4] * dxyz[0];
                for (int i = 0; i < 2; i++)
                    corner_scalars[i] = corner_scalars[i] * (1.0 - dxyz[1]) + corner_scalars[i + 2] * dxyz[1];

                return corner_scalars[0] * (1.0 - dxyz[2]) + corner_scalars[1] * dxyz[2];
            }

            public double[] interpolate_vector(vtkDataArray vectors, long[] indx, double[] dxyz)
            {
                Vector3[] corner_vectors = new Vector3[8];
                for (int i = 0; i < 8; i++)
                    corner_vectors[i] = vectors.GetVector(indx[i]);

                double[] result = new double[3];

                for (int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < 4; i++)
                        corner_vectors[i][j] = (float)(corner_vectors[i][j] * (1.0f - dxyz[0]) + corner_vectors[i + 4][j] * dxyz[0]);
                    for (int i = 0; i < 2; i++)
                        corner_vectors[i][j] = (float)(corner_vectors[i][j] * (1.0f - dxyz[1]) + corner_vectors[i + 2][j] * dxyz[1]);
                    result[j] = corner_vectors[0][j] * (1.0 - dxyz[2]) + corner_vectors[1][j] * dxyz[2];
                }

                return result;
            }

            public bool inside(Vector3 point)
            {
                for (int i = 0; i < 3; i++)
                    if (point[i] < origin[i] || point[i] > topright[i])
                        return false;
                return true;
            }
        }

        class VTI_MetropolisHastings : VTIInterpolator
        {
            vtkDataArray density;
            double[] minmax;
            double[] mapping;
            vtkImageData vti;
            double stdDev;

            public VTI_MetropolisHastings(vtkDataSet dataset, string varname) : base(dataset)
            {
                vti = vtkImageData.SafeDownCast(dataset);
                density = vti.GetPointData().GetArray(varname);
                minmax = new double[2];
                density.GetRange(minmax);

                Vector3 diag = new Vector3((float)origin[0], (float)origin[1], (float)origin[2]) - new Vector3((float)topright[0], (float)topright[1], (float)topright[2]);
                stdDev = diag.magnitude / 50.0;
            }

            public double get_stddev() { return stdDev; }

            public Vector3 get_starting_point()
            {
                double mm = minmax[0] + mapping[0] * (minmax[1] - minmax[0]);
                double MM = minmax[0] + mapping[1] * (minmax[1] - minmax[0]);

                double best_v = mm;
                int best_i = 0, best_j = 0, best_k = 0;
                int indx = 0;
                double[] vv = new double[1];
                for (int k = 0; k < counts[0]; k++)
                {
                    for (int j = 0; j < counts[1]; j++)
                    {
                        for (int i = 0; i < counts[2]; i++)
                        {
                            density.GetTuple(indx++, vv);
                            double v = vv[0];
                            if (v >= mm && v <= MM && v > best_v)
                            {
                                best_i = i;  
                                best_j = j;
                                best_k = k;
                                best_v = v;

                            }
                        }
                    }
                }

                if (best_v == mm)
                {
                    Debug.Log("No data in range");
                    return center();
                }

                Vector3 best_point = new Vector3((float)(origin[2] + best_i * deltas[2]), (float)(origin[1] + best_j * deltas[1]), (float)(origin[2] + best_k * deltas[2]));
                return best_point;
            }

            public double Q(Vector3 point, long[] indx, double[] dxyz, int p)
            {
                if (!inside(point))
                    return 0.0;
                
                interpolant(point, indx, dxyz);
                double v = interpolate_scalar(density, indx, dxyz);

                double mm = minmax[0] + mapping[0] * (minmax[1] - minmax[0]);
                double MM = minmax[0] + mapping[1] * (minmax[1] - minmax[0]);

                v = (v - mm) / (MM - mm);
                if (v < 0.0) return 0.0;
                else if (v > 1.0) return 1.0;

                if (p > 1)
                {
                    double vv = v;
                    for (int i = 1; i < p; i++)
                        v = v * vv;
                }

                return v;
            }

            public void set_mapping_range(double[] r)
            {
                mapping = r;
            }
        }

        abstract class interpolator
        {
            protected vtkDataArray  srcArray;
            protected vtkFloatArray dstArray;

            public interpolator() {}

            public vtkFloatArray get_vtk() { return dstArray; }

            abstract public void initialize(vtkDataArray array);
            abstract public void interpolate(VTIInterpolator data_interpolator, long[] indx, double[] dxyz);
        };

        class scalar_interpolator : interpolator
        {
            public override void initialize(vtkDataArray s) 
            {
                srcArray = s;
                dstArray = vtkFloatArray.New();
                dstArray.SetNumberOfComponents(1);
            }

            public override void interpolate(VTIInterpolator data_interpolator, long[] indx, double[] dxyz)
            {
                double v = data_interpolator.interpolate_scalar(srcArray, indx, dxyz);
                dstArray.InsertNextTuple1(v);
            }
        };

        class vector_interpolator : interpolator
        {
            public override void initialize(vtkDataArray s) 
            {
                srcArray = s;
                dstArray = vtkFloatArray.New();
                dstArray.SetNumberOfComponents(3);
            }

            public override void interpolate(VTIInterpolator data_interpolator, long[] indx, double[] dxyz)
            {
                double[] v = data_interpolator.interpolate_vector(srcArray, indx, dxyz);
                dstArray.InsertNextTuple3(v[0], v[1], v[2]);
            }
        };

        class interpolator_set
        {
            public Dictionary<string, interpolator> interpolators;
            vtkImageData dataset;
            vtkPoints points;
            int nSamples;

            public interpolator_set(vtkDataSet ds, int n)
            {
                dataset = vtkImageData.SafeDownCast(ds);
                nSamples = 0;

                interpolators = new Dictionary<string, interpolator>();

                 for (int i = 0; i < dataset.GetPointData().GetNumberOfArrays(); i++)
                {
                    vtkDataArray array = dataset.GetPointData().GetArray(i);
                    string arrayName = dataset.GetPointData().GetArrayName(i);

                    interpolators[arrayName] = (array.GetNumberOfComponents() == 3) ? 
                        (interpolator) new vector_interpolator() : 
                        (interpolator) new scalar_interpolator();
                    
                    interpolators[arrayName].initialize(array);
                }

                points = vtkPoints.New();
                points.Allocate(n, 0);
            }

            public int get_number_of_samples() { return nSamples; }

            public void interpolate(VTIInterpolator data_interpolator, Vector3 point, long[] indx, double[] dxyz)
            {
                nSamples = nSamples + 1;
                foreach (var interp in interpolators)
                    interp.Value.interpolate(data_interpolator, indx, dxyz);

                double[] p = new double[] { (double)point.x, (double)point.y, (double)point.z };
                points.InsertNextPoint(p);
            }

            public vtkUnstructuredGrid create_vtu()
            {

                /*
                StreamWriter wrtr = new StreamWriter("/Users/gda/m-h.csv", false);
                wrtr.WriteLine("X,Y,Z");
                for (int i = 0; i < points.GetNumberOfPoints(); i++)
                {
                    Vector3 p = points.GetPoint(i);
                    wrtr.WriteLine(string.Format("{0},{1},{2}", p.x, p.y, p.z));
                }
                wrtr.Close();
                */

                vtkUnstructuredGrid ug = vtkUnstructuredGrid.New();
                ug.SetPoints(points);

                ug.Allocate(ug.GetNumberOfPoints(), 1);

                vtkIdList ids = vtkIdList.New();
                ids.SetNumberOfIds(1);

                for (int i = 0; i < ug.GetNumberOfPoints(); i++)
                {
                    ids.SetId(0, i);
                    ug.InsertNextCell(1, ids);
                }

                foreach (var interp in interpolators)
                {
                    vtkDataArray darray = interp.Value.get_vtk();
                    if (interp.Key != "points")
                    {
                        darray.SetName(interp.Key);
                        ug.GetPointData().AddArray(darray);
                    }
                }
                return ug;
            }
        };
  
        public override void ComputeDataset()
        {

            if (_sourceVariableSocket.GetInput() == null) return;
            DataVariable inputVariable = ((DataVariable)_sourceVariableSocket.GetInput());
            Dataset ds = inputVariable.GetDataSet();

            int numberOfRequestedSamples = (Range<int>)_sampleCount.GetInput();
            double stepScale = (Range<float>)_stepScale.GetInput();

            double[] mappingRange = new double[2];

            mappingRange[0] = ((MinMax<float>)_variableRange.GetInput()).lowerValue;
            mappingRange[1] = ((MinMax<float>)_variableRange.GetInput()).upperValue;

            vtkDataSet inputVTKDataset = ((VTKDataset)ds).GetVTKDataset();

            VTI_MetropolisHastings mh_sampler = new VTI_MetropolisHastings(inputVTKDataset, inputVariable.GetName());
            mh_sampler.set_mapping_range(mappingRange);

            interpolator_set interpolators = new interpolator_set(inputVTKDataset, numberOfRequestedSamples);

            long[] corner_indices = new long[8];      // Corner indices for point in contention
            double[] dxyz = new double[3];          // Deltas within cell for point in contention

            Vector3 point = mh_sampler.get_starting_point();

            mh_sampler.interpolant(point, corner_indices, dxyz);
            double point_density = mh_sampler.Q(point, corner_indices, dxyz, 1);


            int misses = 0;
            Random.InitState(((Range<int>)_sampleSeed.GetInput()));
            while (interpolators.get_number_of_samples() < numberOfRequestedSamples)
            {
                Vector3 candidate = point + GaussianRandomVector(mh_sampler.get_stddev() * stepScale);

                mh_sampler.interpolant(candidate, corner_indices, dxyz);
                double candidate_density = mh_sampler.Q(candidate, corner_indices, dxyz, 1);

                if (candidate_density > 0.0)
                {
                    if (candidate_density >= point_density)
                    {
                        point = candidate;
                        point_density = candidate_density;
                        interpolators.interpolate(mh_sampler, point, corner_indices, dxyz);
                    }
                    else
                    {
                        double u = Random.Range(0.0f, 1.0f);
                        if (u < (candidate_density / point_density))
                        {
                            point = candidate;
                            point_density = candidate_density;
                            interpolators.interpolate(mh_sampler, point, corner_indices, dxyz);
                        }
                    }
                }
                else
                {
                    misses = misses + 1;
                    if ((misses % 1000) == 0)
                    {
                        int k = misses / 1000;
                        if (k < 4) stepScale = stepScale * 2;
                        else
                        {
                            Debug.Log("M-H excessive misses");
                            break;
                        }
                    }

                }
            }

            if (interpolators.get_number_of_samples() == 0)
            {
                mh_sampler.interpolant(mh_sampler.center(), corner_indices, dxyz);
                interpolators.interpolate(mh_sampler, mh_sampler.center(), corner_indices, dxyz);
            }

            _outputVTKDataset = interpolators.create_vtu();

            if(_generatedDataset == null) {
                _generatedDataset = CreateInstance<VTKDataset>().Init(_outputVTKDataset,0,0);
                _generatedDataset.LoadDataset();

            } else {
                _generatedDataset.SetDataset(_outputVTKDataset);
                _generatedDataset.SetName("MH on " + inputVariable.GetName());

            }
            SetDataset(_generatedDataset);
            UpdateModule();
            SetUpToDate();

        }

        

        public override void UpdateModule(string updatedSocket = null) {
                        Variable v = null;
            v = ((Variable)_sourceVariableSocket.GetInput());
            if(updatedSocket!= null) {
            if(updatedSocket  == _variableDataMax.GetUniqueIdentifier() ) {
                    

                if(_sourceVariableSocket.GetInput() != null)  {
                    ((Objectify<float>)_variableDataMax.GetInput()).value = Mathf.Min(((Objectify<float>)(_variableDataMax.GetInput())).value,v.GetMax().x);

                    ((MinMax<float>)_variableRange.GetInput()).upperValue = map(((Objectify<float>)_variableDataMax.GetInput()).value,v.GetMin().x,v.GetMax().x,0,1);
                }
            }
            if(updatedSocket == _variableDataMin.GetUniqueIdentifier()) {
                    
                if(_sourceVariableSocket.GetInput() != null)  {
                    float newVal = ((Objectify<float>)(_variableDataMin.GetInput())).value;
                    float minVal = v.GetMin().x;
                    float best = Mathf.Max(newVal,minVal);
                    ((Objectify<float>)_variableDataMin.GetInput()).value = best;


                    float newnormalized = map(((Objectify<float>)_variableDataMin.GetInput()).value,v.GetMin().x,v.GetMax().x,0,1);

                    ((MinMax<float>)_variableRange.GetInput()).lowerValue = map(((Objectify<float>)_variableDataMin.GetInput()).value,v.GetMin().x,v.GetMax().x,0,1);
                }
            }
            if(updatedSocket  == _variableRange.GetUniqueIdentifier() ) {
                    
                if(_sourceVariableSocket.GetInput() != null)  {

                    ((Objectify<float>)_variableDataMin.GetInput()).value = map(((MinMax<float>)_variableRange.GetInput()).lowerValue,0,1,v.GetMin().x,v.GetMax().x);
                    ((Objectify<float>)_variableDataMax.GetInput()).value = map(((MinMax<float>)_variableRange.GetInput()).upperValue,0,1,v.GetMin().x,v.GetMax().x);
                }
            }
            if(updatedSocket  == _sourceVariableSocket.GetUniqueIdentifier() ) {
                if(_sourceVariableSocket.GetInput() != null)  {
                    ((Objectify<float>)_variableDataMin.GetInput()).value = map(((MinMax<float>)_variableRange.GetInput()).lowerValue,0,1,v.GetMin().x,v.GetMax().x);
                    ((Objectify<float>)_variableDataMax.GetInput()).value = map(((MinMax<float>)_variableRange.GetInput()).upperValue,0,1,v.GetMin().x,v.GetMax().x);
                }
            }
            }
            base.UpdateModule(updatedSocket);
        }
        VTKDataset _generatedDataset;

        [SerializeField]
        public PointDataset _pointDataset;

        [SerializeField]
        public VariableSocket _sourceVariableSocket;

        [SerializeField]
        public StyleTypeSocket<Range<int>> _sampleSeed;

        [SerializeField]
        public StyleTypeSocket<Range<float>> _stepScale;

        [SerializeField]
        public StyleTypeSocket<Range<int>> _sampleCount;

        // [SerializeField]
        // public StyleTypeSocket<Range<float>> _densityMapMax;

        // [SerializeField]
        // public StyleTypeSocket<Range<float>> _densityMapMin;

        [SerializeField]
        public StyleTypeSocket<Objectify<float>> _variableDataMin;
        
        [SerializeField]
        public StyleTypeSocket<Objectify<float>> _variableDataMax;
        
        [SerializeField]
        public StyleTypeSocket<MinMax<float>> _variableRange;


        [SerializeField]
        public StyleSocket _generatedDatasetSocket;
        
        public StyleMHSamplerDataset Init()
        {
            _generatedDatasetSocket = (new StyleSocket()).Init("", this, false, true, null);

            AddSubmodule(_generatedDatasetSocket);
            // _derivableVariableSocket = (new StyleSocket()).Init("Hook for deriving", this, false, true, _variable);
            // AddSubmodule(_generatedVariableSocket);


            _sourceVariableSocket = new VariableSocket();
            _sourceVariableSocket.Init("Domain", this);
            AddSubmodule(_sourceVariableSocket);

            _sampleCount = (new StyleTypeSocket<Range<int>>()).Init("Number of samples", this);
            _sampleCount.SetDefaultInputObject((new Range<int>(1, 25000, 100)));
            AddSubmodule(_sampleCount);


            _sampleSeed = (new StyleTypeSocket<Range<int>>()).Init("SampleSeed", this);
            _sampleSeed.SetDefaultInputObject((new Range<int>(1, 10)));
            AddSubmodule(_sampleSeed);

            _stepScale = (new StyleTypeSocket<Range<float>>()).Init("Step Scale", this);
            _stepScale.SetDefaultInputObject((new Range<float>((float)0.1, (float)10.0, (float)1.0)));
            AddSubmodule(_stepScale);

            // _densityMapMax = (new StyleTypeSocket<Range<float>>()).Init("Density Mapping Max", this);
            // _densityMapMax.SetDefaultInputObject((new Range<float>((float)0.0, (float)1.0, (float)1.0)));
            // AddSubmodule(_densityMapMax);

            // _densityMapMin = (new StyleTypeSocket<Range<float>>()).Init("Density Mapping Min", this);
            // _densityMapMin.SetDefaultInputObject((new Range<float>((float)0.0, (float)1.0, (float)0.0)));
            // AddSubmodule(_densityMapMin);

            _variableRange = new StyleTypeSocket<MinMax<float>>();
            _variableRange.Init("Data Range",this);
            _variableRange.SetDefaultInputObject(new MinMax<float>(0,1));

            AddSubmodule(_variableRange);

            _variableDataMin = new StyleTypeSocket<Objectify<float>>();
            _variableDataMin.Init("Data min",this);
            _variableDataMin.SetDefaultInputObject(new Objectify<float>(0));

            AddSubmodule(_variableDataMin);

            _variableDataMax = new StyleTypeSocket<Objectify<float>>();
            _variableDataMax.Init("Data max",this);
            _variableDataMax.SetDefaultInputObject(new Objectify<float>(1));

            AddSubmodule(_variableDataMax);

 
            return this;
        }


        public override string GetLabel()
        {
            // if(_generatedVariableSocket.GetOutput() != null && ((Variable)_generatedVariableSocket.GetOutput())!=null)
            // Debug.Log("Custom Output:" + ((Variable)_generatedVariableSocket.GetOutput()).GetStream(null,0,0).GetNumberOfElements());
            return "M-H VTI Sampler";
        }

        public override bool IsValid()
        {
            if(_sourceVariableSocket.GetInput() != null)
                return true;
            return false;
        }

        public override StyleDataset CopyDataset(StyleDataset toCopy)
        {
            if (toCopy != null && toCopy is StyleMHSamplerDataset)
            {
                _sampleCount = ((StyleMHSamplerDataset)toCopy)._sampleCount;

            }
            return Init();
        }


    }
}

