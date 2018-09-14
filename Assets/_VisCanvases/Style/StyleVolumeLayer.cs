using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StyleVolumeLayer : StyleLayer
    {

        
        float map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s-a1)*(b2-b1)/(a2-a1);
        }


        // [SerializeField]
        // public VariableSocket _volumeVariable;



        // public StyleTypeSocket<Colormap> _colorMapInput;
        // public StyleTypeSocket<Colormap>  _opacityMapInput;
        public StyleTypeSocket<Range<float>> _opacityMultiplierInput;
        // public StyleTypeSocket<MinMax<float>> _dataRangeInput;


        [SerializeField]
        public StyleColorModifier _colorModifier;

        [SerializeField]
        public StyleOpacityModifier _opacityModifier;

        // [SerializeField]
        // public StyleTypeSocket<Objectify<Color>> _colorInput;

        // [SerializeField]
        // public StyleTypeSocket<Range<bool>> _useColormapInput;

        // [SerializeField]
        // public StyleTypeSocket<Range<bool>> _flipColormapInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _staggerPathInput;

        [SerializeField]
        Material _volumeMaterial;

        [SerializeField]
        Mesh _volumeCubeMesh;
        [SerializeField]
        public StyleTypeSocket<Range<bool>> _usePlane1;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _usePlane2;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _usePlane3;




        public override bool HasBounds()
        {
            return _colorModifier._variable.IsAssigned();
        }
        public override Bounds GetBounds()
        {
            return ((Variable)_colorModifier._variable.GetInput()).GetBounds();
        }

        public override void DrawLayer(Canvas canvas)
        {

            
            if (!_colorModifier._variable.IsAssigned()) return;


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


            Datastream stream = ((Variable)_colorModifier._variable.GetInput()).GetStream(null, 0, 0);

            _volumeMaterial.SetTexture("_ColorMap", ((Colormap)_colorModifier._colormapSocket.GetInput()).GetTexture());
            _volumeMaterial.SetTexture("_OpacityMap", ((Colormap)_opacityModifier._opacitymapSocket.GetInput()).GetTexture());


            _volumeMaterial.SetColor("_Color", (Objectify<Color>)_colorModifier._colorSocket.GetInput());
            _volumeMaterial.SetFloat("_Opacity", (Range<float>)_opacityModifier._opacitySocket.GetInput());


            _volumeMaterial.SetInt("_useColormap", (Range<bool>)_colorModifier._useVariable.GetInput()?1:0);
            _volumeMaterial.SetInt("_useOpacitymap", (Range<bool>)_opacityModifier._useVariable.GetInput()?1:0);

            _volumeMaterial.SetInt("_flipColormap", (Range<bool>)_colorModifier._flipColormapSocket.GetInput()?1:0);
            _volumeMaterial.SetInt("_flipOpacitymap", (Range<bool>)_opacityModifier._flipOpacityMapSocket.GetInput()?1:0);





            // if(_colorMapInput.GetInput() != null) _volumeMaterial.SetTexture("_ColorMap", ((Colormap)_colorMapInput.GetInput()).GetTexture());
            // if(_opacityMapInput.GetInput() != null) _volumeMaterial.SetTexture("_OpacityMap", ((Colormap)_opacityMapInput.GetInput()).GetTexture());
            // Range<float> o = ((Range<float>)_opacityMultiplierInput.GetInput());

            // _colorModifier._variable.LowerBound = ((MinMax<float>)_dataRangeInput.GetInput()).lowerValue;
            // _colorModifier._variable.UpperBound = ((MinMax<float>)_dataRangeInput.GetInput()).upperValue;

            _volumeMaterial.SetFloat("_OpacityMultiplier", ((Range<float>)_opacityMultiplierInput.GetInput()));
            // _volumeMaterial.SetInt("_useColormap", (Range<bool>)_useColormapInput.GetInput()?1:0);
            // _volumeMaterial.SetInt("_flipColormap", (Range<bool>)_flipColormapInput.GetInput()?1:0);
            _volumeMaterial.SetInt("_stagger", (Range<bool>)_staggerPathInput.GetInput()?1:0);

            // _volumeMaterial.SetColor("_Color", (Objectify<Color>)_colorInput.GetInput());

            _volumeMaterial.SetInt("_usePlane1", (Range<bool>)_usePlane1.GetInput()?1:0);
            _volumeMaterial.SetInt("_usePlane2", (Range<bool>)_usePlane2.GetInput()?1:0);
            _volumeMaterial.SetInt("_usePlane3", (Range<bool>)_usePlane3.GetInput()?1:0);


            _volumeMaterial.SetVector("_plane1min", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMins()[0]);
            _volumeMaterial.SetVector("_plane2min", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMins()[1]);
            _volumeMaterial.SetVector("_plane3min", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMins()[2]);

            _volumeMaterial.SetVector("_plane1max", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMaxes()[0]);
            _volumeMaterial.SetVector("_plane2max", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMaxes()[1]);
            _volumeMaterial.SetVector("_plane3max", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMaxes()[2]);

            Material canvasMaterial = GetCanvasMaterial(canvas, _volumeMaterial);
            // _volumeVariable.Bind(canvasMaterial, 0, 0);
            _colorModifier._variable.Bind(_volumeMaterial, 0, 0);
            _opacityModifier._variable.Bind(_volumeMaterial, 0, 0);

            Graphics.DrawMesh(_volumeCubeMesh,canvas.GetInnerSceneTransformMatrix()*Matrix4x4.TRS(GetBounds().center,Quaternion.identity,GetBounds().size),canvasMaterial,0);


        }


        public override StyleLayer CopyLayer(StyleLayer toCopy)
        {
            if (toCopy != null && toCopy is StyleVolumeLayer)
            {
                _volumeMaterial = new Material(((StyleVolumeLayer)toCopy)._volumeMaterial);


                _volumeCubeMesh = ((StyleVolumeLayer)toCopy)._volumeCubeMesh;

            }

            return Init();
        }

        public new StyleVolumeLayer Init()
        {

            //SetAnchorSocket(_anchorVariable);
            // _volumeVariable = new VariableSocket();
            // _volumeVariable.Init("Volume",this,1);
            // _volumeVariable.SetAnchorVariableSocket(null);
			// _volumeVariable.RequireScalar();


            // AddSubmodule(_volumeVariable);

            
            _colorModifier = CreateInstance<StyleColorModifier>();
            _colorModifier.Init(null,this,1);
            AddSubmodule(_colorModifier);
            

            _opacityModifier = CreateInstance<StyleOpacityModifier>();
            _opacityModifier.Init(null,this,3);
            AddSubmodule(_opacityModifier);


			// _colorMapInput = (new StyleTypeSocket<Colormap> ()).Init("Color map",this);
            // _colorMapInput.SetDefaultInputObject(Colormap.DefaultColormap());
            // _opacityMapInput = (new StyleTypeSocket<Colormap> ()).Init("Opacity map",this);
            // _opacityMapInput.SetDefaultInputObject(Colormap.DefaultColormap());
            _opacityMultiplierInput = (new StyleTypeSocket<Range<float>> ()).Init("Opacity multiplier",this);
            _opacityMultiplierInput.SetDefaultInputObject((new Range<float>(0, 1,1f)));


            // _dataRangeInput = (new StyleTypeSocket<MinMax<float>> ()).Init("Data Range",this);
            // _dataRangeInput.SetDefaultInputObject((new MinMax<float>(0, 1)));

			// AddSubmodule(_colorMapInput);
			// AddSubmodule(_opacityMapInput);
			AddSubmodule(_opacityMultiplierInput);
            // AddSubmodule(_dataRangeInput);



            // _colorInput = (new StyleTypeSocket<Objectify<Color>>()).Init("GlyphColor", this);
            // _colorInput.SetDefaultInputObject(new Objectify< Color>(Color.white));
            // AddSubmodule(_colorInput);


            // _useColormapInput = (new StyleTypeSocket<Range<bool>>()).Init("UseColormap", this);
            // _useColormapInput.SetDefaultInputObject(new Range<bool>(false,true,false));
            // AddSubmodule(_useColormapInput);

            // _flipColormapInput = (new StyleTypeSocket<Range<bool>>()).Init("FlipColormap", this);
            // _flipColormapInput.SetDefaultInputObject(new Range<bool>(false,true,false));
            // AddSubmodule(_flipColormapInput);

            _staggerPathInput = (new StyleTypeSocket<Range<bool>>()).Init("Stagger path starts", this);
            _staggerPathInput.SetDefaultInputObject(new Range<bool>(false,true,false));
            AddSubmodule(_staggerPathInput);



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
            if(_colorModifier._variable.GetInput() != null) {
                Variable v = ((Variable)_colorModifier._variable.GetInput());
            // ((MinMax<float>)_dataRangeInput.GetInput()).lowerBound = v.GetMin().x;
            // ((MinMax<float>)_dataRangeInput.GetInput()).upperBound = v.GetMax().x;


            }
			// if(_colorMapInput.GetInput() != null && _colorMapInput.GetInput() is Colormap) {
			// 	_colorMap = ((Colormap)_colorMapInput.GetInput()).GetTexture();
			// }
			// if(_opacityMapInput.GetInput() != null && _opacityMapInput.GetInput() is Colormap) {
			// 	_opacityMap = ((Colormap)_opacityMapInput.GetInput()).GetTexture();
			// }
		} 

        public override string GetLabel()
        {
            return "Volume Render Layer";
        }

    }
}

