using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StyleSimpleGlyphLayer : StyleLayer
    {


		public override JSONObject serialize() {
			JSONObject json = new JSONObject();
			return json;
		}

		public override string GetTypeTag() {
			return "GLYPH_LAYER";
		}

        float map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s-a1)*(b2-b1)/(a2-a1);
        }

        [SerializeField]
        public VariableSocket _anchorVariable;

        // [SerializeField]
        // public VariableSocket _colorVariable;
        // public StyleTypeSocket<MinMax<float>> _colordataRangeInput;


        [SerializeField]
        public StyleColorModifier _colorModifier;

        [SerializeField]
        public StyleOpacityModifier _opacityModifier;


        [SerializeField]
        public StyleScaleModifier _scaleModifer;

        [SerializeField]
        public VariableSocket _directionVariable;

        // [SerializeField]
        // public VariableSocket _opacityVariable;
        // public StyleTypeSocket<MinMax<float>> _opacitydataRangeInput;

        // [SerializeField]
        // public StyleTypeSocket<Range<float>> _opacityThresholdInput;

        // [SerializeField]
        // public StyleTypeSocket<Colormap> _colorMapInput;

        [SerializeField]
        public StyleTypeSocket<Glyph> _glyphInput;

        [SerializeField]
        public StyleTypeSocket<Range<int>> _lodLevel;

        [SerializeField]
        public StyleTypeSocket<Range<int>> _maxGlyphs;


        [SerializeField]
        public StyleTypeSocket<Range<float>> _percentGlyphs;

        [SerializeField]
        public StyleTypeSocket<Range<float>> _glyphScaleInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _useMeshInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _useThumbnailInput;
        [SerializeField]
        public StyleTypeSocket<Range<bool>> _faceCameraInput;

        // [SerializeField]
        // public StyleTypeSocket<Range<float>> _opacityMultiplierInput;

        // [SerializeField]
        // public StyleTypeSocket<Objectify<Color>> _colorInput;

        // [SerializeField]
        // public StyleTypeSocket<Range<bool>> _useColormapInput;

        // [SerializeField]
        // public StyleTypeSocket<Range<bool>> _flipColormapInput;


        [SerializeField]
        public Material _pointMaterial;

        [SerializeField]
        public Mesh _billboardMesh;


        [SerializeField]
        public StyleTypeSocket<Range<bool>> _usePlane1;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _usePlane2;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _usePlane3;


        [SerializeField]
        bool _sampleAtCenter = true;
        
   
	[SerializeField]
	public int instanceCount = 50000;


        private ComputeBuffer argsBuffer;
        private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

        public override bool HasBounds()
        {
            return _anchorVariable != null && _anchorVariable.IsAssigned();
        }
        public override Bounds GetBounds()
        {
            return ((Variable)_anchorVariable.GetInput()).GetBounds();
        }


        public override void DrawLayer(Canvas canvas)
        {


            if (_anchorVariable == null || !_anchorVariable.IsAssigned()) return;
            Datastream stream = ((Variable)_anchorVariable.GetInput()).GetStream(null, 0, 0);
            float[] a = stream.GetArray();


            Variable v = null;
        
            //if(_colorModifier._variable.GetInput() != null) 
                v = ((Variable)_colorModifier._variable.GetInput());
                if(v != null) {
                    MinMax<float> selectedRange = ((MinMax<float>)_colorModifier._variableRange.GetInput());

                    float leftVal = map(selectedRange.lowerValue,0,1,v.GetMin().x,v.GetMax().x);

                    float rightVal = map(selectedRange.upperValue,0,1,v.GetMin().x,v.GetMax().x);

                    _colorModifier._variable.LowerBound = leftVal;
                    _colorModifier._variable.UpperBound = rightVal;
                }
        


                v = ((Variable)_opacityModifier._variable.GetInput());
                if(v != null) {
                    MinMax<float> selectedRange = ((MinMax<float>)_opacityModifier._variableRange.GetInput());

                    float leftVal = map(selectedRange.lowerValue,0,1,v.GetMin().x,v.GetMax().x);

                    float rightVal = map(selectedRange.upperValue,0,1,v.GetMin().x,v.GetMax().x);

                    _opacityModifier._variable.LowerBound = leftVal;
                    _opacityModifier._variable.UpperBound = rightVal;
                }
        

    

                v = ((Variable)_scaleModifer._variable.GetInput());
                if(v != null) {
                    MinMax<float> selectedRange = ((MinMax<float>)_scaleModifer._variableRange.GetInput());

                    float leftVal = map(selectedRange.lowerValue,0,1,v.GetMin().x,v.GetMax().x);

                    float rightVal = map(selectedRange.upperValue,0,1,v.GetMin().x,v.GetMax().x);

                    _scaleModifer._variable.LowerBound = leftVal;
                    _scaleModifer._variable.UpperBound = rightVal;
                }
        

            // _opacityVariable.LowerBound = ((MinMax<float>)_opacitydataRangeInput.GetInput()).lowerValue;
            // _opacityVariable.UpperBound = ((MinMax<float>)_opacitydataRangeInput.GetInput()).upperValue;


            //if (_colorMapInput.GetInput() != null)
            _pointMaterial.SetTexture("_ColorMap", ((Colormap)_colorModifier._colormapSocket.GetInput()).GetTexture());
            _pointMaterial.SetTexture("_OpacityMap", ((Colormap)_opacityModifier._opacitymapSocket.GetInput()).GetTexture());
            //_pointMaterial.SetFloat("_ScaleMin", ((MinMax<float>)_opacityModifier._opacitySocket.GetInput()).lowerValue);
            //_pointMaterial.SetFloat("_ScaleMax", ((MinMax<float>)_opacityModifier._opacitySocket.GetInput()).lowerValue);


            _pointMaterial.SetColor("_Color", (Objectify<Color>)_colorModifier._colorSocket.GetInput());
            _pointMaterial.SetFloat("_Opacity", (Range<float>)_opacityModifier._opacitySocket.GetInput());
            //_pointMaterial.SetFloat("_Scale", (Range<float>)_opacityModifier._opacitySocket.GetInput());


            _pointMaterial.SetInt("_useColormap", (Range<bool>)_colorModifier._useVariable.GetInput()?1:0);
            _pointMaterial.SetInt("_useOpacitymap", (Range<bool>)_opacityModifier._useVariable.GetInput()?1:0);
           // _pointMaterial.SetInt("_useScalemap", (Range<bool>)_opacityModifier._useVariable.GetInput()?1:0);

            _pointMaterial.SetInt("_flipColormap", (Range<bool>)_colorModifier._flipColormapSocket.GetInput()?1:0);
            _pointMaterial.SetInt("_flipOpacitymap", (Range<bool>)_opacityModifier._flipOpacityMapSocket.GetInput()?1:0);
           // _pointMaterial.SetInt("_flipScalemap", (Range<bool>)_opacityModifier._flipOpacityMapSocket.GetInput()?1:0);

            _pointMaterial.SetFloat("_glyphPercent", (Range<float>)_percentGlyphs.GetInput());

            _pointMaterial.SetFloat("_glyphScale", (Range<float>)_glyphScaleInput.GetInput());
            _pointMaterial.SetInt("_useMesh",1);
            _pointMaterial.SetInt("_useThumbnail", 0);
            // _pointMaterial.SetColor("_Color", (Objectify<Color>)_colorModifier._colorSocket.GetInput());
            // _pointMaterial.SetFloat("_OpacityMultiplier", (Range<float>)_opacityMultiplierInput.GetInput());
            _pointMaterial.SetInt("_faceCamera", 0);
            // _pointMaterial.SetFloat("_opacityThreshold", (Range<float>)_opacityThresholdInput.GetInput());

            _pointMaterial.SetInt("_usePlane1", (Range<bool>)_usePlane1.GetInput()?1:0);
            _pointMaterial.SetInt("_usePlane2", (Range<bool>)_usePlane2.GetInput()?1:0);
            _pointMaterial.SetInt("_usePlane3", (Range<bool>)_usePlane3.GetInput()?1:0);


            _pointMaterial.SetVector("_plane1min", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMins()[0]);
            _pointMaterial.SetVector("_plane2min", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMins()[1]);
            _pointMaterial.SetVector("_plane3min", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMins()[2]);

            _pointMaterial.SetVector("_plane1max", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMaxes()[0]);
            _pointMaterial.SetVector("_plane2max", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMaxes()[1]);
            _pointMaterial.SetVector("_plane3max", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMaxes()[2]);

            {


                if (_glyphInput.GetInput() != null)
                    {
                         

                        if(argsBuffer == null) argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);

                        Mesh instanceMesh;
                        if(true) {
                            instanceMesh = ((Glyph)(_glyphInput.GetInput())).GetLODMesh(((Range<int>)_lodLevel.GetInput()));
                            if(((Glyph)(_glyphInput.GetInput())).HasNormals()) {
                                _pointMaterial.SetTexture("_BumpMap",((Glyph)(_glyphInput.GetInput())).GetLODNormalMap(((Range<int>)_lodLevel.GetInput())));
                                _pointMaterial.SetInt("_hasBumpMap", 1);
                            } else {
                                _pointMaterial.SetInt("_hasBumpMap", 0);

                            }
               

                        }
                        else {
                            instanceMesh = _billboardMesh;
                            _pointMaterial.SetTexture("_BumpMap", Glyph.DefaultNormalMap());
                            if(((Range<bool>)_useThumbnailInput.GetInput()) == true) {
                                _pointMaterial.SetTexture("_MainTex", ((Glyph)(_glyphInput.GetInput())).GetPreviewImage());
                            }
                            _pointMaterial.SetTexture("_AlphaTex", ((Glyph)(_glyphInput.GetInput())).GetAlphaMap());

                        }

                        args[0] = (uint)instanceMesh.GetIndexCount(0);
                        args[1] = (uint)Mathf.Min((uint)stream.GetNumberOfElements(), Mathf.Floor((int)(Range<int>)_maxGlyphs.GetInput() * (float)(Range<float>)_percentGlyphs.GetInput()));
                        args[2] = (uint)instanceMesh.GetIndexStart(0);
                        args[3] = (uint)instanceMesh.GetBaseVertex(0);
                        argsBuffer.SetData(args);


                        Material canvasMaterial = GetCanvasMaterial(canvas, _pointMaterial);
                        _anchorVariable.Bind(_pointMaterial, 0, 0);
                        _colorModifier._variable.Bind(_pointMaterial, 0, 0,_colorModifier.lowerBound(),_colorModifier.upperBound());
                        _opacityModifier._variable.Bind(_pointMaterial, 0, 0,_opacityModifier.lowerBound(),_opacityModifier.upperBound());
                        _directionVariable.Bind(_pointMaterial, 0, 0);

                        //Graphics.DrawMesh(instanceMesh, canvas.GetInnerSceneTransformMatrix(), canvasMaterial, 0);
                        Graphics.DrawMeshInstancedIndirect(instanceMesh, 0, canvasMaterial, new Bounds(new Vector3(0, 0, 0), new Vector3(300, 300, 300)),argsBuffer,0,null,UnityEngine.Rendering.ShadowCastingMode.Off,false);

                }

            }

        }


        public override StyleLayer CopyLayer(StyleLayer toCopy = null)
        {
              if(toCopy == null)
                toCopy = new StyleSimpleGlyphLayer();
                
            if (toCopy != null && toCopy is StyleSimpleGlyphLayer)
            {
                _pointMaterial = new Material(((StyleSimpleGlyphLayer)toCopy)._pointMaterial);
                instanceCount = ((StyleSimpleGlyphLayer)toCopy).instanceCount;
                _billboardMesh = ((StyleSimpleGlyphLayer)toCopy)._billboardMesh;

            }

            return Init();
        }

        public StyleSimpleGlyphLayer Init()
        {
            _glyphInput = (new StyleTypeSocket<Glyph> ()).Init("Glyph",this);
            _glyphInput.SetDefaultInputObject(Glyph.DefaultGlyph());
			AddSubmodule(_glyphInput);

            _maxGlyphs = (new StyleTypeSocket<Range<int>>()).Init("Max glyphs", this);
            _maxGlyphs.SetDefaultInputObject((new Range<int>(0, 60000, 10000)));
            AddSubmodule(_maxGlyphs);

            _percentGlyphs = (new StyleTypeSocket<Range<float>>()).Init("Percent of glyphs", this);
            _percentGlyphs.SetDefaultInputObject((new Range<float>(0, 1, 1)));
            AddSubmodule(_percentGlyphs);

            _anchorVariable = new VariableSocket();
            _anchorVariable.Init("Anchor",this,0);
            AddSubmodule(_anchorVariable);



            _directionVariable = new VariableSocket();
            _directionVariable.Init("Direction",this,2);
	        _directionVariable.SetAnchorVariableSocket(_anchorVariable);
			_directionVariable.RequireVector();
            AddSubmodule(_directionVariable);


            _colorModifier = CreateInstance<StyleColorModifier>();
            _colorModifier.Init(_anchorVariable,this,1);
            AddSubmodule(_colorModifier);


            _opacityModifier = CreateInstance<StyleOpacityModifier>();
            _opacityModifier.Init(_anchorVariable,this,3);
            AddSubmodule(_opacityModifier);

            _scaleModifer = CreateInstance<StyleScaleModifier>();
            _scaleModifer.Init(_anchorVariable,this,1);
            AddSubmodule(_scaleModifer);

            // _opacityVariable = new VariableSocket();
            // _opacityVariable.Init("Opacity",this,3);
	        // _opacityVariable.SetAnchorVariableSocket(_anchorVariable);
			// _opacityVariable.RequireScalar();

            // _colordataRangeInput = (new StyleTypeSocket<MinMax<float>> ()).Init("Color Data Range",this);
            // _colordataRangeInput.SetDefaultInputObject((new MinMax<float>(0, 1)));

            // _opacitydataRangeInput = (new StyleTypeSocket<MinMax<float>> ()).Init("OpacityData Range",this);
            // _opacitydataRangeInput.SetDefaultInputObject((new MinMax<float>(0, 1)));

            // _opacityThresholdInput = (new StyleTypeSocket<Range<float>>()).Init("Opacity threshold", this);
            // _opacityThresholdInput.SetDefaultInputObject((new Range<float>(0, 1, 0.5f)));
            // AddSubmodule(_opacityThresholdInput);



            // AddSubmodule(_colorVariable);
            // AddSubmodule(_colordataRangeInput);
            // AddSubmodule(_opacityVariable);
            // AddSubmodule(_opacitydataRangeInput);




            _lodLevel = (new StyleTypeSocket<Range<int>>()).Init("Glyph LOD", this);
            _lodLevel.SetDefaultInputObject((new Range<int>(0, 2,2)));
            AddSubmodule(_lodLevel);


            _glyphScaleInput = (new StyleTypeSocket<Range<float>>()).Init("Glyph scale", this);
            _glyphScaleInput.SetDefaultInputObject((new Range<float>(0, 0.1f,0.05f)));
            AddSubmodule(_glyphScaleInput);


            // _useMeshInput = (new StyleTypeSocket<Range<bool>>()).Init("Use mesh", this);
            // _useMeshInput.SetDefaultInputObject((new Range<bool>(false, true,true)));
            // AddSubmodule(_useMeshInput);

            // _useThumbnailInput = (new StyleTypeSocket<Range<bool>>()).Init("Use thumbnail", this);
            // _useThumbnailInput.SetDefaultInputObject((new Range<bool>(false, true,false)));
            // AddSubmodule(_useThumbnailInput);

            // _faceCameraInput = (new StyleTypeSocket<Range<bool>>()).Init("FaceCamera", this);
            // _faceCameraInput.SetDefaultInputObject((new Range<bool>(false, true,false)));
            // AddSubmodule(_faceCameraInput);

            // _opacityMultiplierInput = (new StyleTypeSocket<Range<float>>()).Init("Opacity mult", this);
            // _opacityMultiplierInput.SetDefaultInputObject((new Range<float>(0,10,1)));
            // AddSubmodule(_opacityMultiplierInput);

            // _colorInput = (new StyleTypeSocket<Objectify<Color>>()).Init("GlyphColor", this);
            // _colorInput.SetDefaultInputObject(new Objectify< Color>(Color.white));
            // AddSubmodule(_colorInput);


            // _useColormapInput = (new StyleTypeSocket<Range<bool>>()).Init("UseColormap", this);
            // _useColormapInput.SetDefaultInputObject(new Range<bool>(false,true,false));
            // AddSubmodule(_useColormapInput);

            // _flipColormapInput = (new StyleTypeSocket<Range<bool>>()).Init("FlipColormap", this);
            // _flipColormapInput.SetDefaultInputObject(new Range<bool>(false,true,false));
            // AddSubmodule(_flipColormapInput);



            _usePlane1 = (new StyleTypeSocket<Range<bool>>()).Init("Use Plane 1", this);
            _usePlane1.SetDefaultInputObject(new Range<bool>(false,true,false));
            AddSubmodule(_usePlane1);

            _usePlane2 = (new StyleTypeSocket<Range<bool>>()).Init("Use Plane 2", this);
            _usePlane2.SetDefaultInputObject(new Range<bool>(false,true,false));
            AddSubmodule(_usePlane2);


            _usePlane3 = (new StyleTypeSocket<Range<bool>>()).Init("Use Plane 3", this);
            _usePlane3.SetDefaultInputObject(new Range<bool>(false,true,false));
            AddSubmodule(_usePlane3);

            return this;

        }

		public override void UpdateModule(string updatedSocket = null) {

            //  if(_colorVariable.GetInput() != null) {

            //     Variable v = ((Variable)_colorVariable.GetInput());

            //     ((MinMax<float>)_colordataRangeInput.GetInput()).lowerBound = v.GetMin().x;
            //     ((MinMax<float>)_colordataRangeInput.GetInput()).upperBound = v.GetMax().x;
            // }

            // if(_opacityVariable.GetInput() != null) {

            //     Variable v = ((Variable)_opacityVariable.GetInput());

            //     ((MinMax<float>)_opacitydataRangeInput.GetInput()).lowerBound = v.GetMin().x;
            //     ((MinMax<float>)_opacitydataRangeInput.GetInput()).upperBound = v.GetMax().x;
            // }

            base.UpdateModule();

		}

        public override string GetLabel()
        {
            //  if(_anchorVariable.GetInput() != null && ((Variable)_anchorVariable.GetInput())!=null)
            // Debug.Log("Custom Output:" + ((Variable)_anchorVariable.GetInput()).GetStream(null,0,0).GetNumberOfElements());
         
            return "Simple Glyph Layer";
        }

    }
}

