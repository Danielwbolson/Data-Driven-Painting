using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VTK;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StyleExampleCustomDataset : StyleCustomDataset
    {

        VTK.vtkDataSet _outputVTKDataset;

        public override void ComputeDataset() {
            if(_sourceVariableSocket.GetInput() == null) return;
            DataVariable inputVariable = ((DataVariable)_sourceVariableSocket.GetInput());
            Dataset ds = inputVariable.GetDataSet();
            //if(!(ds is VTKDataset)) return;


            
            VTK.vtkDataSet inputVTKDataset = ((VTKDataset)ds).GetVTKDataset();



            Debug.Log(inputVTKDataset.GetClassName());
            int n  = (Range<int>)_sampleCount.GetInput();

            vtkUnstructuredGrid output = vtkUnstructuredGrid.New();
            output.Allocate(n ,0);
            vtkPoints npts = vtkPoints.New();
            output.SetPoints(npts);
            vtkPointData npd = output.GetPointData();

            Random.InitState((Range<int>)_sampleSeed.GetInput());

            vtkFloatArray data = vtkFloatArray.New();
            data.SetName("data");
            output.GetPointData().AddArray(data);




            vtkIdList idlist = vtkIdList.New();

            for(int i =0; i < n; i++) {

                float x = Random.Range(inputVTKDataset.GetBounds().min.x,inputVTKDataset.GetBounds().max.x);
                float y = Random.Range(inputVTKDataset.GetBounds().min.y,inputVTKDataset.GetBounds().max.y);
                float z = Random.Range(inputVTKDataset.GetBounds().min.z,inputVTKDataset.GetBounds().max.z);


                idlist.InsertId(0,npts.InsertNextPoint(x,y,z));
                output.InsertNextCell(1,idlist);
                float fi = (float)i;
                unsafe{data.InsertNextTuple(new System.IntPtr((void*)&fi));}
                
            }


            Debug.Log(inputVTKDataset.GetClassName());

            _outputVTKDataset = output;
            if(_generatedDataset == null) {
                _generatedDataset = CreateInstance<VTKDataset>().Init(_outputVTKDataset,0,0);
                _generatedDataset.LoadDataset();

            } else {
                _generatedDataset.SetDataset(_outputVTKDataset);
                _generatedDataset.SetName("Randomly generated from" + inputVariable.GetName());

                
            }
            SetDataset(_generatedDataset);
            UpdateModule();
            SetUpToDate();

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
        
		// public override int GetNumberOfSubmodules() {
		// 	return GetAnchors().Count + GetAnchoredVariables().Count + GetContinuousVariables().Count;
		// }

		// public override StyleModule GetSubmodule(int i) {
        //     if(i < GetAnchors().Count) 
        //         return GetAnchors()[i];
        //     else if(i-GetAnchors().Count < GetAnchoredVariables().Count)
        //         return GetAnchoredVariables()[i-GetAnchors().Count ];
        //     else if(i-GetAnchors().Count-GetAnchoredVariables().Count < GetContinuousVariables().Count)
        //         return GetContinuousVariables()[i-GetAnchors().Count-GetAnchoredVariables().Count];
		// 	return null;
		// }
		// public override void AddSubmodule(StyleModule module) {
        //     if(module is Anchor) 
		//  	    AddAnchor((Anchor)module);
        //     else if (module is Variable) 
        //         AddVariable((Variable)module);
		// }


        public StyleExampleCustomDataset Init()
        {

            _generatedDatasetSocket = (new StyleSocket()).Init("", this, false, true, null);

            AddSubmodule(_generatedDatasetSocket);
            // _derivableVariableSocket = (new StyleSocket()).Init("Hook for deriving", this, false, true, _variable);
            // AddSubmodule(_generatedVariableSocket);


            _sourceVariableSocket = new VariableSocket();
            _sourceVariableSocket.Init("Domain", this);
            AddSubmodule(_sourceVariableSocket);

            _sampleCount = (new StyleTypeSocket<Range<int>>()).Init("Number of samples", this);
            _sampleCount.SetDefaultInputObject((new Range<int>(1, 100000,1000)));
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
            return "Random Volume Sampler";
        }

        public override bool IsValid()
        {
            if(_sourceVariableSocket.GetInput() != null)
                return true;
            return false;
        }

        public override StyleDataset CopyDataset(StyleDataset toCopy)
        {
            if (toCopy != null && toCopy is StyleExampleCustomDataset)
            {
                _sampleCount = ((StyleExampleCustomDataset)toCopy)._sampleCount;

            }
            return Init();
        }


    }
}

