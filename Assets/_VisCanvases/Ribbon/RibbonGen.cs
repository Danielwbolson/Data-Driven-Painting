using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis {
    [CreateAssetMenu()]

    public class RibbonGen : StyleLayer {

        // VARIABLE SOCKETS (streamline data generated with VTK)

        // positions for the centerline of the ribbon
        Vector3[][] dataAnchors;
        public VariableSocket _dataAnchors;

        // tanget along the 3D curve
        Vector3[][] dataTangents;
        public VariableSocket _dataTangents;

        // normal of the 3D curve, this is used as the normal for the ribbon surface.
        // the ribbon extends sideways in the "binormal" direction, where
        // binormal = tangent (cross) normal
        Vector3[][] dataNormals;
        public VariableSocket _dataNormals;

        // arc lengths are used to calculate texture coordinates
        float[][] dataArcLengths;

        // Number of ribbons to draw
        public StyleTypeSocket<Range<int>> _maxPaths;
        public int LineCount = 1000; // Not sure what this is useful for

        public StyleTypeSocket<Glyph> _glyphInput;


        // TYPE SOCKETS (textures and other input from the visual mapper)

        // the ribbon geometry will have a width of twice this value in world coordinates 
        public float halfRibbonWidth = 0.2f;
        // this texture will be stretch across the entire width of the ribbon.  if the
        // ribbon is long, the texture will repeat.  the arc length to traverse before
        // repeating is set based on the ribbon width so that the texture maintains its
        // original aspect ratio as it is stamped onto the ribbon.
        public Texture2D mainTexture;
        // used for normal mapping, uses the same tex coords as for mainTexture.
        public Texture2D normalTexture;

        public Material _ribbonMaterial;

        public Mesh[] _ribbonMesh;

        private Vector3[][] _meshVerts;
        private Vector3[][] _meshNorms;


        public override bool HasBounds() {
            return _dataAnchors != null && _dataAnchors.IsAssigned();
        }
        public override Bounds GetBounds() {
            return ((Variable)_dataAnchors.GetInput()).GetBounds();
        }
        public override string GetLabel() {
            return "RibbonGen Layer";
        }

        public override void DrawLayer(Canvas canvas) {
            if (_ribbonMaterial == null) return;
            if (_ribbonMesh == null) return;

            if (_glyphInput.GetInput() != null) {
                _ribbonMaterial.SetTexture("_MainTex", ((Glyph)(_glyphInput.GetInput())).GetPreviewImage());
            }

            Material canvasMaterial = GetCanvasMaterial(canvas, _ribbonMaterial);

            for (int i = 0; i < _ribbonMesh.Length; i++) {
                Graphics.DrawMesh(_ribbonMesh[i], canvas.GetInnerSceneTransformMatrix(), canvasMaterial, 0);
            }

            //for (int i = 0; i < _ribbonMesh.Length; i++) {
            //    for (int j = 0; j < _meshVerts[i].Length; j++) {
            //        Debug.DrawLine(_meshVerts[i][j], _meshVerts[i][j] + _meshNorms[i][j]);
            //    }
            //}
        }

        public override StyleLayer CopyLayer(StyleLayer toCopy = null) {
            if (toCopy != null && toCopy is RibbonGen) {
                if (((RibbonGen)toCopy)._ribbonMaterial != null)
                    _ribbonMaterial = new Material(((RibbonGen)toCopy)._ribbonMaterial);
                LineCount = ((RibbonGen)toCopy).LineCount;
                mainTexture = ((RibbonGen)toCopy).mainTexture;
                normalTexture = ((RibbonGen)toCopy).normalTexture;
            }

            return Init();
        }

        public RibbonGen Init() {
            _dataAnchors = new VariableSocket();
            _dataAnchors.Init("Anchor", this, 0);

            _dataTangents = new VariableSocket();
            _dataTangents.Init("Tangents", this);
            _dataTangents.SetAnchorVariableSocket(_dataAnchors);
            _dataTangents.RequireVector();

            _dataNormals = new VariableSocket();
            _dataNormals.Init("Normals", this);
            _dataNormals.SetAnchorVariableSocket(_dataAnchors);
            _dataNormals.RequireVector();

            _glyphInput = (new StyleTypeSocket<Glyph>()).Init("Inkwash", this);

            AddSubmodule(_dataAnchors);
            AddSubmodule(_dataTangents);
            AddSubmodule(_dataNormals);
            AddSubmodule(_glyphInput);

            _maxPaths = (new StyleTypeSocket<Range<int>>()).Init("Max paths", this);
            _maxPaths.SetDefaultInputObject(new Range<int>(1, 50000, 1000));
            AddSubmodule(_maxPaths);

            return this;
        }

        public override void UpdateModule() {

            if (_dataAnchors.GetInput() == null) return;

            GenerateRibbon();

        }

        void GenerateRibbon() {

            //InitDummyData(100);
            InitRealData();

            float aspectRatio = (float)mainTexture.height / (float)mainTexture.width;
            float texRepeatLength = aspectRatio * halfRibbonWidth * 2.0f;

            _ribbonMesh = new Mesh[dataAnchors.Length];
            _meshNorms = new Vector3[dataAnchors.Length][];
            _meshVerts = new Vector3[dataAnchors.Length][];

            for (int i = 0; i < dataAnchors.Length; i++) {

                // Create a ribbon based on the data
                List<Vector3> verts = new List<Vector3>();
                List<Vector2> texCoords = new List<Vector2>();
                List<Vector3> normsFront = new List<Vector3>();
                List<Vector3> normsBack = new List<Vector3>();
                List<int> indicesFront = new List<int>();
                List<int> indicesBack = new List<int>();

                _meshNorms[i] = new Vector3[dataAnchors[i].Length];
                _meshVerts[i] = new Vector3[dataAnchors[i].Length];

                Vector3 binormalPrev = new Vector3();

                for (int j = 0; j < dataAnchors[i].Length; j++) {
                    float v = (float)dataArcLengths[i][j] / texRepeatLength;

                    Vector3 binormal = Vector3.Cross(dataTangents[i][j], dataNormals[i][j]);

                    // Fixing odd flip of binormal
                    if (i > 0) {
                        if (Vector3.Dot(binormal, binormalPrev) < 0.0f) {
                            binormal = -binormal;
                        }
                    }

                    verts.Add(dataAnchors[i][j] + halfRibbonWidth * binormal);
                    texCoords.Add(new Vector2(0.0f, v));
                    normsFront.Add(dataNormals[i][j]);
                    normsBack.Add(-dataNormals[i][j]);


                    verts.Add(dataAnchors[i][j] - halfRibbonWidth * binormal);
                    texCoords.Add(new Vector2(1.0f, v));
                    normsFront.Add(dataNormals[i][j]);
                    normsBack.Add(-dataNormals[i][j]);

                    binormalPrev = binormal;

                }

                for (int j = 0; j < dataAnchors[i].Length - 1; j++) {
                    indicesFront.Add(2 * j + 0);
                    indicesFront.Add(2 * j + 2);
                    indicesFront.Add(2 * j + 1);

                    indicesFront.Add(2 * j + 1);
                    indicesFront.Add(2 * j + 2);
                    indicesFront.Add(2 * j + 3);


                    indicesBack.Add(2 * dataAnchors[i].Length + 2 * j + 0);
                    indicesBack.Add(2 * dataAnchors[i].Length + 2 * j + 1);
                    indicesBack.Add(2 * dataAnchors[i].Length + 2 * j + 2);

                    indicesBack.Add(2 * dataAnchors[i].Length + 2 * j + 1);
                    indicesBack.Add(2 * dataAnchors[i].Length + 2 * j + 3);
                    indicesBack.Add(2 * dataAnchors[i].Length + 2 * j + 2);
                }

                // combine front and back into larger a single mesh:
                // use the same verts and tex coords for both front and back
                verts.AddRange(verts);
                texCoords.AddRange(texCoords);
                // but use different normals and also indices for opposite vertex winding
                List<Vector3> norms = normsFront;
                norms.AddRange(normsBack);
                List<int> indices = indicesFront;
                indices.AddRange(indicesBack);

                Mesh mesh = new Mesh();
                mesh.name = "StreamlineRibbonMesh";
                mesh.Clear();
                mesh.SetVertices(verts);
                mesh.SetNormals(norms);
                mesh.SetUVs(0, texCoords);
                mesh.SetTriangles(indices, 0);

                // Original normals occasionally flip. Could be detected and fixed?
                // For now, works
                mesh.RecalculateNormals();

                _ribbonMesh[i] = mesh;
                _meshNorms[i] = norms.ToArray();
                _meshVerts[i] = verts.ToArray();
            }

            _ribbonMaterial = new Material(Shader.Find("Standard"));
            _ribbonMaterial.mainTexture = mainTexture;
            _ribbonMaterial.SetTexture("_BumpMap", normalTexture);
            _ribbonMaterial.shaderKeywords = new string[1] { "_NORMALMAP" };
            ChangeRenderMode(_ribbonMaterial, BlendMode.Cutout);

            //GameObject go = new GameObject("StreamlineRibbon");
            //MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            //meshFilter.sharedMesh = mesh;
            //MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            //meshRenderer.material = new Material(Shader.Find("Standard"));
            //meshRenderer.material.mainTexture = mainTexture;
            //meshRenderer.material.SetTexture("_BumpMap", normalTexture);
            //meshRenderer.material.shaderKeywords = new string[1] { "_NORMALMAP" };
            //ChangeRenderMode(meshRenderer.material, BlendMode.Cutout);
        }

        void InitRealData() {
            Datastream stream = ((Variable)_dataAnchors.GetInput()).GetStream(null, 0, 0);

            Mesh[] m = stream.GetMeshes();

            Debug.Log("Number of paths:" + m.Length);

            float[] points = ((Variable)_dataAnchors.GetInput()).GetStream(null, 0, 0).GetArray();
            VTK.vtkDataSet pd = ((Variable)_dataAnchors.GetInput()).GetStream(null, 0, 0).GetVTKDataset();

            VTK.vtkIdList idlist = VTK.vtkIdList.New();
            VTK.vtkDataArray normalArray = pd.GetPointData().GetArray("Normals");

            dataAnchors = new Vector3[pd.GetNumberOfCells()][];
            dataArcLengths = new float[dataAnchors.Length][];
            dataTangents = new Vector3[dataAnchors.Length][];
            dataNormals = new Vector3[dataAnchors.Length][];

            for (long c = 0; c < pd.GetNumberOfCells(); c++) {

                pd.GetCellPoints(c, idlist);
                int idCount = (int)idlist.GetNumberOfIds();


                int pointsInPath = idCount;
                // Set up data lists

                dataAnchors[c] = new Vector3[pointsInPath];

                dataArcLengths[c] = new float[dataAnchors[c].Length];
                dataArcLengths[c][0] = 0.0f;

                dataTangents[c] = new Vector3[dataAnchors[c].Length];
                dataNormals[c] = new Vector3[dataAnchors[c].Length];

                Vector3 normalPrev = new Vector3();

                Vector3 binormalGuess = new Vector3();
                Vector3 prevBinormalGuess = new Vector3();

                for (int i = 0; i < idCount; i++) {

                    int id = (int)idlist.GetId(i);
                    Vector3 point = pd.GetPoint(id);

                    // Set Anchor points
                    dataAnchors[c][i] = point;

                    // Calculate arc lengths, starting from second index
                    if (i > 0)
                        dataArcLengths[c][i] = dataArcLengths[c][i - 1] + (dataAnchors[c][i] - dataAnchors[c][i - 1]).magnitude;

                    // Calculate tangents from anchors
                    Vector3 tan = new Vector3();
                    if (i > 0) {
                        tan += dataAnchors[c][i] - dataAnchors[c][i - 1];
                    }
                    if (i < dataAnchors[c].Length - 1) {
                        tan += dataAnchors[c][i + 1] - dataAnchors[c][i];
                    }
                    tan.Normalize();
                    dataTangents[c][i] = tan;

                    // Set normal
                    Vector3 normal = new Vector3();

                    if (_dataNormals.GetInput() != null) {
                        double[] d = new double[3];
                        normalArray.GetTuple(id, d);
                        normal = new Vector3((float)d[0], (float)d[1], (float)d[2]);
                    } else {
                        if (i == 0) {
                            // start with an initial random guess
                            binormalGuess = Vector3.Normalize(
                                new Vector3(
                                    Random.Range(0.0f, 1.0f),
                                    Random.Range(0.0f, 1.0f),
                                    Random.Range(0.0f, 1.0f)));
                        } else {
                            binormalGuess = prevBinormalGuess;
                        }
                        // "fix it" by making sure it is perpendicular to the tangent
                        Vector3 normalGuess = Vector3.Normalize(Vector3.Cross(binormalGuess, dataTangents[c][i]));
                        Vector3 binormal = Vector3.Normalize(Vector3.Cross(dataTangents[c][i], normalGuess));
                        normal = Vector3.Normalize(Vector3.Cross(binormal, dataTangents[c][i]));
                        prevBinormalGuess = binormal;
                    }

                    dataNormals[c][i] = normal;
                    normalPrev = normal;
                }
            }
        }


        public enum BlendMode {
            Opaque,
            Cutout,
            Fade,
            Transparent
        }

        public static void ChangeRenderMode(Material standardShaderMaterial, BlendMode blendMode) {
            switch (blendMode) {
                case BlendMode.Opaque:
                    standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    standardShaderMaterial.SetInt("_ZWrite", 1);
                    standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    standardShaderMaterial.renderQueue = -1;
                    break;
                case BlendMode.Cutout:
                    standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    standardShaderMaterial.SetInt("_ZWrite", 1);
                    standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    standardShaderMaterial.renderQueue = 2450;
                    break;
                case BlendMode.Fade:
                    standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    standardShaderMaterial.SetInt("_ZWrite", 0);
                    standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                    standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    standardShaderMaterial.renderQueue = 3000;
                    break;
                case BlendMode.Transparent:
                    standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    standardShaderMaterial.SetInt("_ZWrite", 0);
                    standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                    standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    standardShaderMaterial.renderQueue = 3000;
                    break;
            }
        }

        // FOR TESTING ONLY...
        /*
         * void InitDummyData(int n) {

            // Create simple curve in the XY plane
            dataAnchors = new Vector3[n];
            for (int i = 0; i < n; i++) {
                float t = (float)i / (float)n;
                dataAnchors[i] = new Vector3(-15.0f + 30.0f * t, Mathf.Sin(Mathf.PI * 2.0f * t), 0.0f);
            }

            // Calculate arc lengths
            dataArcLengths = new float[dataAnchors.Length];
            dataArcLengths[0] = 0.0f;
            for (int i = 1; i < dataAnchors.Length; i++) {
                dataArcLengths[i] = dataArcLengths[i - 1] + (dataAnchors[i] - dataAnchors[i - 1]).magnitude;
            }

            // Calculate tangents
            dataTangents = new Vector3[dataAnchors.Length];
            for (int i = 0; i < dataAnchors.Length; i++) {
                Vector3 tan = new Vector3();
                if (i > 0) {
                    tan += dataAnchors[i] - dataAnchors[i - 1];
                }
                if (i < dataAnchors.Length - 1) {
                    tan += dataAnchors[i + 1] - dataAnchors[i];
                }
                tan.Normalize();
                dataTangents[i] = tan;
            }

            // Given a binormal of +Z, calculate the normals
            dataNormals = new Vector3[dataAnchors.Length];
            Vector3 binormal = new Vector3(0, 0, 1);
            for (int i = 0; i < dataAnchors.Length; i++) {
                dataNormals[i] = Vector3.Normalize(Vector3.Cross(binormal, dataTangents[i]));
            }
        }
        */
    }
}