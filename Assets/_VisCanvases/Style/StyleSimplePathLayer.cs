using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis {
    [CreateAssetMenu()]
    public class StyleSimplePathLayer : StyleLayer {



        [SerializeField]
        public VariableSocket _anchorVariable;

        [SerializeField]
        public VariableSocket _colorVariable;
        public StyleTypeSocket<MinMax<float>> _colordataRangeInput;


        [SerializeField]
        public VariableSocket _opacityVariable;

        public StyleTypeSocket<MinMax<float>> _opacitydataRangeInput;

        [SerializeField]
        public StyleTypeSocket<Colormap> _colorMapInput;

        [SerializeField]
        public StyleTypeSocket<Range<int>> _maxPaths;

        [SerializeField]
        public Material _lineMaterial;

        [SerializeField]
        public int LineCount = 1000;

        [SerializeField]
        public StyleTypeSocket<Objectify<Color>> _colorInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _useColormapInput;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _flipColormapInput;


        [SerializeField]
        public StyleTypeSocket<Range<bool>> _usePlane1;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _usePlane2;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _usePlane3;
        public override bool HasBounds() {
            return _anchorVariable != null && _anchorVariable.IsAssigned();
        }
        public override Bounds GetBounds() {
            return ((Variable)_anchorVariable.GetInput()).GetBounds();
        }

        public override void DrawLayer(Canvas canvas) {
            if (_anchorVariable == null || !_anchorVariable.IsAssigned()) return;
            Datastream stream = ((Variable)_anchorVariable.GetInput()).GetStream(null, 0, 0);



            Mesh[] m = stream.GetMeshes();
            // if(_colorVariable.IsAssigned()) {
            // 	Texture3D tex = _colorVariable.GetInput().GetStream(null,0,0).Get3DTexture();
            // 	_lineMaterial.SetTexture("_ColorData",tex);
            // 	_lineMaterial.SetInt("_HasColorVariable",1);
            // 	_lineMaterial.SetMatrix("_DataBoundsMatrixInv",Matrix4x4.TRS(_colorVariable.GetBounds().center,Quaternion.identity,_colorVariable.GetBounds().size).inverse);

            // } else {
            // 	_lineMaterial.SetInt("_HasColorVariable",0);

            // }

            _colorVariable.LowerBound = ((MinMax<float>)_colordataRangeInput.GetInput()).lowerValue;
            _colorVariable.UpperBound = ((MinMax<float>)_colordataRangeInput.GetInput()).upperValue;

            _opacityVariable.LowerBound = ((MinMax<float>)_opacitydataRangeInput.GetInput()).lowerValue;
            _opacityVariable.UpperBound = ((MinMax<float>)_opacitydataRangeInput.GetInput()).upperValue;


            if (_colorMapInput.GetInput() != null)
                _lineMaterial.SetTexture("_ColorMap", ((Colormap)_colorMapInput.GetInput()).GetTexture());
            _lineMaterial.SetInt("_useColormap", (Range<bool>)_useColormapInput.GetInput()?1:0);
            _lineMaterial.SetInt("_flipColormap", (Range<bool>)_flipColormapInput.GetInput()?1:0);
            _lineMaterial.SetColor("_Color", (Objectify<Color>)_colorInput.GetInput());


            _lineMaterial.SetInt("_usePlane1", (Range<bool>)_usePlane1.GetInput()?1:0);
            _lineMaterial.SetInt("_usePlane2", (Range<bool>)_usePlane2.GetInput()?1:0);
            _lineMaterial.SetInt("_usePlane3", (Range<bool>)_usePlane3.GetInput()?1:0);

            _lineMaterial.SetVector("_plane1min", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMins()[0]);
            _lineMaterial.SetVector("_plane2min", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMins()[1]);
            _lineMaterial.SetVector("_plane3min", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMins()[2]);

            _lineMaterial.SetVector("_plane1max", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMaxes()[0]);
            _lineMaterial.SetVector("_plane2max", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMaxes()[1]);
            _lineMaterial.SetVector("_plane3max", ((StyleController)FindObjectOfType(typeof(StyleController))).GetPlaneMaxes()[2]);

            Material canvasMaterial = GetCanvasMaterial(canvas, _lineMaterial);
            _anchorVariable.Bind(canvasMaterial, 0, 0);
            _colorVariable.Bind(canvasMaterial, 0, 0);
            _opacityVariable.Bind(canvasMaterial, 0, 0);

            if (m != null && m.Length > 0) {
                for (int i = 0; i < Mathf.Min(m.Length, (Range<int>)_maxPaths.GetInput()); i += 1) {
                    Mesh mesh = m[i];
                    if (mesh != null) {
                        Graphics.DrawMesh(mesh, canvas.GetInnerSceneTransformMatrix(), canvasMaterial, 0);

                    }
                }
            }

        }


        public override StyleLayer CopyLayer(StyleLayer toCopy) {
            if (toCopy != null && toCopy is StyleSimplePathLayer) {
                _lineMaterial = new Material(((StyleSimplePathLayer)toCopy)._lineMaterial);
                LineCount = ((StyleSimplePathLayer)toCopy).LineCount;

            }

            return Init();
        }

        public StyleSimplePathLayer Init() {
            _anchorVariable = new VariableSocket();
            _anchorVariable.Init("Anchor", this, 0);
            //SetAnchorSocket(_anchorVariable);
            _colorVariable = new VariableSocket();
            _colorVariable.Init("Color", this, 1);
            _colorVariable.SetAnchorVariableSocket(_anchorVariable);
            _colorVariable.RequireScalar();

            _opacityVariable = new VariableSocket();
            _opacityVariable.Init("Opacity", this, 3);
            _opacityVariable.SetAnchorVariableSocket(_anchorVariable);
            _opacityVariable.RequireScalar();


            _colordataRangeInput = (new StyleTypeSocket<MinMax<float>> ()).Init("Color Data Range",this);
            _colordataRangeInput.SetDefaultInputObject((new MinMax<float>(0, 1)));

            _opacitydataRangeInput = (new StyleTypeSocket<MinMax<float>> ()).Init("OpacityData Range",this);
            _opacitydataRangeInput.SetDefaultInputObject((new MinMax<float>(0, 1)));

            AddSubmodule(_anchorVariable);
            AddSubmodule(_colorVariable);
            AddSubmodule(_colordataRangeInput);
            AddSubmodule(_colordataRangeInput);

            AddSubmodule(_opacityVariable);
            AddSubmodule(_opacitydataRangeInput);

            _colorMapInput = (new StyleTypeSocket<Colormap>()).Init("Color map", this);
            AddSubmodule(_colorMapInput);


            _maxPaths = (new StyleTypeSocket<Range<int>>()).Init("Max paths", this);
            _maxPaths.SetDefaultInputObject(new Range<int>(1, 50000, 1000));
            AddSubmodule(_maxPaths);



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
            if (_colorMapInput.GetInput() != null && _colorMapInput.GetInput() is Colormap) {
            
            
            }
        }


        public override string GetLabel() {
            return "Simple Path Layer";
        }

    }
}

