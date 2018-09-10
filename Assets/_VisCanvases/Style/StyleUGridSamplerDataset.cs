using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using VTK;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StyleUGridSamplerDataset : StyleCustomDataset
    {
        vtkDataSet _outputVTKDataset;

        public static double[] barycentric_weights(int cType, long[] pids, vtkDataArray points)
        {
            double[] bary;




            if (cType == 12) // Hexahedron
            {
                double[] r = new double[]
                {
                        Random.Range(0.0f, 1.0f),
                        Random.Range(0.0f, 1.0f),
                        Random.Range(0.0f, 1.0f)
                };
                bary = new double[] {
                            ((1.0 - r[0]) * (1.0 - r[1]) * (1.0 - r[2])),
                             (r[0] * (1.0 - r[1]) * (1.0 - r[2])),
                             (r[0] * r[1] * (1.0 - r[2])),
                            ((1.0 - r[0]) * r[1] * (1.0 - r[2])),
                            ((1.0 - r[0]) * (1.0 - r[1]) * r[2]),
                             (r[0] * (1.0 - r[1]) * r[2]),
                             (r[0] * r[1] * r[2]),
                            ((1.0 - r[0]) * r[1] * r[2])
                };

                return bary;
            }
            else if (cType == 10) // Tetra
            {
                bary = new double[]
                {
                    Random.Range(0.0f, 1.0f),
                    Random.Range(0.0f, 1.0f),
                    Random.Range(0.0f, 1.0f),
                    Random.Range(0.0f, 1.0f)
                };
                if (bary[1] + bary[2] > 1.0)
                {
                    bary[1] = 1.0 - bary[1];
                    bary[2] = 1.0 - bary[2];

                }
                if (bary[2] + bary[3] > 1.0)
                {
                    double tmp = bary[3];
                    bary[3] = 1.0 - bary[1] - bary[2];
                    bary[2] = 1.0 - tmp;
                }
                else if (bary[1] + bary[2] + bary[3] > 1.0)
                {
                    double tmp = bary[3];
                    bary[3] = bary[1] + bary[2] + bary[3] - 1.0;
                    bary[1] = 1 - bary[2] - tmp;
                }
                bary[0] = 1 - bary[1] - bary[2] - bary[3];
            }
            else if (cType == 5) // Triangle
            {
                Vector3[] vertices = new Vector3[] 
                {
                    points.GetVector(pids[0]),
                    points.GetVector(pids[1]),
                    points.GetVector(pids[2])
                };

                Vector3 v0 = vertices[1] - vertices[0];
                Vector3 v1 = vertices[2] - vertices[0];

                double[] r = new double[]
                {
                    Random.Range(0.0f, 1.0f),
                    Random.Range(0.0f, 1.0f)
                };
                if (r[0] + r[1] > 1.0)
                {
                    r[0] = 1.0 - r[0];
                    r[1] = 1.0 - r[1];
                }
                Vector3 p = vertices[0] + (v0 * (float)r[0]) + (v1 * (float)r[1]);
                bary = new double[3];
                double sum = 0.0;
                for (int i = 0; i < 3; i++)
                {
                    v0 = p - vertices[(i + 1) % 3];
                    v1 = vertices[(i + 2) % 3] - vertices[(i + 1) % 3];
                    bary[i] = 0.5 + Vector3.Cross(v0, v1).magnitude;
                    sum = sum + bary[i];
                }
                for (int i = 0; i < 3; i++)
                    bary[i] = bary[i] / sum;
            }
            else if (cType == 9) // Quad
            {
                double[] r = new double[]
                {
                    Random.Range(0.0f, 1.0f),
                    Random.Range(0.0f, 1.0f)
                };

                bary = new double[] {
                  ((1.0 - r[0]) * (1.0 - r[1])),
                  (r[0] * (1.0 - r[1])),
                  (r[0] * r[1]),
                  ((1.0 - r[0]) * r[1])
                };
            }
            else
            {
                Debug.Log("unknown cell type");
                bary = new double[] { -1.0 };
            }

            return bary;

        }

        abstract class interpolator
        {
            protected vtkDataArray  srcArray;
            protected vtkFloatArray dstArray;

            public interpolator() {}

            public vtkFloatArray get_vtk() { return dstArray; }

            abstract public void initialize(vtkDataArray array);
            abstract public void interpolate(double[] bary, long[] pids);
        };

        class scalar_interpolator : interpolator
        {
            public override void initialize(vtkDataArray s) 
            {
                srcArray = s;
                dstArray = vtkFloatArray.New();
                dstArray.SetNumberOfComponents(1);
            }

            public override void interpolate(double[] bary, long[] pids)
            {
                double v = 0;
                for (int i = 0; i < bary.Length; i++)
                    v = v + bary[i] * srcArray.GetTuple1(pids[i]);
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

            public override void interpolate(double[] bary, long[] pids)
            {
                Vector3 v = new Vector3(0.0f, 0.0f, 0.0f);
                for (int i = 0; i < bary.Length; i++)
                {
                    Vector3 v0 = srcArray.GetVector(pids[i]);
                    v = v + (v0 * (float)bary[i]);
                }
                dstArray.InsertNextTuple3(v.x, v.y, v.z);
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
                dataset = vtkPointSet.SafeDownCast(ds);

                points = vtkPointSet.SafeDownCast(ds).GetPoints();

                interpolators = new Dictionary<string, interpolator>();

                interpolators["points"] = (interpolator) new vector_interpolator();
                interpolators["points"].initialize(points.GetData());

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

            public void interpolate(int n, int cType, long[] ids)
            {
                for (int i = 0; i < n; i++)
                {
                    double[] bary = StyleUGridSamplerDataset.barycentric_weights(cType, ids, points.GetData());
                    foreach (var interp in interpolators)
                        interp.Value.interpolate(bary, ids);
                }
            }

            public vtkUnstructuredGrid  create_vtu()
            {
                vtkUnstructuredGrid ug = vtkUnstructuredGrid.New();

                vtkPoints points = vtkPoints.New();
                points.SetData(interpolators["points"].get_vtk());
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
   
        public int[] error_diffusion(int n_samples, vtkDataArray qual)
        {
            double[] volumes = new double[qual.GetNumberOfTuples()];

            double sum = 0.0;
            for (int i = 0; i < qual.GetNumberOfTuples(); i++)
            {
                volumes[i] = qual.GetTuple1(i);
                sum = sum + volumes[i];
            }
      
            double[] f_per_cell = new double[volumes.Length];
            for (int i = 0; i < volumes.Length; i++)
            {
                volumes[i] = volumes[i] / sum;
                f_per_cell[i] = n_samples * volumes[i];
            }

            int[] n_per_cell = new int[volumes.Length];
            for (int i = 0; i < volumes.Length; i++)
            {
                n_per_cell[i] = (int)f_per_cell[i];
                double remainder = f_per_cell[i] - n_per_cell[i];
                if (i < (f_per_cell.Length - 1)) f_per_cell[i + 1] = f_per_cell[i + 1] + remainder;
            }
            return n_per_cell;
        }

        public override void UpdateModule()
        {
            if (_sourceVariableSocket.GetInput() == null) return;
            DataVariable inputVariable = ((DataVariable)_sourceVariableSocket.GetInput());
            Dataset ds = inputVariable.GetDataSet();

            vtkDataSet inputVTKDataset = ((VTKDataset)ds).GetVTKDataset();

            vtkMeshQuality m = vtkMeshQuality.New();
            m.SetTriangleQualityMeasureToArea();
            m.SetQuadQualityMeasureToArea();
            m.SetTetQualityMeasureToVolume();
            m.SetHexQualityMeasureToVolume();
            m.SetInputData(inputVTKDataset);
            m.Update();

            vtkDataSet mqds = m.GetOutput();

            int numberOfRequestedSamples = (Range<int>)_sampleCount.GetInput();

            int[] n_per_cell = error_diffusion(numberOfRequestedSamples, mqds.GetCellData().GetArray("Quality"));
      
            interpolator_set interpolators = new interpolator_set(inputVTKDataset);

            for (int i = 0; i < inputVTKDataset.GetNumberOfCells(); i++)
            {
                vtkCell cell;
                cell = inputVTKDataset.GetCell(i);

                if (n_per_cell[i] > 0)
                {
                    long nVertices = cell.GetNumberOfPoints();

                    long[] pids = new long[nVertices];
                    for (int j = 0; j < nVertices; j++)
                        pids[j] = cell.GetPointId(j);

                    interpolators.interpolate(n_per_cell[i], (int)cell.GetCellType(), pids);
                }

            }

            vtkUnstructuredGrid ug = interpolators.create_vtu();

            double[] vtx = new double[3];
            for (int i = 0; i < ug.GetNumberOfPoints(); i++)
            {
                ug.GetPoint(i, vtx);
                Debug.Log(vtx[0] + " " + vtx[1] + " " + vtx[2]);
            }

            _outputVTKDataset = ug;

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
        
        public StyleUGridSamplerDataset Init()
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
            return "UnstructuredGrid Sampler";
        }

        public override bool IsValid()
        {
            if(_sourceVariableSocket.GetInput() != null)
                return true;
            return false;
        }

        public override StyleDataset CopyDataset(StyleDataset toCopy)
        {
            if (toCopy != null && toCopy is StyleUGridSamplerDataset)
            {
                _sampleCount = ((StyleUGridSamplerDataset)toCopy)._sampleCount;

            }
            return Init();
        }


    }
}

