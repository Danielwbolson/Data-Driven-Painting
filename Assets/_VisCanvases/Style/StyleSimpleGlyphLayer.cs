using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StyleSimpleGlyphLayer : StyleLayer
    {



        [SerializeField]
        public VariableSocket _anchorVariable;

        [SerializeField]
        public VariableSocket _colorVariable;
        public StyleTypeSocket<MinMax<float>> _colordataRangeInput;

        [SerializeField]
        public VariableSocket _directionVariable;

        [SerializeField]
        public VariableSocket _opacityVariable;
        public StyleTypeSocket<MinMax<float>> _opacitydataRangeInput;

        [SerializeField]
        public StyleTypeSocket<Range<float>> _opacityThresholdInput;

        [SerializeField]
        public StyleTypeSocket<Colormap> _colorMapInput;

        [SerializeField]
        public StyleTypeSocket<Glyph> _glyphInput;

        [SerializeField]
        public StyleTypeSocket<Range<int>> _lodLevel;

        [SerializeField]
        public StyleTypeSocket<Range<int>> _maxGlyphs;

        [SerializeField]
        public StyleTypeSocket<Range<float>> _glyphScaleInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _useMeshInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _useThumbnailInput;
        [SerializeField]
        public StyleTypeSocket<Range<bool>> _faceCameraInput;

        [SerializeField]
        public StyleTypeSocket<Range<float>> _opacityMultiplierInput;

        [SerializeField]
        public StyleTypeSocket<Objectify<Color>> _colorInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _useColormapInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _flipColormapInput;


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

            
            _colorVariable.LowerBound = ((MinMax<float>)_colordataRangeInput.GetInput()).lowerValue;
            _colorVariable.UpperBound = ((MinMax<float>)_colordataRangeInput.GetInput()).upperValue;

            _opacityVariable.LowerBound = ((MinMax<float>)_opacitydataRangeInput.GetInput()).lowerValue;
            _opacityVariable.UpperBound = ((MinMax<float>)_opacitydataRangeInput.GetInput()).upperValue;


            if (_colorMapInput.GetInput() != null)
                _pointMaterial.SetTexture("_ColorMap", ((Colormap)_colorMapInput.GetInput()).GetTexture());
            _pointMaterial.SetFloat("_glyphScale", (Range<float>)_glyphScaleInput.GetInput());
            _pointMaterial.SetInt("_useMesh", (Range<bool>)_useMeshInput.GetInput()?1:0);
            _pointMaterial.SetInt("_useThumbnail", (Range<bool>)_useThumbnailInput.GetInput()?1:0);
            _pointMaterial.SetColor("_Color", (Objectify<Color>)_colorInput.GetInput());
            _pointMaterial.SetFloat("_OpacityMultiplier", (Range<float>)_opacityMultiplierInput.GetInput());
            _pointMaterial.SetInt("_faceCamera", (Range<bool>)_faceCameraInput.GetInput()?1:0);
            _pointMaterial.SetFloat("_opacityThreshold", (Range<float>)_opacityThresholdInput.GetInput());
            _pointMaterial.SetInt("_useColormap", (Range<bool>)_useColormapInput.GetInput()?1:0);
            _pointMaterial.SetInt("_flipColormap", (Range<bool>)_flipColormapInput.GetInput()?1:0);

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
                        if(((Range<bool>)_useMeshInput.GetInput()) == true) {
                            instanceMesh = ((Glyph)(_glyphInput.GetInput())).GetLODMesh(((Range<int>)_lodLevel.GetInput()));
                            _pointMaterial.SetTexture("_BumpMap", ((Glyph)(_glyphInput.GetInput())).GetLODNormalMap(((Range<int>)_lodLevel.GetInput())));

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
                        args[1] = (uint)Mathf.Min((uint)stream.GetNumberOfElements(), (int)(Range<int>)_maxGlyphs.GetInput());
                        args[2] = (uint)instanceMesh.GetIndexStart(0);
                        args[3] = (uint)instanceMesh.GetBaseVertex(0);
                        argsBuffer.SetData(args);


                        Material canvasMaterial = GetCanvasMaterial(canvas, _pointMaterial);
                        _anchorVariable.Bind(_pointMaterial, 0, 0);
                        _colorVariable.Bind(_pointMaterial, 0, 0);
                        _opacityVariable.Bind(_pointMaterial, 0, 0);
                        _directionVariable.Bind(_pointMaterial, 0, 0);

                        //Graphics.DrawMesh(instanceMesh, canvas.GetInnerSceneTransformMatrix(), canvasMaterial, 0);
                        Graphics.DrawMeshInstancedIndirect(instanceMesh, 0, canvasMaterial, new Bounds(new Vector3(0, 0, 0), new Vector3(300, 300, 300)),argsBuffer);

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
            _anchorVariable = new VariableSocket();
            _anchorVariable.Init("Anchor",this,0);
            //SetAnchorSocket(_anchorVariable);
            _colorVariable = new VariableSocket();
            _colorVariable.Init("Color",this,1);
            _colorVariable.SetAnchorVariableSocket(_anchorVariable);
			_colorVariable.RequireScalar();

            _opacityVariable = new VariableSocket();
            _opacityVariable.Init("Opacity",this,3);
	        _opacityVariable.SetAnchorVariableSocket(_anchorVariable);
			_opacityVariable.RequireScalar();

            _colordataRangeInput = (new StyleTypeSocket<MinMax<float>> ()).Init("Color Data Range",this);
            _colordataRangeInput.SetDefaultInputObject((new MinMax<float>(0, 1)));

            _opacitydataRangeInput = (new StyleTypeSocket<MinMax<float>> ()).Init("OpacityData Range",this);
            _opacitydataRangeInput.SetDefaultInputObject((new MinMax<float>(0, 1)));

            _opacityThresholdInput = (new StyleTypeSocket<Range<float>>()).Init("Opacity threshold", this);
            _opacityThresholdInput.SetDefaultInputObject((new Range<float>(0, 1, 0.5f)));
            AddSubmodule(_opacityThresholdInput);


            _directionVariable = new VariableSocket();
            _directionVariable.Init("Direction",this,2);
	        _directionVariable.SetAnchorVariableSocket(_anchorVariable);
			_directionVariable.RequireVector();

            AddSubmodule(_anchorVariable);
            AddSubmodule(_colorVariable);
            AddSubmodule(_colordataRangeInput);
            AddSubmodule(_opacityVariable);
            AddSubmodule(_opacitydataRangeInput);
            AddSubmodule(_directionVariable);

            _maxGlyphs = (new StyleTypeSocket<Range<int>>()).Init("Max glyphs", this);
            _maxGlyphs.SetDefaultInputObject((new Range<int>(0, 60000, 1000)));
            AddSubmodule(_maxGlyphs);

            _colorMapInput = (new StyleTypeSocket<Colormap> ()).Init("Color map",this);
            _glyphInput = (new StyleTypeSocket<Glyph> ()).Init("Glyph",this);

			AddSubmodule(_colorMapInput);
			AddSubmodule(_glyphInput);

            _lodLevel = (new StyleTypeSocket<Range<int>>()).Init("Glyph LOD", this);
            _lodLevel.SetDefaultInputObject((new Range<int>(0, 2,2)));
            AddSubmodule(_lodLevel);


            _glyphScaleInput = (new StyleTypeSocket<Range<float>>()).Init("Glyph scale", this);
            _glyphScaleInput.SetDefaultInputObject((new Range<float>(0, 10,1)));
            AddSubmodule(_glyphScaleInput);


            _useMeshInput = (new StyleTypeSocket<Range<bool>>()).Init("Use mesh", this);
            _useMeshInput.SetDefaultInputObject((new Range<bool>(false, true,true)));
            AddSubmodule(_useMeshInput);

            _useThumbnailInput = (new StyleTypeSocket<Range<bool>>()).Init("Use thumbnail", this);
            _useThumbnailInput.SetDefaultInputObject((new Range<bool>(false, true,false)));
            AddSubmodule(_useThumbnailInput);

            _faceCameraInput = (new StyleTypeSocket<Range<bool>>()).Init("FaceCamera", this);
            _faceCameraInput.SetDefaultInputObject((new Range<bool>(false, true,false)));
            AddSubmodule(_faceCameraInput);

            _opacityMultiplierInput = (new StyleTypeSocket<Range<float>>()).Init("Opacity mult", this);
            _opacityMultiplierInput.SetDefaultInputObject((new Range<float>(0,10,1)));
            AddSubmodule(_opacityMultiplierInput);

            _colorInput = (new StyleTypeSocket<Objectify<Color>>()).Init("GlyphColor", this);
            _colorInput.SetDefaultInputObject(new Objectify< Color>(Color.white));
            AddSubmodule(_colorInput);


            _useColormapInput = (new StyleTypeSocket<Range<bool>>()).Init("UseColormap", this);
            _useColormapInput.SetDefaultInputObject(new Range<bool>(false,true,false));
            AddSubmodule(_useColormapInput);

            _flipColormapInput = (new StyleTypeSocket<Range<bool>>()).Init("FlipColormap", this);
            _flipColormapInput.SetDefaultInputObject(new Range<bool>(false,true,false));
            AddSubmodule(_flipColormapInput);



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

		public override void UpdateModule() {

             if(_colorVariable.GetInput() != null) {

                Variable v = ((Variable)_colorVariable.GetInput());

                ((MinMax<float>)_colordataRangeInput.GetInput()).lowerBound = v.GetMin().x;
                ((MinMax<float>)_colordataRangeInput.GetInput()).upperBound = v.GetMax().x;
            }

            if(_opacityVariable.GetInput() != null) {

                Variable v = ((Variable)_opacityVariable.GetInput());

                ((MinMax<float>)_opacitydataRangeInput.GetInput()).lowerBound = v.GetMin().x;
                ((MinMax<float>)_opacitydataRangeInput.GetInput()).upperBound = v.GetMax().x;
            }

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

