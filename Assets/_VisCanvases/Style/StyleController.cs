﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SculptingVis
{
    // [ExecuteInEditMode]
    public class StyleController : MonoBehaviour
    {

        
        public void Report() {



            // for(int i = 0; i < _links.Count; i++) {
            //     Debug.Log("Link[" + i + "] : " + _links[i].GetSource().GetOutput().GetInstanceID()+ "->" + _links[i].GetDestination().GetInput());
            // }
        }
        public void Reset()
        {
            Debug.Log("Reseting");
            GetLinks().Clear();


            GetLayers().Clear();
            GetVariables().Clear();
            GetDatasets().Clear();
            GetVisualElements().Clear();
            GetUserVariables().Clear();
            while(GetCanvases().Count > 0)
                RemoveCanvas(GetCanvases()[GetCanvases().Count-1]);
            AddCanvas();
            AddCanvas();
            GetCanvases()[0].SetBounds(new Vector3(5,4,5));
            GetCanvases()[0].gameObject.transform.SetPositionAndRotation(new Vector3(0,0,15),Quaternion.identity);




            VTKFileAsset contourData = new VTKFileAsset();
            contourData.SetPath("/Users/sethjohnson/NSF-Sculpting-Vis-Platform/unity/VisualizationPlatform/Assets/StreamingAssets/example_data/VTK/contour.vtp");

            Debug.Log(contourData.GetDataset().GetClassName());           
            SmartData.Dataset dataset = new SmartData.Dataset();
            dataset.SetName("Test Dataset");


            SmartData.Variable anchoredVariable = new SmartData.AnchoredVariable(dataset);
            anchoredVariable.SetName("Anchored Variable A");

            SmartData.Variable anchoredVariable2 = new SmartData.AnchoredVariable(dataset);
            anchoredVariable2.SetName("Anchored Variable B");


            SmartData.Variable continuousVariable = new SmartData.ContinuousVariable(dataset);
            continuousVariable.SetName("Continuous Variable A");

            dataset.AddVariable(anchoredVariable);
            dataset.AddVariable(anchoredVariable2);
            dataset.AddVariable(continuousVariable);

            SmartData.Anchor anchor = new SmartData.Anchor(dataset);
            anchor.SetName( "Original Anchor");
            SmartData.VTKPointDataStrategy vtkStrat = new SmartData.VTKPointDataStrategy();
            vtkStrat.SetDatasetFile(contourData);

            anchor.SetDataStrategy(vtkStrat);

            dataset.SetSourceAnchor(anchor);

            SmartData.Anchor anchor2 = new SmartData.Anchor(dataset);
            // anchor2.SetNumberOfPoints(3);
            anchor2.SetName( "Derived Anchor");
            SmartData.RandomSubsetPointDataStrategy strat = (new SmartData.RandomSubsetPointDataStrategy()).Init(dataset.GetSourceAnchor());

            anchor2.SetDataStrategy(strat);

            dataset.AddAnchor(anchor);
            dataset.AddAnchor(anchor2);

            SmartData.Datastream ds = dataset.GetDatastream( anchoredVariable2,anchor);

            Debug.Log(ds);

            anchor2.TriggerUpdate();

            StyleDataset datasetModule = ScriptableObject.CreateInstance<StyleDataset>();
            datasetModule.SetDataset(dataset);
            GetDatasets().Add(datasetModule);



            // _variables.Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init());
            // _variables.Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init());
            // _variables.Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init());
            // _variables.Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init());

        }


        [SerializeField]
        Dictionary<string,FileAsset> _files;

        public FileAsset GetFile(string path) {
            if(_files == null) _files = new Dictionary<string, FileAsset>();
            if(!_files.ContainsKey(path)) {
                return null;
            }
            return _files[path];
        }
        
        
		[SerializeField] 
		List<StyleLayer> _layerTypes;

        [SerializeField]
        List<StyleVariable> _variableTypes;

        [SerializeField]
        List<StyleModule> _visualElements;

        [SerializeField]
        List<StyleModule> _layers;

		[SerializeField]
		Style _style;

        [SerializeField]
        VariableController _userVariables;

        [SerializeField]
        List<StyleModule> _variables;


        [SerializeField]
        List<StyleDataset> _datasets;
        
        [SerializeField]
        List<StyleLink> _links;

        [SerializeField]
        Dictionary<string, StyleLink> _linksByDestination;

        [SerializeField]
        Dictionary<string, List<StyleLink>> _linksBySource;

        protected Dictionary<string, StyleLink> GetLinksByDestination()
        {
            if (_linksByDestination == null) _linksByDestination = new Dictionary<string, StyleLink>();
            return _linksByDestination;
        }
        protected  StyleLink GetLinkByDestination(StyleSocket destinationsocket)
        {
            if (_linksByDestination == null) _linksByDestination = new Dictionary<string, StyleLink>();
			if(_linksByDestination.ContainsKey(destinationsocket.GetUniqueIdentifier()))
				return _linksByDestination[destinationsocket.GetUniqueIdentifier()];
            return null;
        }
        protected Dictionary<string, List<StyleLink> > GetLinksBySource()
        {
            if (_linksBySource == null) _linksBySource = new Dictionary<string, List<StyleLink> >();
            return _linksBySource;
        }
		

        public List<StyleLink> GetLinks()
        {
            if (_links == null) _links = new List<StyleLink>();
            return _links;
        }

		public void RemoveLink(StyleLink link, bool removeFromIndex = false) {
			if (link != null){
				Debug.Log("Removing link: " + link.GetSource().GetUniqueIdentifier() + " -> " + link.GetDestination().GetUniqueIdentifier());
				link.GetDestination().ClearInput();
                GetLinks().Remove(link);
				if(link.GetDestination() != null)
            		GetLinksByDestination()[link.GetDestination().GetUniqueIdentifier()] = null;
				if(removeFromIndex)
				GetLinksBySource()[link.GetSource().GetUniqueIdentifier()].Remove(link);
			
				UpdateModuleLinks(link.GetDestination().GetModule());
			}
		}
        public void ClearSocket(StyleSocket socket)
        {
			if(socket.IsInput()) {
				StyleLink currentLink = null;
				if (GetLinksByDestination().ContainsKey(socket.GetUniqueIdentifier()))
					currentLink = GetLinkByDestination(socket);
				RemoveLink(currentLink);
				if(currentLink != null)currentLink.GetDestination().GetModule().UpdateModule();

			} 
			if(socket.IsOutput()) {
				StyleLink currentLink = null;


				foreach(StyleLink link in GetLinksBySource()[socket.GetUniqueIdentifier()]) {
					RemoveLink(link, false);
				}

				GetLinksBySource()[socket.GetUniqueIdentifier()].Clear();

			}


        }
        public void AddLink(StyleLink link)
        {

            ClearSocket(link.GetDestination());

            GetLinks().Add(link);
            GetLinksByDestination()[link.GetDestination().GetUniqueIdentifier()] = link;
			if(!GetLinksBySource().ContainsKey(link.GetSource().GetUniqueIdentifier()))
				GetLinksBySource()[link.GetSource().GetUniqueIdentifier()] = new List<StyleLink>();
			GetLinksBySource()[link.GetSource().GetUniqueIdentifier()].Add(link);

			link.GetDestination().SetInputObject(link.GetSource().GetOutput());

			link.GetDestination().GetModule().UpdateModule();
        }


        public List<StyleModule> GetVisualElements()
        {
            if (_visualElements == null) _visualElements = new List<StyleModule>();

            return _visualElements;
        }



		public Style GetStyle() {
			if(_style == null) _style = ScriptableObject.CreateInstance<Style>();
			return _style;
		}
        public VariableController GetUserVariableController()
        {
            if (_userVariables == null) _userVariables = ScriptableObject.CreateInstance<VariableController>();
            return _userVariables;
        }
           
        public List<StyleVariable> GetUserVariables()
        {
            return GetUserVariableController().GetVariables();
        }

        public List<StyleLayer> GetLayers()
        {
            return GetStyle().GetLayers();
        }

        public List<StyleModule> GetVariables()
        {
            if (_variables == null) _variables = new List<StyleModule>();

            return _variables;
        }

        public List<StyleDataset> GetDatasets()
        {
            if (_datasets == null) _datasets = new List<StyleDataset>();

            return _datasets;
        }
        // Use this for initialization
        void Start()
        {
            Reset();
            #if UNITY_EDITOR
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 45;
            #endif
        }

		public void UpdateModuleLinks(StyleModule module) {
			for(int i = 0; i < module.GetNumberOfSubmodules(); i++) {
                if( module.GetSubmodule(i) is StyleSocket) {
                    StyleSocket socket = (StyleSocket)module.GetSubmodule(i);
                    StyleLink link;
                    if((link = GetLinkByDestination(socket))!=null){
                        if(!link.GetDestination().DoesAccept(link.GetSource())) {
                            RemoveLink(link);
                        }

                    }
                }
			
			}
		}

        // Update is called once per frame
        void Update()
        {
            foreach(var dataset in GetDatasets()) {
                dataset.GetDataset().Update();
            }
        }

        public void LoadData(string path)
        {
            Debug.Log("Loading " + path);
            string extention = (Path.GetExtension(path));
            if (extention == "")
            {
                {
                    DirectoryInfo info = new DirectoryInfo(path);
                    FileInfo[] fileInfo = info.GetFiles();
                    foreach (var file in fileInfo)
                        LoadData(file.FullName);
                }


                {
                    DirectoryInfo info = new DirectoryInfo(path);
                    DirectoryInfo[] fileInfo = info.GetDirectories();
                    foreach (var file in fileInfo)
                        LoadData(file.FullName);
                }
            }

			else if(true) {
				// Texture2D loadedImage = new Texture2D(1,1);
				// loadedImage.LoadImage(File.ReadAllBytes(path));
				//GetVisualElements().Add(ScriptableObject.CreateInstance<StyleColormap>().Init(loadedImage,Path.GetFileNameWithoutExtension(path)));
				VTKDataset vtkds = VTKDataset.CreateInstance<VTKDataset>();
				vtkds.Init(path,0,0);
				if(vtkds.LoadDataset()) {
					Debug.Log("Loaded a VTK file! " + path);
				}

				for(int i = 0; i < vtkds.GetVariables().Length; i++) {
					GetVariables().Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init(vtkds.GetVariables()[i]));
				}
				
				if(vtkds.GetAnchor() != null)
					GetVariables().Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init(vtkds.GetAnchor()));
			}
        }

        public void LoadVisualElements(string path)
        {
            string extention = (Path.GetExtension(path));
            if (extention == "")
            {
                {
                    DirectoryInfo info = new DirectoryInfo(path);
                    FileInfo[] fileInfo = info.GetFiles();
                    foreach (var file in fileInfo)
                        LoadVisualElements(file.FullName);
                }


                {
                    DirectoryInfo info = new DirectoryInfo(path);
                    DirectoryInfo[] fileInfo = info.GetDirectories();
                    foreach (var file in fileInfo)
                        LoadVisualElements(file.FullName);
                }
            }

			else {
                VisualElement result = VisualElement.LoadFile(path);

                if(result != null)
				    GetVisualElements().Add(ScriptableObject.CreateInstance<StyleVisualElement>().Init(result));
			}
        }

        public string[] GetFileFilters()
        {
            List<string> filters = new List<string>();

            filters.Add("*.xml");
            filters.Add("*.json");
            filters.Add("*.png");
            filters.Add("*.obj");

            return filters.ToArray();
        }




		int _selectedLayerTypeIndex = 0;
		public void SetLayerTypeToCreate(int layerTypeIndex) {
			_selectedLayerTypeIndex = layerTypeIndex;
		}
		public int GetLayerTypeToCreate() {
			return _selectedLayerTypeIndex;
		}
		public string[] GetLayerTypes() {
			string [] types =  new string[_layerTypes.Count];
			for(int i =0; i < _layerTypes.Count;i++) {
				types[i] = _layerTypes[i].GetLabel();
			}
			return types;
		}
		public void CreateLayer() {
            //_layers.Add(ScriptableObject.CreateInstance<StyleTestLayer>().Init());
			_style.AddLayer(((StyleLayer)ScriptableObject.CreateInstance(_layerTypes[GetLayerTypeToCreate()].GetType().ToString())).CopyLayer(_layerTypes[GetLayerTypeToCreate()]));
		}




        int _selectedCustomVariableTypeIndex = 0;
        public void SetCustomVariableTypeToCreate(int variableType)
        {
            _selectedCustomVariableTypeIndex = variableType;
        }
        public int GetCustomVariableTypeToCreate()
        {
            return _selectedCustomVariableTypeIndex;
        }
        public string[] GetCustomVariableTypes()
        {
            string[] variableTypes = new string[_variableTypes.Count];
            for (int i = 0; i < _variableTypes.Count; i++)
            {
                variableTypes[i] = _variableTypes[i].GetLabel();
            }
            return variableTypes;
        }
        public void CreateCustomVariable()
        {
            //_layers.Add(ScriptableObject.CreateInstance<StyleTestLayer>().Init());
            string type = _variableTypes[GetCustomVariableTypeToCreate()].GetType().ToString();
            StyleVariable variable = (((StyleVariable)ScriptableObject.CreateInstance(type)).CopyVariable(_variableTypes[GetCustomVariableTypeToCreate()]));
            GetUserVariableController().AddVariable(variable);
        }


        [SerializeField] Canvas _CanvasPrefab;
		[SerializeField] List<Canvas> _canvases;

		public List<Canvas> GetCanvases() {
			if(_canvases == null) _canvases = new List<Canvas>();
			return _canvases;
		}
		public void AddCanvas() {
			Canvas c = Instantiate(_CanvasPrefab);
			c.SetStyle(_style);
		   	GetCanvases().Add(c);
		}

		public void RemoveCanvas(Canvas canvas) {
			GetCanvases().Remove(canvas);
			DestroyImmediate(canvas.gameObject);
		}

		public void RemoveModule(StyleModule module) {
			if(module is StyleVisualElement) {
				GetVisualElements().Remove(module);
			} else if(module is StyleLayer) {
				GetLayers().Remove((StyleLayer)module);
			} else if(module is StyleDataVariable) {
				GetVariables().Remove(module);
			} else if(module is StyleCustomVariable) {
                GetUserVariables().Remove((StyleVariable)module);
            }


			for(int i = 0; i < module.GetNumberOfSubmodules(); i++) {
                if(module.GetSubmodule(i) is StyleSocket) {
                    StyleSocket socket = (StyleSocket)module.GetSubmodule(i);
				    ClearSocket(socket);
                } else {
                   RemoveModule(module.GetSubmodule(i));  
                }
				
			}

		}

    }

}
