using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using VTK;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StyleMHSamplerDataset : StyleCustomDataset
    {
        vtkDataSet _outputVTKDataset;
        Random random;
        double mean = 0.0, stdDev = 1.0;

        public float GaussianRandomNumber()
        {
            return (float)(mean + stdDev * Mathf.Sqrt(-2.0f * Mathf.Log(Random.Range(0.0f, 1.0f))) * Mathf.Sin(2.0f * Mathf.PI * Random.Range(0.0f, 1.0f)));
        }


        public Vector3 GaussianRandomVector()
        {
            return new Vector3(GaussianRandomNumber(), GaussianRandomNumber(), GaussianRandomNumber());
        }


        class VTIInterpolator
        {
            double[] origin;
            double[] deltas;
            double[] topright;
            long[] incs;
            int[] counts;

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
                        bool isSet = 0 == (i & ((2 - j) << j));
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

            public VTI_MetropolisHastings(vtkDataSet dataset, string varname) : base(dataset)
            {
                vti = vtkImageData.SafeDownCast(dataset);
                density = vti.GetPointData().GetArray(varname);
                minmax = new double[2];
                density.GetRange(minmax);
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
                if (v < 0.0 || v > 1.0) return 0.0;

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
  
        public override void UpdateModule()
        {
            if (random == null) random = new Random();

            if (_sourceVariableSocket.GetInput() == null) return;
            DataVariable inputVariable = ((DataVariable)_sourceVariableSocket.GetInput());
            Dataset ds = inputVariable.GetDataSet();

            int numberOfRequestedSamples = (Range<int>)_sampleCount.GetInput();

            double[] mappingRange = new double[2];

            mappingRange[0] = ((MinMax<float>)_densityRange.GetInput()).lowerValue;
            mappingRange[1] = ((MinMax<float>)_densityRange.GetInput()).upperValue;


            vtkDataSet inputVTKDataset = ((VTKDataset)ds).GetVTKDataset();

            VTI_MetropolisHastings mh_sampler = new VTI_MetropolisHastings(inputVTKDataset, "snd");
            mh_sampler.set_mapping_range(mappingRange);

            interpolator_set interpolators = new interpolator_set(inputVTKDataset, numberOfRequestedSamples);

            long[] corner_indices = new long[8];      // Corner indices for point in contention
            double[] dxyz = new double[3];          // Deltas within cell for point in contention

            Vector3 point = mh_sampler.center();

            mh_sampler.interpolant(point, corner_indices, dxyz);
            double point_density = mh_sampler.Q(point, corner_indices, dxyz, 1);


            while (interpolators.get_number_of_samples() < numberOfRequestedSamples)
            {
                Vector3 candidate = point + GaussianRandomVector();

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
            }

            _outputVTKDataset = interpolators.create_vtu();

            if(_generatedDataset == null) {
                _generatedDataset = CreateInstance<VTKDataset>().Init(_outputVTKDataset,0,0);
                _generatedDataset.LoadDataset();

            } else {
                _generatedDataset.SetDataset(_outputVTKDataset);
            }
            SetDataset(_generatedDataset);
            
            base.UpdateModule();
         }

        VTKDataset _generatedDataset;

        [SerializeField]
        public PointDataset _pointDataset;

        [SerializeField]
        public VariableSocket _sourceVariableSocket;

        [SerializeField]
        public StyleTypeSocket<Range<int>> _sampleSeed;

        [SerializeField]
        public StyleTypeSocket<Range<int>> _sampleCount;

        [SerializeField]
        public StyleTypeSocket<MinMax<float>> _densityRange;

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
            _sampleCount.SetDefaultInputObject((new Range<int>(1, 100000,100)));
            AddSubmodule(_sampleCount);


            _sampleSeed = (new StyleTypeSocket<Range<int>>()).Init("SampleSeed", this);
            _sampleSeed.SetDefaultInputObject((new Range<int>(1, 10)));
            AddSubmodule(_sampleSeed);

            _densityRange = (new StyleTypeSocket<MinMax<float>>()).Init("Density Mapping Range", this);
            _densityRange.SetDefaultInputObject((new MinMax<float>(0, 1)));
            AddSubmodule(_densityRange);

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

