using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SculptingVis;
using System.IO;

public class SculptingVisWindow : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    SculptingVisWindow window;

    static StyleController GetStyleController()
    {
        return (StyleController)FindObjectOfType(typeof(StyleController));
    }
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/SculptingVis")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SculptingVisWindow window = (SculptingVisWindow)EditorWindow.GetWindow(typeof(SculptingVisWindow));
        window.Show();
        window.scrollView = new Vector2[7];
    }

    Vector4 CubicSolve(float x_1, float v_1, float x_2, float v_2)
    {
        Vector4 terms = new Vector4();
        terms[0] = 2 * x_1 - 2 * x_2 + v_1 + v_2;
        terms[1] = (v_2 - v_1 - 3 * terms[0]) / 2;
        terms[2] = v_1;
        terms[3] = x_1;
        return terms;
    }

    float CubicEvaluate(Vector4 terms, float t)
    {
        Vector4 powers = new Vector4(t * t * t, t * t, t, 1);
        return Vector4.Dot(terms, powers);
    }

    void DrawWire(Rect startRect, Vector2 endPos) {
        if(startRect.center.x < endPos.x) {
            Vector2 startPos = startRect.center+Vector2.right*startRect.width/2;
            DrawWire(startPos,Vector2.right,endPos,Vector2.left);
        } else {
            Vector2 startPos = startRect.center+Vector2.left*startRect.width/2;
            DrawWire(startPos,Vector2.left,endPos,Vector2.right);    
        }
    }

    void DrawWire(Rect startRect, Rect endRect) {
        if(startRect.center.x < endRect.center.x) {
            Vector2 startPos = startRect.center+Vector2.right*startRect.width/2;
            Vector2 endPos = endRect.center+Vector2.left*endRect.width/2;
            DrawWire(startPos,Vector2.right,endPos,Vector2.left);
        } else {
            Vector2 startPos = startRect.center+Vector2.left*startRect.width/2;
            Vector2 endPos = endRect.center+Vector2.right*endRect.width/2;
            DrawWire(startPos,Vector2.left,endPos,Vector2.right);    
        }
    }
    void DrawWire(Vector2 startPosition, Vector2 endPosition)
    {

    }
    Material mat;

    void DrawWire(Vector2 startPosition, Vector2 startDirection, Vector2 endPosition, Vector2 endDirection)
    {

        if (mat == null)
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            mat = new Material(shader);
        }

        Vector2 startVector = Vector3.Project(new Vector3(endPosition.x - startPosition.x, endPosition.y - startPosition.y, 0), new Vector3(startDirection.x, startDirection.y, 0));
        Vector2 endVector = -Vector3.Project(new Vector3(startPosition.x - endPosition.x, startPosition.y - endPosition.y, 0), new Vector3(endDirection.x, endDirection.y, 0));

        Vector4 Xterms = CubicSolve(startPosition.x, startVector.x, endPosition.x, endVector.x);
        Vector4 Yterms = CubicSolve(startPosition.y, startVector.y, endPosition.y, endVector.y);

        int steps = 100;
        for (int i = 0; i < steps; i += 1)
        {
            float t_1 = i / (float)steps;
            float t_2 = (i + 1) / (float)steps;

            Vector2 p_1 = new Vector2(CubicEvaluate(Xterms, t_1), CubicEvaluate(Yterms, t_1));
            Vector2 p_2 = new Vector2(CubicEvaluate(Xterms, t_2), CubicEvaluate(Yterms, t_2));

            GL.PushMatrix();
            //GL.Clear(true, false, Color.black);
            mat.SetPass(0);

            GL.Begin(GL.LINES);
            GL.Color(Color.black);

            GL.Vertex3(p_1.x, p_1.y, 0);
            GL.Vertex3(p_2.x, p_2.y, 0);
            GL.End();
            GL.PopMatrix();

        }
    }

    Vector2 mousePos;
    Vector2 boxPosition;
    Vector2[] scrollView;
    bool showPosition = false;


    Dictionary<string, Vector2> _scrollPositions;
    struct SocketHook
    {
        Rect _screenLocation;
        int _instanceID;
    }
    Dictionary<string, Rect> _socketHooks;
    Dictionary<string, StyleSocket> _sockets;

    Vector2Int activeLink;

    StyleSocket activeSource = null;
    Dictionary<string,bool> _foldoutStates;
    public Dictionary<string,bool> GetFoldoutStates() {
        if(_foldoutStates == null) _foldoutStates = new Dictionary<string, bool>();
        return _foldoutStates;
    }
    public bool GetFoldoutState(string id) {
        if(!GetFoldoutStates().ContainsKey(id)) GetFoldoutStates()[id] = false;
        return GetFoldoutStates()[id];
    }

    void DrawDatasetModule(SculptingVis.SmartData.Dataset module, Rect next, bool showInputs= true, bool showOutputs = true) {

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        GUILayout.Label(module.GetLabel());

        GUILayout.EndHorizontal();


        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label(module.GetSourceAnchor().GetLabel() +" (" + module.GetSourceAnchor().GetNumberOfPoints() + " points)");
        GUILayout.EndHorizontal();

        foreach(var anchor in module.GetAnchors()) {
            if(anchor != module.GetSourceAnchor()) {
                GUILayout.BeginHorizontal();
                GetFoldoutStates()[""+anchor.GetHashCode()] = EditorGUILayout.Foldout(GetFoldoutState(anchor.GetHashCode()+""), anchor.GetLabel() +" (" + anchor.GetNumberOfPoints() + " points)");
                GUILayout.Button("-",GUILayout.Width(20));
                GUILayout.EndHorizontal();

                if(GetFoldoutState(anchor.GetHashCode()+"")) {
                    GUILayout.BeginVertical();
                    for(int i = 0; i < anchor.GetDataStrategy().GetNumberOfSubmodules(); i++) {
                        if(anchor.GetDataStrategy().GetSubmodule(i) is StyleSocket)
                        DrawSocket((StyleSocket)anchor.GetDataStrategy().GetSubmodule(i),next,true,true);
                    }
                    GUILayout.EndVertical();
                }
            }
        }
        GUILayout.EndVertical();


        foreach(var anchoredVar in module.GetAnchoredVariables()) {
            if(anchoredVar != null) {
                GUILayout.BeginHorizontal();
                GUILayout.Label(anchoredVar.GetLabel());
                GUILayout.EndHorizontal();
            }
        }

        foreach(var continuousVar in module.GetContinuousVariables()) {
            if(continuousVar != null) {
                GUILayout.BeginHorizontal();
                GUILayout.Label(continuousVar.GetLabel());
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }


    void DrawSocket(StyleSocket socket, Rect nest, bool showInputs = true, bool showOutputs = true) {
            if (socket.IsInput() && !showInputs) return;
            if (socket.IsOutput() && !showOutputs) return;

            bool inputHookLeft = false;
            bool inputHookRight = false;
            if (socket.GetModule() is StyleLayer && socket is StyleTypeSocket)
                inputHookLeft = true;
            else if (socket.GetModule() is StyleLayer && socket is VariableSocket)
                inputHookRight = true;
            else if(socket.GetModule() is StyleDataVariable && socket.GetLabel() != "") {
                inputHookRight = true;
            }
            if (socket.GetModule() is StyleCustomVariable && socket.IsInput())
                inputHookLeft = true;
            if (socket.GetModule() is StyleCustomVariable && socket.IsOutput() && socket.GetLabel() != "")
                inputHookRight = true;


            BeginSocketHook(socket,nest);


            
                GUILayout.Label(socket.GetLabel());
                if(socket is StyleTypeSocket<Range<int>>) {
                    int A = ((Range<int>)socket.GetInput()).value;
                    ((Range<int>)socket.GetInput()).value = EditorGUILayout.IntSlider(((Range<int>)socket.GetInput()).value,((Range<int>)socket.GetInput()).lowerBound,((Range<int>)socket.GetInput()).upperBound);
                    if(A != ((Range<int>)socket.GetInput()).value)
                        socket.GetModule().UpdateModule();
                }

                if(socket is StyleTypeSocket<Range<float>>) {
                    float A = ((Range<float>)socket.GetInput()).value;
                    ((Range<float>)socket.GetInput()).value = EditorGUILayout.Slider(((Range<float>)socket.GetInput()).value,((Range<float>)socket.GetInput()).lowerBound,((Range<float>)socket.GetInput()).upperBound);
                    if(A != ((Range<float>)socket.GetInput()).value)
                        socket.GetModule().UpdateModule();
                }
                if(socket is StyleTypeSocket<Range<bool>>) {
                    bool A = ((Range<bool>)socket.GetInput()).value;
                    ((Range<bool>)socket.GetInput()).value = EditorGUILayout.Toggle(((Range<bool>)socket.GetInput()).value);
                    if(A != ((Range<bool>)socket.GetInput()).value)
                        socket.GetModule().UpdateModule();
                }

                if(socket is StyleTypeSocket<Objectify<Color>>) {
                    Color A = ((Objectify<Color>)socket.GetInput()).value;
                    ((Objectify<Color>)socket.GetInput()).value = EditorGUILayout.ColorField(A);
                    if(A != ((Objectify<Color>)socket.GetInput()).value) {
                        socket.GetModule().UpdateModule();
                    }
                }


                GUILayout.FlexibleSpace();
                EndSocketHook(socket,nest);


    }
    void DrawStyleModule(StyleModule module, Rect nest, bool foldup, bool showInputs = true, bool showOutputs = true)
    {
        // if(module is SculptingVis.SmartData.Dataset) {
        //     DrawDatasetModule((SculptingVis.SmartData.Dataset)module, nest, showInputs, showOutputs);
        //     return;
        // }
        int submodule_index = 0;
        bool labelOutputHook = false;
        bool labelOutputHookLeft = false;
        bool labelOutputHookRight = false;
        if (showOutputs && module.GetNumberOfSubmodules() > 0 && module.GetSubmodule(0) is StyleSocket && ((StyleSocket)module.GetSubmodule(0)).IsOutput())
        {
            labelOutputHook = true;
            StyleSocket socket = ((StyleSocket) module.GetSubmodule(0));
            // Temporary inspection to see which column it's in, and which it's going to
            if (module is StyleVisualElement)
            {
                if (socket.GetOutput() is VisualElement)
                    labelOutputHookRight = true;
            }
            else if (module is StyleVariable && socket.GetLabel() == "")
                labelOutputHookLeft = true;

        }

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        if(labelOutputHook)
            BeginSocketHook((StyleSocket)module.GetSubmodule(submodule_index),nest);

        // if (labelOutputHookLeft && module.GetSubmodule(submodule_index) is StyleSocket)
        // {
        //     if (labelOutputHook) DrawSocketHook((StyleSocket)module.GetSubmodule(submodule_index++), nest);

        // }

        // if (labelOutputHookRight) {
            
        //     if(GUILayout.Button("-",EditorStyles.miniButton,GUILayout.MaxWidth(20))) {
        //         GetStyleController().RemoveModule(module);
        //     }
        // } 

        // Draw Module Label
        float x = EditorGUIUtility.fieldWidth;
        EditorGUIUtility.fieldWidth = 10;
        if(module.GetNumberOfSubmodules() >(labelOutputHook?1:0) )
            GetFoldoutStates()[""+module.GetHashCode()] = EditorGUILayout.Foldout(GetFoldoutState(module.GetHashCode()+""),GUIContent.none, false);
        EditorGUIUtility.fieldWidth = x;

        GUILayout.Label(module.GetLabel());
        GUILayout.FlexibleSpace();

        // End Draw Module label
        if(!labelOutputHook) GUILayout.FlexibleSpace();

        // if (!labelOutputHookRight) {
            // if(labelOutputHook)            GUILayout.FlexibleSpace();

            if(GUILayout.Button("-",EditorStyles.miniButton,GUILayout.MaxWidth(20))) {
                GetStyleController().RemoveModule(module);
            }
        // } 

        if(module is StyleVisualElement) {
            if(true) {
                Texture t = ((StyleVisualElement)module).GetVisualElement().GetPreviewImage();
                float aspectRatio = ((StyleVisualElement)module).GetVisualElement().GetPreviewImageAspectRatio();
                Rect r = GUILayoutUtility.GetRect(30*aspectRatio,30);

                GUI.DrawTexture(r,t,ScaleMode.ScaleToFit,true,aspectRatio);

            }


        }


        // if (labelOutputHookRight)
        // {
        //     StyleModule submod = module.GetSubmodule(submodule_index++);
        //     if(submod is StyleSocket) {
        //         StyleSocket socket = (StyleSocket)submod;
        //         if (labelOutputHook) DrawSocketHook(socket, nest);
        //     }
        // }

        GUILayout.EndHorizontal();

        if(labelOutputHook)
            EndSocketHook((StyleSocket)module.GetSubmodule(submodule_index++),nest);

        if(GetFoldoutState(module.GetHashCode()+"")) {
            for (; submodule_index < module.GetNumberOfSubmodules(); submodule_index++)
            {
                StyleModule submod = module.GetSubmodule(submodule_index);
                if(submod is StyleSocket) {
                    StyleSocket socket = (StyleSocket)submod;
                    DrawSocket(socket,nest,showInputs,showOutputs);
                }
                else {
                    DrawStyleModule(submod,nest,true,true,true);
                }



            }
        }



        GUILayout.EndVertical();
    }

    void DrawSocketHook(StyleSocket socket, Rect nest)
    {
        // if (socket != null)
        // {
            bool disabled = false;
            if (activeSource != null && !socket.DoesAccept(activeSource))
                disabled = true;

            EditorGUI.BeginDisabledGroup(disabled);
            //GUILayout.Label(socket.GetUniqueIdentifier());
            GUILayout.Box("", socket.IsOutput() ? EditorStyles.radioButton : EditorStyles.miniButton);
            EditorGUI.EndDisabledGroup();
            if (Event.current.type == EventType.Repaint)
            {
                Rect hook = GUILayoutUtility.GetLastRect();
                hook.position += nest.position;
                _socketHooks[socket.GetUniqueIdentifier()] = hook;
                _sockets[socket.GetUniqueIdentifier()] = socket;
            }

            //Debug.Log("_socketHooks[" + module.GetSockets()[i] + "] = " + hook);
        // }

    }
    Dictionary<string, EditorGUILayout.HorizontalScope> _horzScopes;
    Dictionary<string,EditorGUILayout.HorizontalScope > GetHorizontalScopes() {
        if(_horzScopes == null) _horzScopes = new Dictionary<string, EditorGUILayout.HorizontalScope>();
        return _horzScopes;
    }
    void BeginSocketHook(StyleSocket socket,Rect nest)  {
         bool disabled = false;
        if (activeSource != null && !socket.DoesAccept(activeSource))
            disabled = true;
        EditorGUI.BeginDisabledGroup(disabled);

              Texture2D buttonBg = GUI.skin.button.normal.background;
              Texture2D[] buttonBGs = (Texture2D[])GUI.skin.button.normal.scaledBackgrounds.Clone();
            if (socket.GetInput() == null && socket.GetOutput() == null){  //set active based on condition
                GUI.skin.button.normal.background = GUI.skin.button.active.background;
                GUI.skin.button.normal.scaledBackgrounds = (Texture2D[])GUI.skin.button.active.scaledBackgrounds.Clone();
            }

        GetHorizontalScopes()[socket.GetUniqueIdentifier()] =  new EditorGUILayout.HorizontalScope("button");
        GUI.skin.button.normal.background = buttonBg;
        GUI.skin.button.normal.scaledBackgrounds = buttonBGs;

    }

    void EndSocketHook(StyleSocket socket, Rect nest) {
        if (Event.current.type == EventType.Repaint)
        {
            Rect hook = GetHorizontalScopes()[socket.GetUniqueIdentifier()].rect;
            hook.position += nest.position;
            _socketHooks[socket.GetUniqueIdentifier()] = hook;
            _sockets[socket.GetUniqueIdentifier()] = socket;

        }
        GetHorizontalScopes()[socket.GetUniqueIdentifier()].Dispose();
        EditorGUI.EndDisabledGroup();

    }
    void MakeSocketHook(StyleSocket socket, Rect hookRect, Rect nest)
    {
        // if (socket != null)
        // {
            bool disabled = false;
            if (activeSource != null && !socket.DoesAccept(activeSource))
                disabled = true;

            EditorGUI.BeginDisabledGroup(disabled);
            //GUILayout.Label(socket.GetUniqueIdentifier());
            GUILayout.Box("", socket.IsOutput() ? EditorStyles.radioButton : EditorStyles.miniButton);
            EditorGUI.EndDisabledGroup();
            if (Event.current.type == EventType.Repaint)
            {
                Rect hook = GUILayoutUtility.GetLastRect();
                hook.position += nest.position;
                _socketHooks[socket.GetUniqueIdentifier()] = hook;
                _sockets[socket.GetUniqueIdentifier()] = socket;
            }

            //Debug.Log("_socketHooks[" + module.GetSockets()[i] + "] = " + hook);
        // }

    }
    Rect[] _columns;
    bool showVisualElementLoader = false;
    bool showCanvasManager = false;
    bool showDataLoader = false;
    bool showCustomDataSettings = false;


    public enum OPTIONS
{
    CUBE = 0,
    SPHERE = 1,
    PLANE = 2
}
    public OPTIONS op;

    void OnGUI()
    {
        if(GetStyleController() == null) {
            GUILayout.Label("There is no StyleController object in this scene.");
            return;
        }
            
        if(!Application.isPlaying) {
            GUILayout.Label("For the time being, please run the scene to make changes.");
            return;
        }
        if (_columns == null) _columns = new Rect[7];



        if (_scrollPositions == null) _scrollPositions = new Dictionary<string, Vector2>();
        if (_socketHooks == null) _socketHooks = new Dictionary<string, Rect>();
        if (_sockets == null) _sockets = new Dictionary<string, StyleSocket>();


        if (Event.current.type == EventType.Repaint)
        {
            _socketHooks.Clear();
            _sockets.Clear();
        }



        //if (_socketLinks == null) _socketLinks = new List<Vector2Int>();

        Rect[] columns = new Rect[7];
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Reset"))
        {
            _socketHooks.Clear();
            _sockets.Clear();
            GetStyleController().Reset();
            _columns = null;
        }
        if (GUILayout.Button("Report"))
        {
            GetStyleController().Report();
        }
        //Rect workspace = GUILayoutUtility.GetRect(0,10000,0,10000);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(350));
        EditorGUILayout.BeginVertical("box");
        showVisualElementLoader = EditorGUILayout.Foldout(showVisualElementLoader, "Load Visual Elements");
        if (showVisualElementLoader)
        {

            if (GUILayout.Button("Load Folder"))
            {
                string path = EditorUtility.OpenFolderPanel("Select Folder containing glyphs or colormaps", Application.streamingAssetsPath + "/", "");
                if (path.Length != 0)
                {
                    GetStyleController().LoadVisualElements(path);
                }
            }
            if (GUILayout.Button("Load Files"))
            {
                string path = EditorUtility.OpenFilePanel("Select Visual Element", Application.streamingAssetsPath + "/","");
                if (path.Length != 0)
                {
                    GetStyleController().LoadVisualElements(path);
                }
            }


        }

        EditorGUILayout.EndVertical();


        columns[0] = GUILayoutUtility.GetRect(new GUIContent(""),GUIStyle.none,GUILayout.ExpandHeight(true));
        EditorGUILayout.EndVertical();

        GUILayoutUtility.GetRect(0, 50, 0, position.height); //GUILayout.FlexibleSpace();






        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(350));
        EditorGUILayout.BeginVertical("box");
        showCanvasManager = EditorGUILayout.Foldout(showCanvasManager, "Manage Canvases");
        if (showCanvasManager)
        {

            for(int c = 0; c < GetStyleController().GetCanvases().Count; c++) {
                SculptingVis.Canvas canvas = GetStyleController().GetCanvases()[c];
                GUILayout.BeginHorizontal();
                if(GUILayout.Button("Select")) {
                    
                }
                canvas.FitStyle(GUILayout.Toggle(canvas.IsFitting(),"Fit"));
                    
                
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("-",EditorStyles.miniButton,GUILayout.MaxWidth(20))) {
                    GetStyleController().RemoveCanvas(canvas);
                }
                GUILayout.EndHorizontal();
            }
           
            if (GUILayout.Button("Add Canvas"))
            {
                GetStyleController().AddCanvas();
            }
           

        }

        EditorGUILayout.EndVertical();

    
        columns[2] = GUILayoutUtility.GetRect(100, 300, 0, position.height);
        EditorGUILayout.EndVertical();
        // if (!_scrollPositions.ContainsKey("Layers")) _scrollPositions["Layers"] = new Vector2(0, 0);
        // _scrollPositions["Layers"] = EditorGUILayout.BeginScrollView(_scrollPositions["Layers"],false,true,GUILayout.MaxWidth(200));


        // GUILayout.Label("Hello");

        // EditorGUILayout.EndScrollView();

        GUILayoutUtility.GetRect(0, 50, 0, position.height); //GUILayout.FlexibleSpace();




        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(350));
        EditorGUILayout.BeginVertical("box");
        showDataLoader = EditorGUILayout.Foldout(showDataLoader, "Load Data");
        if (showDataLoader)
        {

            // if (GUILayout.Button("Load Folder"))
            // {
            //     string path = EditorUtility.OpenFolderPanel("Select Folder containing glyphs or colormaps", "", "");
            //     if (path.Length != 0)
            //     {
            //         GetStyleController().LoadVisualElements(path);
            //     }
            // }
            if (GUILayout.Button("Load File"))
            {
                string path = EditorUtility.OpenFilePanel("Select VTK file", Application.streamingAssetsPath + "/example_data/VTK/","");
                if (path.Length != 0)
                {
                    GetStyleController().LoadData(path);
                }
            }

        }

        EditorGUILayout.EndVertical();

        columns[4] = GUILayoutUtility.GetRect(100, 300, 0, position.height);
        EditorGUILayout.EndVertical();



        GUILayoutUtility.GetRect(0, 50, 0, position.height); //GUILayout.FlexibleSpace();




        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(350));
        EditorGUILayout.BeginVertical("box");
        showCustomDataSettings = EditorGUILayout.Foldout(showCustomDataSettings, "Custom Variable Settings");
        if (showCustomDataSettings)
        {

            // if (GUILayout.Button("Create new Point Field"))
            // {
            //     string path = EditorUtility.OpenFilePanel("Select VTK file", Application.streamingAssetsPath + "/example_data/VTK/","");
            //     if (path.Length != 0)
            //     {
            //         GetStyleController().LoadData(path);
            //     }
            // }

        }

        EditorGUILayout.EndVertical();

        columns[6] = GUILayoutUtility.GetRect(100, 300, 0, position.height);
        EditorGUILayout.EndVertical();


        // GUILayoutUtility.GetRect(0, 50, 0, position.height); //GUILayout.FlexibleSpace();


        // columns[6] = GUILayoutUtility.GetRect(100, 300, 0, position.height);



        if (Event.current.type == EventType.Repaint)
            for (int i = 0; i < 7; i++)
            {
                _columns[i] = columns[i];
            }

        for (int i = 0; i < columns.Length; i++)
        {

            //columns[i] = new Rect(new Vector2(i * position.width / 7, workspace.y), new Vector2(position.width / 7, workspace.height));
        }


        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();



        for (int i = 0; i < _columns.Length; i++)
        {
            if (i % 2 == 1) continue;

            if (i == 0)
            {



                GUILayout.BeginArea(_columns[i]);

                if (!_scrollPositions.ContainsKey("VisualElements")) _scrollPositions["VisualElements"] = new Vector2(0, 0);
                _scrollPositions["VisualElements"] = EditorGUILayout.BeginScrollView(_scrollPositions["VisualElements"]);


                Rect scrollView = _columns[0];
                scrollView.position -= _scrollPositions["VisualElements"];


                for (int m = 0; m < GetStyleController().GetVisualElements().Count; m++)
                {
                    // Rect scrollRect = columns[i];
                    // scrollRect.position -= _scrollPositions["VisualElements"];
                    DrawStyleModule(GetStyleController().GetVisualElements()[m], scrollView, false);
                }



                EditorGUILayout.EndScrollView();
                GUILayout.EndArea();

            }


            if (i == 2)
            {

                GUILayout.BeginArea(_columns[i]);
                if (!_scrollPositions.ContainsKey("Layers")) _scrollPositions["Layers"] = new Vector2(0, 0);
                _scrollPositions["Layers"] = GUILayout.BeginScrollView(_scrollPositions["Layers"]);


                for (int m = 0; m < GetStyleController().GetLayers().Count; m++)
                {
                    Rect scrollRect = _columns[i];
                    scrollRect.position -= _scrollPositions["Layers"];
                    DrawStyleModule(GetStyleController().GetLayers()[m], scrollRect, false);
                }

                

                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Create layer: ");
                string [] l = GetStyleController().GetLayerTypes();
                int selected = GetStyleController().GetLayerTypeToCreate();
                GetStyleController().SetLayerTypeToCreate(EditorGUILayout.Popup(selected,l));

                if(GUILayout.Button("+",EditorStyles.miniButton, GUILayout.MaxWidth(20)))
                    GetStyleController().CreateLayer();

                EditorGUILayout.EndHorizontal();
                
                GUILayout.EndScrollView();
                GUILayout.EndArea();

            }


            if (i == 4)
            {

                GUILayout.BeginArea(_columns[i]);
                if (!_scrollPositions.ContainsKey("Variables")) _scrollPositions["Variables"] = new Vector2(0, 0);
                _scrollPositions["Variables"] = GUILayout.BeginScrollView(_scrollPositions["Variables"]);


                for (int m = 0; m < GetStyleController().GetVariables().Count; m++)
                {
                    Rect scrollRect = _columns[i];
                    scrollRect.position -= _scrollPositions["Variables"];
                    DrawStyleModule(GetStyleController().GetVariables()[m], scrollRect, false);
                }

                for (int m = 0; m < GetStyleController().GetUserVariables().Count; m++)
                {
                    Rect scrollRect = _columns[i];
                    scrollRect.position -= _scrollPositions["Variables"];
                    DrawStyleModule(GetStyleController().GetUserVariables()[m], scrollRect, false,false, true);
                }

                for(int m = 0; m < GetStyleController().GetDatasets().Count;m++) {
                    Rect scrollRect = _columns[i];
                    scrollRect.position -= _scrollPositions["Variables"];
                    DrawStyleModule(GetStyleController().GetDatasets()[m], scrollRect, false,false, true);
         
                }
                GUILayout.EndScrollView();
                GUILayout.EndArea();

            }



            if (i == 6)
            {

                GUILayout.BeginArea(_columns[i]);
                if (!_scrollPositions.ContainsKey("CustomVariables")) _scrollPositions["CustomVariables"] = new Vector2(0, 0);
                _scrollPositions["CustomVariables"] = GUILayout.BeginScrollView(_scrollPositions["CustomVariables"]);


                for (int m = 0; m < GetStyleController().GetUserVariables().Count; m++)
                {
                    Rect scrollRect = _columns[i];
                    scrollRect.position -= _scrollPositions["CustomVariables"];
                    DrawStyleModule(GetStyleController().GetUserVariables()[m], scrollRect,false,true,false);
                }



                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Create CustomVariable: ");
                string[] l = GetStyleController().GetCustomVariableTypes();
                int selected = GetStyleController().GetCustomVariableTypeToCreate();
                GetStyleController().SetCustomVariableTypeToCreate(EditorGUILayout.Popup(selected, l));

                if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
                    GetStyleController().CreateCustomVariable();

                EditorGUILayout.EndHorizontal();

                GUILayout.EndScrollView();
                GUILayout.EndArea();

            }
        }

        //GUILayout.EndArea();


        Event evt = Event.current;
        bool isDragging = false;
        Vector2 mousePos = Vector2.zero;

        switch (evt.type)
        {
            case EventType.MouseMove:
                activeSource = null;
                break;

            case EventType.MouseDown:


                foreach (string socket in _socketHooks.Keys)
                {
                    if (_socketHooks[socket].Contains(evt.mousePosition) && _sockets[socket].IsOutput())
                    {
                        activeSource = _sockets[socket];
                        Debug.Log(activeSource.GetLabel() + " clicked");
                        break;
                    }


                    if (_socketHooks[socket].Contains(evt.mousePosition) && _sockets[socket].IsInput())
                    {
                        GetStyleController().ClearSocket(_sockets[socket]);
                        if(activeSource != null) Debug.Log(activeSource.GetLabel() + " cleared");

                        break;
                    }
                }

                break;
            case EventType.MouseDrag:
                if (activeSource != null)
                {

                    isDragging = true;
                    mousePos = evt.mousePosition;
                }
                break;
            case EventType.MouseUp:
                if (activeSource != null)
                {
                    foreach (string socket in _socketHooks.Keys)
                    {
                        if (_socketHooks[socket].Contains(evt.mousePosition))
                        {
                            StyleSocket receiving = _sockets[socket];
                            Debug.Log(receiving.GetLabel() + " receiving");

                            if (receiving.DoesAccept(activeSource))
                            {
                                StyleLink link = new StyleLink();
                                link.SetSource(activeSource);
                                link.SetDestination(_sockets[socket]);
                                GetStyleController().AddLink(link);
                            }
                            activeSource = null;
                            break;
                        }
                    }

                }
                    activeSource = null;

                break;

        }



        Repaint();


        for (int i = 0; i < GetStyleController().GetLinks().Count; i++)
        {
            StyleLink link = GetStyleController().GetLinks()[i];
            if (_socketHooks.ContainsKey(link.GetSource().GetUniqueIdentifier()) && _socketHooks.ContainsKey(link.GetDestination().GetUniqueIdentifier()))
                DrawWire(_socketHooks[link.GetSource().GetUniqueIdentifier()], _socketHooks[link.GetDestination().GetUniqueIdentifier()]);

        }

        if (activeSource != null)
        {
            if (_socketHooks.ContainsKey(activeSource.GetUniqueIdentifier()))
                DrawWire(_socketHooks[activeSource.GetUniqueIdentifier()], evt.mousePosition);

        }
    }
}