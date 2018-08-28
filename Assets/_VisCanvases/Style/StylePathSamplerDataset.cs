using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using VTK;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StylePathSamplerDataset : StyleCustomDataset
    {
        vtkDataSet _outputVTKDataset;

        abstract class interpolator
        {
            protected vtkDataArray  srcArray;
            protected vtkFloatArray dstArray;

            public interpolator() {}

            public vtkFloatArray get_vtk() { return dstArray; }

            abstract public void initialize(vtkDataArray array);
            abstract public void interpolate(long i0, long i1, double d);
        };

        class scalar_interpolator : interpolator
        {
            public override void initialize(vtkDataArray s) 
            {
                srcArray = s;
                dstArray = vtkFloatArray.New();
                dstArray.SetNumberOfComponents(1);
            }

			public override void interpolate(long i0, long i1, double d)
            {
                double v0 = srcArray.GetTuple1(i0);
                double v1 = srcArray.GetTuple1(i1);

                float v = (float)((v0 * (1.0 - d)) + (v1 * d));
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

            public override void interpolate(long i0, long i1, double d)
            {
                Vector3 v0 = srcArray.GetVector(i0);
                Vector3 v1 = srcArray.GetVector(i1);
                double x = (v0.x * (1.0 - d)) + (v1.x * d);
                double y = (v0.y * (1.0 - d)) + (v1.y * d);
                double z = (v0.z * (1.0 - d)) + (v1.z * d);
                dstArray.InsertNextTuple3(x, y, z);
            }
        };

        class interpolator_set
        {
            public Dictionary<string, interpolator> interpolators;
            vtkDataSet dataset;
            vtkPoints points;
            
            int k;

            public interpolator_set(vtkDataSet ds)
            {
                dataset = ds;
                points = vtkPoints.New();
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
            }

            public void interpolate(int n, vtkIdList ids)
            {
                float l = 0;
                long i1, i0 = ids.GetId(0);
                Vector3 pt1, pt0 = dataset.GetPoint(i0);
                for (int i = 1; i < ids.GetNumberOfIds(); i++)
                {
                    long id = ids.GetId(i);
                    pt1 = dataset.GetPoint(id);
                    Vector3 dff = pt1 - pt0;
                    l = l + dff.magnitude;
                    pt0 = pt1;
                }

                float stepsize = l / (n - 1);

                double  l1, l0 = 0.0;
                i1 = i0 = ids.GetId(0);
                pt1 = pt0 = dataset.GetPoint(i0);
                float lnext = stepsize;

                points.InsertNextPoint(pt0.x, pt0.y, pt0.z);
                foreach (var interp in interpolators)
                    interp.Value.interpolate(0, 1, 0.0);
                
                for (int i = 1, k = 0; i < ids.GetNumberOfIds() && k < (n - 1); i++)
                {
                    i1 = ids.GetId(i);
                    pt1 = dataset.GetPoint(i1);
                    Vector3 dff = pt1 - pt0;
                    l1 = l0 + dff.magnitude;
                    while (lnext < l1 && k < (n - 1))
                    {
                        double d = ((lnext - l0) / (l1 - l0));

                        double x = (pt0.x * (1.0 - d)) + (pt1.x * d);
                        double y = (pt0.y * (1.0 - d)) + (pt1.y * d);
                        double z = (pt0.z * (1.0 - d)) + (pt1.z * d);

                        //Debug.Log(k + ": " + i0 + " " + i1 + " " + d + " " + x + " " + y + " " + z);
             
                        points.InsertNextPoint(x, y, z);
                        foreach (var interp in interpolators)
                            interp.Value.interpolate(i0, i1, d);

                        lnext = lnext + stepsize;
                        k = k + 1;
                    }
                    l0 = l1;
                    i0 = i1;
                    pt0 = pt1;
                }

                points.InsertNextPoint(pt1.x, pt1.y, pt1.z);
                foreach (var interp in interpolators)
                    interp.Value.interpolate(i0, i1, 1.0);
            }

            public void add_arrays_to_vtu(vtkUnstructuredGrid ug)
            {
                ug.SetPoints(points);

                foreach (var interp in interpolators)
                {
                    vtkDataArray darray = interp.Value.get_vtk();
                    darray.SetName(interp.Key);
                    ug.GetPointData().AddArray(darray);
                }

            }
        };
        

        public override void UpdateModule()
        {
            if(_sourceVariableSocket.GetInput() == null) return;
            DataVariable inputVariable = ((DataVariable)_sourceVariableSocket.GetInput());
            Dataset ds = inputVariable.GetDataSet();
 
            vtkDataSet inputVTKDataset = ((VTKDataset)ds).GetVTKDataset();

            inputVTKDataset.ShallowCopy(((VTKDataset)ds).GetVTKDataset());

            vtkDataSet ods;
            vtkPoints opts;
            vtkPointData opd = inputVTKDataset.GetPointData();

            if (inputVTKDataset.IsA("vtkPolyData"))
            {
                vtkPolyData p = vtkPolyData.New();
                p.ShallowCopy(vtkPolyData.SafeDownCast(inputVTKDataset));
                opts = p.GetPoints();
                ods = (vtkDataSet)p;
            }
            else if (inputVTKDataset.IsA("vtkUnstructuredGrid"))
            {
                vtkUnstructuredGrid u = vtkUnstructuredGrid.New();
                u.ShallowCopy(vtkUnstructuredGrid.SafeDownCast(inputVTKDataset));
                opts = u.GetPoints();
                ods = (vtkDataSet)u;
            }
            else
            {
                Debug.Log("inappropriate dataset type: " + inputVTKDataset.GetClassName());
                return;
            }

            float   totalLength = 0;
            float[] pathLengths = new float[ods.GetNumberOfCells()];

            vtkFloatArray arcLengthArray = vtkFloatArray.New();
            arcLengthArray.SetNumberOfComponents(1);
            arcLengthArray.SetNumberOfTuples(ods.GetNumberOfPoints());
            arcLengthArray.SetName("ArcLength");
            ods.GetPointData().AddArray(arcLengthArray);

            vtkIdList pids = vtkIdList.New();
            for (int i = 0; i < ods.GetNumberOfCells(); i++)
            {
                if (ods.GetCellType(i) == 4)
                {
                    ods.GetCellPoints(i, pids);
                    Vector3 p0 = opts.GetPoint(pids.GetId(0));
                    float length = 0;
                    arcLengthArray.SetTuple1(pids.GetId(0), length);
                    for (int j = 1; j < pids.GetNumberOfIds(); j++)
                    {
                        Vector3 p1 = opts.GetPoint(pids.GetId(j));
                        Vector3 d = p1 - p0;
                        length = length + d.magnitude;
                        p0 = p1;
                        arcLengthArray.SetTuple1(pids.GetId(j), length);
                    }
                    totalLength = totalLength + length;
                    Debug.Log("path " + i + " length: " + length);
                    pathLengths[i] = length;
                }
            }
             
            int numberOfRequestedSamples = (Range<int>)_sampleCount.GetInput();

            int[] samples_per_path = new int[ods.GetNumberOfCells()];
            for (int i = 0; i < ods.GetNumberOfCells(); i++)
            {
                samples_per_path[i] = (int)((pathLengths[i] / totalLength) * numberOfRequestedSamples);
                if (samples_per_path[i] < 2) samples_per_path[i] = 2;
            }

            interpolator_set interpolators = new interpolator_set(ods);

            vtkUnstructuredGrid vtu = vtkUnstructuredGrid.New();
            vtu.Allocate(ods.GetNumberOfCells(), 1);

            int offset = 0; // start of indices into point sets as we accrue cells

            for (int i = 0; i < ods.GetNumberOfCells(); i++)
            {
                if (ods.GetCellType(i) == 4)
                {
                    ods.GetCellPoints(i, pids);

                    interpolators.interpolate(samples_per_path[i], pids);

                    pids.Reset();
                    for (int j = 0; j < samples_per_path[i]; j++)
                        pids.InsertNextId(offset++);

                    vtu.InsertNextCell(4, pids);
                    Debug.Log("Cell " + i + ": " + pids.GetNumberOfIds());
                }
            }

            interpolators.add_arrays_to_vtu(vtu);

            _outputVTKDataset = vtu;

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
        public StyleSocket _generatedDatasetSocket;
        
        public StylePathSamplerDataset Init()
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

            return this;
        }


        public override string GetLabel()
        {
            // if(_generatedVariableSocket.GetOutput() != null && ((Variable)_generatedVariableSocket.GetOutput())!=null)
            // Debug.Log("Custom Output:" + ((Variable)_generatedVariableSocket.GetOutput()).GetStream(null,0,0).GetNumberOfElements());
            return "Path Sampler";
        }

        public override bool IsValid()
        {
            if(_sourceVariableSocket.GetInput() != null)
                return true;
            return false;
        }

        public override StyleDataset CopyDataset(StyleDataset toCopy)
        {
            if (toCopy != null && toCopy is StylePathSamplerDataset)
            {
                _sampleCount = ((StylePathSamplerDataset)toCopy)._sampleCount;

            }
            return Init();
        }


    }
}

