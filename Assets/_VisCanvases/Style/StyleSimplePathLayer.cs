using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StyleSimplePathLayer : StyleLayer
    {



        [SerializeField]
        public VariableSocket _anchorVariable;

        [SerializeField]
        public VariableSocket _colorVariable;


        [SerializeField]
        public VariableSocket _opacityVariable;


        [SerializeField]
        public StyleColormapSocket _colorMapInput;

        [SerializeField]
        public Texture2D _colorMap;

        [SerializeField]
        public Material _lineMaterial;

        [SerializeField]
        public int LineCount = 1000;



        public override bool HasBounds()
        {
            return _anchorVariable != null && _anchorVariable.IsAssigned();
        }
        public override Bounds GetBounds()
        {
            return _anchorVariable.GetInput().GetBounds();
        }

        public override void DrawLayer(Canvas canvas)
        {
            if (_anchorVariable == null || !_anchorVariable.IsAssigned()) return;
            Datastream stream = _anchorVariable.GetInput().GetStream(null, 0, 0);



            Mesh[] m = stream.GetMeshes();
            // if(_colorVariable.IsAssigned()) {
            // 	Texture3D tex = _colorVariable.GetInput().GetStream(null,0,0).Get3DTexture();
            // 	_lineMaterial.SetTexture("_ColorData",tex);
            // 	_lineMaterial.SetInt("_HasColorVariable",1);
            // 	_lineMaterial.SetMatrix("_DataBoundsMatrixInv",Matrix4x4.TRS(_colorVariable.GetBounds().center,Quaternion.identity,_colorVariable.GetBounds().size).inverse);

            // } else {
            // 	_lineMaterial.SetInt("_HasColorVariable",0);

            // }
            _lineMaterial.SetTexture("_ColorMap", _colorMap);

            Material canvasMaterial = GetCanvasMaterial(canvas, _lineMaterial);
            _anchorVariable.Bind(canvasMaterial, 0, 0);
            _colorVariable.Bind(canvasMaterial, 0, 0);
            _opacityVariable.Bind(canvasMaterial, 0, 0);

            if (m != null && m.Length > 0)
            {
                for (int i = 0; i < Mathf.Min(m.Length, LineCount); i += 1)
                {
                    Mesh mesh = m[i];
                    if (mesh != null)
                    {
                        Graphics.DrawMesh(mesh, canvas.GetInnerSceneTransformMatrix(), canvasMaterial, 0);

                    }
                }
            }

        }


        public override StyleLayer CopyLayer(StyleLayer toCopy)
        {
            if (toCopy != null && toCopy is StyleSimplePathLayer)
            {
                _lineMaterial = ((StyleSimplePathLayer)toCopy)._lineMaterial;
                LineCount = ((StyleSimplePathLayer)toCopy).LineCount;
				 _colorMap = ((StyleSimplePathLayer)toCopy)._colorMap;

            }

            return Init();
        }

        public StyleSimplePathLayer Init()
        {
            _anchorVariable = CreateInstance<VariableSocket>();
            _anchorVariable.Init("Anchor",this);
            //SetAnchorSocket(_anchorVariable);
            _colorVariable = CreateInstance<VariableSocket>();
            _colorVariable.Init("Color",this,1);
            _colorVariable.SetAnchorVariableSocket(_anchorVariable);
			_colorVariable.RequireScalar();

            _opacityVariable = CreateInstance<VariableSocket>();
            _opacityVariable.Init("Opacity",this,3);
	        _opacityVariable.SetAnchorVariableSocket(_anchorVariable);
			_opacityVariable.RequireScalar();

            AddSocket(_anchorVariable);
            AddSocket(_colorVariable);
            AddSocket(_opacityVariable);
			_colorMapInput = (StyleColormapSocket)CreateInstance<StyleColormapSocket>().Init("Color map",this,true,false);
			AddSocket(_colorMapInput);

            return this;

        }

		public override void UpdateModule() {
			if(_colorMapInput.GetTexture() != null) {
				_colorMap = (Texture2D)_colorMapInput.GetTexture();
			}
		}

        public override string GetLabel()
        {
            return "Simple Path Layer";
        }

    }
}
