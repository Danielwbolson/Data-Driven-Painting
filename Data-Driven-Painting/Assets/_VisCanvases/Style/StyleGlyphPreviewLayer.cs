using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    [CreateAssetMenu()]
    public class StyleGlyphPreviewLayer : StyleLayer
    {

		public override string GetTypeTag() {
			return "GLYPHPREVIEW_LAYER";
		}


        [SerializeField]
        public StyleTypeSocket<Glyph> _glyphInput;

        [SerializeField]
        public StyleTypeSocket<Range<int>> _lodLevel;


        [SerializeField]
        public StyleTypeSocket<Range<bool>> _transformVerts;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _transformNormals;

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _transformTangents;

        [SerializeField]
        public Material _glyphMaterial;

        public int LODlevel = 0;



        public override bool HasBounds()
        {
            return _glyphInput.GetInput() != null;
        }
        public override Bounds GetBounds()
        {
            if(_glyphInput.GetInput() != null && ((Glyph)(_glyphInput.GetInput())).GetNumberOfLODs() > 0)
                return ((Glyph)(_glyphInput.GetInput())).GetLODMesh(0).bounds; 
            return new Bounds();
        }

        public override void DrawLayer(Canvas canvas)
        {
           

            _glyphMaterial.SetMatrix("_GlyphTransform",Matrix4x4.TRS(new Vector3(0,0,0),GameObject.Find("GlyphTransform").transform.rotation,GameObject.Find("GlyphTransform").transform.localScale));
            _glyphMaterial.SetMatrix("_GlyphTransformInverse",Matrix4x4.TRS(new Vector3(0,0,0),GameObject.Find("GlyphTransform").transform.rotation,GameObject.Find("GlyphTransform").transform.localScale).inverse);


            _glyphMaterial.SetInt("_TransformVertices",(Range<bool>)_transformVerts.GetInput()?1:0);
            _glyphMaterial.SetInt("_TransformNormals",(Range<bool>)_transformNormals.GetInput()?1:0);
            _glyphMaterial.SetInt("_TransformTangents",(Range<bool>)_transformTangents.GetInput()?1:0);



            if(_glyphInput.GetInput() != null)
            {
                if(((Glyph)(_glyphInput.GetInput())).HasNormals()) {
                    _glyphMaterial.SetTexture("_BumpMap",((Glyph)(_glyphInput.GetInput())).GetLODNormalMap(((Range<int>)_lodLevel.GetInput())));
                    _glyphMaterial.SetInt("_hasBumpMap", 1);
                } else {
                    _glyphMaterial.SetInt("_hasBumpMap", 0);

                }
               

                Material canvasMaterial = GetCanvasMaterial(canvas, _glyphMaterial);

                Graphics.DrawMesh(((Glyph)(_glyphInput.GetInput())).GetLODMesh(((Range<int>)_lodLevel.GetInput())), canvas.GetInnerSceneTransformMatrix(), canvasMaterial, 0);
            }

        }


        public override StyleLayer CopyLayer(StyleLayer toCopy = null)
        {
            if(toCopy == null)
                toCopy = new StyleGlyphPreviewLayer();

            if (toCopy != null && toCopy is StyleGlyphPreviewLayer)
            {
                _glyphMaterial = new Material(((StyleGlyphPreviewLayer)toCopy)._glyphMaterial);

            }

            return Init();
        }

        public StyleGlyphPreviewLayer Init()
        {
          
			_glyphInput = (new StyleTypeSocket<Glyph>()).Init("Glyph",this);
			AddSubmodule(_glyphInput);

			_lodLevel = (new StyleTypeSocket<Range<int>>()).Init("Glyph LOD",this);
            _lodLevel.SetDefaultInputObject((new Range<int>(0,2)));
			AddSubmodule(_lodLevel);

			_transformVerts = (new StyleTypeSocket<Range<bool>>()).Init("Use GlyphTransform on verts",this);
            _transformVerts.SetDefaultInputObject((new Range<bool>(false,true,true)));
			AddSubmodule(_transformVerts);


            _transformNormals = (new StyleTypeSocket<Range<bool>>()).Init("Use GlyphTransform on norms",this);
            _transformNormals.SetDefaultInputObject((new Range<bool>(false,true,true)));
			AddSubmodule(_transformNormals);



            _transformTangents = (new StyleTypeSocket<Range<bool>>()).Init("Use GlyphTransform on tangents",this);
            _transformTangents.SetDefaultInputObject((new Range<bool>(false,true,true)));
			AddSubmodule(_transformTangents);

            return this;

        }

		public override void UpdateModule(string updatedSocket = null) {

		}


        public override string GetLabel()
        {
            return "Glyph Preview Layer";
        }

    }
}
