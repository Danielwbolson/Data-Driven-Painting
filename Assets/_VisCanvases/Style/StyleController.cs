using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System;

namespace SculptingVis
{
    // [ExecuteInEditMode]
    public class StyleController : MonoBehaviour
    {


        public virtual void UpdateRemoteAssets() {
            System.Net.WebClient client = new WebClient();
            Debug.Log(Application.persistentDataPath);
            //client.DownloadFile(new Uri(https://github.com/joh08230/SculptingVisFiles/blob/master/zep.vti?raw=true), Path.Combine(Application.persistentDataPath,"zep.vti"));
            WWW www = new WWW("https://github.com/joh08230/SculptingVisFiles/raw/master/zep.vti");
            while(!www.isDone);
            System.IO.File.WriteAllBytes( (Application.persistentDataPath + "/" + "zep.vti"), www.bytes);

        }
        public void LoadBrainPreset() {
            if(!File.Exists(Application.persistentDataPath + "/"  + "zep.vti")) {
                UpdateRemoteAssets();
            }
            LoadData(Application.persistentDataPath + "/"  + "zep.vti");

            LoadVisualElements(Application.streamingAssetsPath+"/"+"tests" + "/" + "twist_line_1.glyph");
            LoadVisualElements(Application.streamingAssetsPath+"/"+"tests" + "/" + "blue.xml");

            LoadVisualElements(Application.streamingAssetsPath+"/"+"tests" + "/" + "red.xml");


            int regularGridTypeID = -1;
            for(int i = 0; i < _customDatasetTypes.Count; i++) {
                if(_customDatasetTypes[i].GetLabel().Contains("Regular") ) 
                    regularGridTypeID = i;
            }
            if(regularGridTypeID == -1) return;
            SetCustomVariableTypeToCreate(regularGridTypeID);
            CreateCustomDataset();

            StyleRegularGridCustomDataset regularVolume = (StyleRegularGridCustomDataset)GetCustomDatasets()[GetCustomDatasets().Count-1];




            int volumeTypeID = -1;
            for(int i = 0; i < _layerTypes.Count; i++) {
                if(_layerTypes[i].GetLabel().Contains("Volume") ) 
                    volumeTypeID = i;
            }
            if(volumeTypeID == -1) return;

            SetLayerTypeToCreate(volumeTypeID);
            CreateLayer();
            StyleVolumeLayer volLayer = (StyleVolumeLayer)GetLayers()[GetLayers().Count-1];

            int glyphLayerTypeID = -1;
            for(int i = 0; i < _layerTypes.Count; i++) {
                if(_layerTypes[i].GetLabel().Contains("Simple Glyph Layer") ) 
                    glyphLayerTypeID = i;
            }
            if(glyphLayerTypeID == -1) return;

            SetLayerTypeToCreate(glyphLayerTypeID);
            CreateLayer();

            StyleSimpleGlyphLayer glyphLayer1 = (StyleSimpleGlyphLayer)GetLayers()[GetLayers().Count-1];

            CreateLayer();

            StyleSimpleGlyphLayer glyphLayer2 = (StyleSimpleGlyphLayer)GetLayers()[GetLayers().Count-1];

            int faVariable = -1;
            for(int i = 0; i < GetVariables().Count; i++) {
                if(GetVariables()[i].GetLabel().Contains("FA")) {
                    faVariable = i;
                }
            }


            int n1Variable = -1;
            for(int i = 0; i < GetVariables().Count; i++) {
                if(GetVariables()[i].GetLabel().Contains("n1")) {
                    n1Variable = i;
                }
            }

            int n2Variable = -1;
            for(int i = 0; i < GetVariables().Count; i++) {
                if(GetVariables()[i].GetLabel().Contains("n2")) {
                    n2Variable = i;
                }
            }

            int f1Variable = -1;
            for(int i = 0; i < GetVariables().Count; i++) {
                if(GetVariables()[i].GetLabel().Contains("f1")) {
                    f1Variable = i;
                }
            }


            int f2Variable = -1;
            for(int i = 0; i < GetVariables().Count; i++) {
                if(GetVariables()[i].GetLabel().Contains("f2")) {
                    f2Variable = i;
                }
            }

            
             int glyphID = -1;
            for(int i = 0; i < GetVisualElements().Count; i++) {

                if(GetVisualElements()[i] != null)
                if(((StyleVisualElement)GetVisualElements()[i]).GetVisualElement() != null) 
                if(((StyleVisualElement)GetVisualElements()[i]).GetVisualElement().GetName() != null)
                if (((StyleVisualElement)GetVisualElements()[i]).GetVisualElement().GetName().Contains("twist")) {
                    glyphID = i;
                }
            }


            int blueMapID = -1;
            for(int i = 0; i < GetVisualElements().Count; i++) {
                if(GetVisualElements()[i] != null &&  ((StyleVisualElement)GetVisualElements()[i]).GetVisualElement().GetName().Contains("30T1")) {
                    blueMapID = i;
                }
            }

            int redMapID = -1;
            for(int i = 0; i < GetVisualElements().Count; i++) {
                if(GetVisualElements()[i] != null &&  ((StyleVisualElement)GetVisualElements()[i]).GetVisualElement().GetName().Contains("R5")) {
                    redMapID = i;
                }
            }

            // Create Links


            // Generate Regular grid
           StyleLink domainLink = new StyleLink();

            domainLink.SetSource((StyleSocket)GetVariables()[faVariable].GetSubmoduleByLabel(""));

            
            domainLink.SetDestination((StyleSocket)regularVolume.GetSubmoduleByLabel("Domain"));


            AddLink(domainLink);
            
            StyleSocket countSocket = (StyleSocket)regularVolume.GetSubmodule(2);
            ((Range<int>)(countSocket.GetInput())).value = 35;


            regularVolume.ComputeDataset();





           StyleLink volumeVariableLink = new StyleLink();
            volumeVariableLink.SetSource((StyleSocket)GetVariables()[faVariable].GetSubmoduleByLabel(""));
            volumeVariableLink.SetDestination((StyleSocket) volLayer.GetSubmoduleByLabel("Volume"));
            AddLink(volumeVariableLink);



            for(int i =0; i < GetCustomDatasets()[0].GetNumberOfSubmodules(); i++) {
                Debug.Log(i + ":" + GetCustomDatasets()[0].GetSubmodule(i).GetLabel() + " " + GetCustomDatasets()[0].GetSubmodule(i) + " " + GetCustomDatasets()[0].GetSubmodule(i).GetType());
            }

            StyleLink glyphAnchor1Link = new StyleLink();
            glyphAnchor1Link.SetSource((StyleSocket)GetCustomDatasets()[0].GetSubmodule(4).GetSubmodule(0));
            glyphAnchor1Link.SetDestination((StyleSocket)glyphLayer1.GetSubmoduleByLabel("Anchor"));
            AddLink(glyphAnchor1Link);


            StyleLink glyphAnchor2Link = new StyleLink();
            glyphAnchor2Link.SetSource((StyleSocket)GetCustomDatasets()[0].GetSubmodule(4).GetSubmodule(0));
            glyphAnchor2Link.SetDestination((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Anchor"));
            AddLink(glyphAnchor2Link);



            ((Range<int>)(((StyleSocket)glyphLayer1.GetSubmoduleByLabel("Max glyphs")).GetInput())).value = 70000;
            ((Range<int>)(((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Max glyphs")).GetInput())).value = 70000;
            ((Range<float>)(((StyleSocket)glyphLayer1.GetSubmoduleByLabel("Glyph scale")).GetInput())).value = 4;
            ((Range<float>)(((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Glyph scale")).GetInput())).value = 4;


            StyleLink glyphDirection1Link = new StyleLink();
            glyphDirection1Link.SetSource((StyleSocket)GetVariables()[n1Variable].GetSubmoduleByLabel(""));
            glyphDirection1Link.SetDestination((StyleSocket)glyphLayer1.GetSubmoduleByLabel("Direction"));
            AddLink(glyphDirection1Link);


            StyleLink glyphDirection2Link = new StyleLink();
            glyphDirection2Link.SetSource((StyleSocket)GetVariables()[n2Variable].GetSubmoduleByLabel(""));
            glyphDirection2Link.SetDestination((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Direction"));
            AddLink(glyphDirection2Link);



            ((Range<bool>)(((StyleSocket)glyphLayer1.GetSubmoduleByLabel("Use Plane 1")).GetInput())).value = true;
            ((Range<bool>)(((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Use Plane 1")).GetInput())).value = true;

            ((Range<bool>)(((StyleSocket)glyphLayer1.GetSubmoduleByLabel("Color Modifier").GetSubmoduleByLabel("Use Variable")).GetInput())).value = true;
            ((Range<bool>)(((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Color Modifier").GetSubmoduleByLabel("Use Variable")).GetInput())).value = true;
            ((Range<bool>)(((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Color Modifier").GetSubmoduleByLabel("Flip Colormap")).GetInput())).value = true;

            ((MinMax<float>)(((StyleSocket)glyphLayer1.GetSubmoduleByLabel("Color Modifier").GetSubmoduleByLabel("Data Range")).GetInput())).upperValue = 0.5f;
            ((MinMax<float>)(((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Color Modifier").GetSubmoduleByLabel("Data Range")).GetInput())).upperValue = 0.5f;

            StyleLink glyph1Link = new StyleLink();
            glyph1Link.SetSource((StyleSocket)GetVisualElements()[glyphID].GetSubmoduleByLabel(""));
            glyph1Link.SetDestination((StyleSocket)glyphLayer1.GetSubmoduleByLabel("Glyph"));
            AddLink(glyph1Link);

            StyleLink glyph2Link = new StyleLink();
            glyph2Link.SetSource((StyleSocket)GetVisualElements()[glyphID].GetSubmoduleByLabel(""));
            glyph2Link.SetDestination((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Glyph"));
            AddLink(glyph2Link);



            StyleLink glyphColor1Link = new StyleLink();
            glyphColor1Link.SetSource((StyleSocket)GetVisualElements()[blueMapID].GetSubmoduleByLabel(""));
            glyphColor1Link.SetDestination((StyleSocket)glyphLayer1.GetSubmoduleByLabel("Color Modifier").GetSubmoduleByLabel("Colormap"));
            AddLink(glyphColor1Link);

            StyleLink glyphColor2Link = new StyleLink();
            glyphColor2Link.SetSource((StyleSocket)GetVisualElements()[redMapID].GetSubmoduleByLabel(""));
            glyphColor2Link.SetDestination((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Color Modifier").GetSubmoduleByLabel("Colormap"));
            AddLink(glyphColor2Link);

            StyleLink glyphColorVar1Link = new StyleLink();
            glyphColorVar1Link.SetSource((StyleSocket)GetVariables()[f1Variable].GetSubmoduleByLabel(""));
            glyphColorVar1Link.SetDestination((StyleSocket)glyphLayer1.GetSubmoduleByLabel("Color Modifier").GetSubmoduleByLabel("Variable"));
            AddLink(glyphColorVar1Link);

            StyleLink glyphColorVar2Link = new StyleLink();
            glyphColorVar2Link.SetSource((StyleSocket)GetVariables()[f2Variable].GetSubmoduleByLabel(""));
            glyphColorVar2Link.SetDestination((StyleSocket)glyphLayer2.GetSubmoduleByLabel("Color Modifier").GetSubmoduleByLabel("Variable"));
            AddLink(glyphColorVar2Link);
            
            planeMaxes[0].y = 0.525f;
            planeMins[0].y = 0.5f;

            planeMaxes[1].y = 0.5f;
            planeMins[1].y = 0.45f;


            RollData(-90);
            ZoomCamera(-10);

        }
        
        public Mesh sphere;
        public Mesh cylinder;

        public Camera sceneCamera;
        public Transform cameraPivot;
        public Transform dataVerticalPivot;
        public Transform dataHorizontalPivot;

        public Color backdropColor;

        protected Vector3[] planeMins;
        protected Vector3[] planeMaxes;

        public Vector3[] GetPlaneMins() {
            if(planeMins == null) planeMins = new Vector3[3];
            return planeMins;
        }

        public Vector3[] GetPlaneMaxes() {
            if(planeMaxes == null) {
                planeMaxes = new Vector3[3];
                for(int i =0; i < 3; i ++)  {
                planeMaxes[i].x =1;
                planeMaxes[i].y =1;
                planeMaxes[i].z =1;
                }
            }
            return planeMaxes;
        }


        public void ZoomCamera(float amount) {
            sceneCamera.fieldOfView += amount;
        }

        public void MoveCamera(float amount) {
            sceneCamera.gameObject.transform.position += sceneCamera.gameObject.transform.forward*amount;
        }

        public void RotateData(float amount) {
            dataHorizontalPivot.eulerAngles += Vector3.up*amount;
        }

        public void RollData(float amount) {
            dataHorizontalPivot.eulerAngles += Vector3.right*amount;
        }
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
            GetCustomDatasets().Clear();

            GetVisualElements().Clear();
            GetUserVariables().Clear();
            while(GetCanvases().Count > 0)
                RemoveCanvas(GetCanvases()[GetCanvases().Count-1]);
            //AddCanvas();
            AddCanvas();
            //GetCanvases()[0].SetBounds(new Vector3(5,4,5));
           // GetCanvases()[0].gameObject.transform.SetPositionAndRotation(new Vector3(0,0,15),Quaternion.identity);
            /*



            VTKFileAsset contourData = new VTKFileAsset();
            contourData.SetPath("/Users/sethjohnson/NSF-Sculpting-Vis-Platform/unity/VisualizationPlatform/Assets/StreamingAssets/example_data/VTK/contour.vtp");

            Debug.Log(contourData.GetDataset().GetClassName());           
            SmartData.Dataset dataset = new SmartData.Dataset();
            dataset.SetName("Test Dataset");


            SmartData.Variable anchoredVariable = new SmartData.AnchoredVariable(dataset).Init();
            anchoredVariable.SetName("Anchored Variable A");

            SmartData.Variable anchoredVariable2 = new SmartData.AnchoredVariable(dataset).Init();
            anchoredVariable2.SetName("Anchored Variable B");


            SmartData.Variable continuousVariable = new SmartData.ContinuousVariable(dataset).Init();
            continuousVariable.SetName("Continuous Variable A");

            dataset.AddVariable(anchoredVariable);
            dataset.AddVariable(anchoredVariable2);
            dataset.AddVariable(continuousVariable);

            SmartData.Anchor anchor = new SmartData.Anchor(dataset).Init();
            anchor.SetName( "Original Anchor");
            SmartData.VTKPointDataStrategy vtkStrat = new SmartData.VTKPointDataStrategy();
            vtkStrat.SetDatasetFile(contourData);

            anchor.SetDataStrategy(vtkStrat);

            dataset.SetSourceAnchor(anchor);

            SmartData.Anchor anchor2 = new SmartData.Anchor(dataset).Init();
            // anchor2.SetNumberOfPoints(3);
            anchor2.SetName( "Derived Anchor");
            SmartData.RandomSubsetPointDataStrategy strat = (new SmartData.RandomSubsetPointDataStrategy()).Init(dataset.GetSourceAnchor());

            anchor2.SetDataStrategy(strat);

            dataset.AddAnchor(anchor);
            dataset.AddAnchor(anchor2);

            SmartData.Datastream ds = dataset.GetDatastream( anchoredVariable2,anchor);

            Debug.Log(ds);

            anchor2.TriggerUpdate();

            // SmartData.Dataset datasetModule = ScriptableObject.CreateInstance<SmartData.Dataset>();
            GetDatasets().Add(dataset);



            // _variables.Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init());
            // _variables.Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init());
            // _variables.Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init());
            // _variables.Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init());

            */

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
        List<StyleDataset> _customDatasetTypes;

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
        List<SmartData.Dataset> _datasets;

        [SerializeField]
        List<StyleDataset> _customDatasets;

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

                if(GetLinksBySource().ContainsKey(socket.GetUniqueIdentifier())) {
                    foreach(StyleLink link in GetLinksBySource()[socket.GetUniqueIdentifier()]) {
                        RemoveLink(link, false);
                    }

				    GetLinksBySource()[socket.GetUniqueIdentifier()].Clear();
                }


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

        public List<SmartData.Dataset> GetDatasets()
        {
            if (_datasets == null) _datasets = new List<SmartData.Dataset>();

            return _datasets;
        }

        public List<StyleDataset> GetCustomDatasets()
        {
            if (_customDatasets == null) _customDatasets = new List<StyleDataset>();

            return _customDatasets;
        }
        // Use this for initialization
        void Start()
        {
            Reset();
            //#if UNITY_EDITOR
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            //Application.targetFrameRate = 45;
            //#endif
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
                dataset.Update();
            }

            // foreach(var dataset in GetCustomDatasets()) {
            //     dataset.UpdateModule();
            // }
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
                VisualElement[] results = VisualElement.LoadFile(path);

                if(results != null) {
                    foreach(var r in results) {
				        GetVisualElements().Add(ScriptableObject.CreateInstance<StyleVisualElement>().Init(r));
                    }
                }
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




        int _selectedCustomDatasetTypeIndex = 0;
        public void SetCustomVariableTypeToCreate(int variableType)
        {
            _selectedCustomDatasetTypeIndex = variableType;
        }
        public int GetCustomDatasetTypeToCreate()
        {
            return _selectedCustomDatasetTypeIndex;
        }
        public string[] GetCustomDatasetTypes()
        {
            string[] datasetTypes = new string[_customDatasetTypes.Count];
            for (int i = 0; i < _customDatasetTypes.Count; i++)
            {
                datasetTypes[i] = _customDatasetTypes[i].GetLabel();
            }
            return datasetTypes;
        }
        public void CreateCustomDataset()
        {
            //_layers.Add(ScriptableObject.CreateInstance<StyleTestLayer>().Init());
            string type = _customDatasetTypes[GetCustomDatasetTypeToCreate()].GetType().ToString();
            StyleDataset dataset = (((StyleDataset)ScriptableObject.CreateInstance(type)).CopyDataset(_customDatasetTypes[GetCustomDatasetTypeToCreate()]));
            GetCustomDatasets().Add(dataset);
        }
        





        [SerializeField] Canvas _CanvasPrefab;
		[SerializeField] List<Canvas> _canvases;

		public List<Canvas> GetCanvases() {
			if(_canvases == null) _canvases = new List<Canvas>();
			return _canvases;
		}
		public void AddCanvas() {
			Canvas c = Instantiate(_CanvasPrefab,dataHorizontalPivot,false);
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
			} else if(module is StyleCustomDataset) {
                GetCustomDatasets().Remove((StyleCustomDataset)module);
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
