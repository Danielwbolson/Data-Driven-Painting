using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StyleVolumeLayer : StyleLayer
    {
        [SerializeField]
        public VariableSocket _volumeVariable;



        public StyleTypeSocket<Colormap> _colorMapInput;
        public StyleTypeSocket<Colormap>  _opacityMapInput;
        public StyleTypeSocket<Range<float>> _opacityMultiplierInput;
        public StyleTypeSocket<MinMax<float>> _dataRangeInput;


        [SerializeField]
        public StyleTypeSocket<Objectify<Color>> _colorInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _useColormapInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _flipColormapInput;


        [SerializeField]
        Material _volumeMaterial;

        [SerializeField]
        Mesh _volumeCubeMesh;





        public override bool HasBounds()
        {
            return _volumeVariable != null && _volumeVariable.IsAssigned();
        }
        public override Bounds GetBounds()
        {
            return ((Variable)_volumeVariable.GetInput()).GetBounds();
        }

        public override void DrawLayer(Canvas canvas)
        {
            if (_volumeVariable == null || !_volumeVariable.IsAssigned()) return;
            Datastream stream = ((Variable)_volumeVariable.GetInput()).GetStream(null, 0, 0);



            if(_colorMapInput.GetInput() != null) _volumeMaterial.SetTexture("_ColorMap", ((Colormap)_colorMapInput.GetInput()).GetTexture());
            if(_opacityMapInput.GetInput() != null) _volumeMaterial.SetTexture("_OpacityMap", ((Colormap)_opacityMapInput.GetInput()).GetTexture());
            Range<float> o = ((Range<float>)_opacityMultiplierInput.GetInput());

            _volumeVariable.LowerBound = ((MinMax<float>)_dataRangeInput.GetInput()).lowerValue;
            _volumeVariable.UpperBound = ((MinMax<float>)_dataRangeInput.GetInput()).upperValue;

            _volumeMaterial.SetFloat("_OpacityMultiplier", ((Range<float>)_opacityMultiplierInput.GetInput()));
            _volumeMaterial.SetInt("_useColormap", (Range<bool>)_useColormapInput.GetInput()?1:0);
            _volumeMaterial.SetInt("_flipColormap", (Range<bool>)_flipColormapInput.GetInput()?1:0);

            _volumeMaterial.SetColor("_Color", (Objectify<Color>)_colorInput.GetInput());

            Material canvasMaterial = GetCanvasMaterial(canvas, _volumeMaterial);
            _volumeVariable.Bind(canvasMaterial, 0, 0);


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
            _volumeVariable = new VariableSocket();
            _volumeVariable.Init("Volume",this,1);
            _volumeVariable.SetAnchorVariableSocket(null);
			_volumeVariable.RequireScalar();


            AddSubmodule(_volumeVariable);
			_colorMapInput = (new StyleTypeSocket<Colormap> ()).Init("Color map",this);
            _opacityMapInput = (new StyleTypeSocket<Colormap> ()).Init("Opacity map",this);
            _opacityMultiplierInput = (new StyleTypeSocket<Range<float>> ()).Init("Opacity multiplier",this);
            _opacityMultiplierInput.SetDefaultInputObject((new Range<float>(0, 1,1f)));


            _dataRangeInput = (new StyleTypeSocket<MinMax<float>> ()).Init("Data Range",this);
            _dataRangeInput.SetDefaultInputObject((new MinMax<float>(0, 1)));

			AddSubmodule(_colorMapInput);
			AddSubmodule(_opacityMapInput);
			AddSubmodule(_opacityMultiplierInput);
            AddSubmodule(_dataRangeInput);



            _colorInput = (new StyleTypeSocket<Objectify<Color>>()).Init("GlyphColor", this);
            _colorInput.SetDefaultInputObject(new Objectify< Color>(Color.white));
            AddSubmodule(_colorInput);


            _useColormapInput = (new StyleTypeSocket<Range<bool>>()).Init("UseColormap", this);
            _useColormapInput.SetDefaultInputObject(new Range<bool>(false,true,false));
            AddSubmodule(_useColormapInput);

            _flipColormapInput = (new StyleTypeSocket<Range<bool>>()).Init("FlipColormap", this);
            _flipColormapInput.SetDefaultInputObject(new Range<bool>(false,true,false));
            AddSubmodule(_flipColormapInput);

            return this;

        }

		public override void UpdateModule() {
            if(_volumeVariable.GetInput() != null) {
                Variable v = ((Variable)_volumeVariable.GetInput());
            ((MinMax<float>)_dataRangeInput.GetInput()).lowerBound = v.GetMin().x;
            ((MinMax<float>)_dataRangeInput.GetInput()).upperBound = v.GetMax().x;


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

