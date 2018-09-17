using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    public abstract class StyleLayer : StyleModule
    {
        public JSONObject GetSerialized() {

            JSONObject result = new JSONObject();
            result.AddField("id",GetUniqueIdentifier());
            return result;
        }
        [HideInInspector]
        public bool _toggled = true;

        public StyleLayer Init()
        {
            return this;
        }


        public override string GetLabel()
        {
            return "Layer";
        }

        public virtual bool HasBounds()
        {
            return false;
        }
        public virtual Bounds GetBounds()
        {
            return new Bounds();
        }
        bool _active = true;
        public void SetActive(bool b) {
            _active = b;
        }
        public bool IsActive() {
            return _active;
        }
        public virtual void DrawLayer(Canvas canvas)
        {

        }


        [SerializeField, HideInInspector]
        Dictionary<Canvas, Material> _canvasMaterials;

        protected Material GetCanvasMaterial(Canvas canvas, Material layerMaterial)
        {
            if (layerMaterial == null)
            {
                Debug.LogError("Layer Material is null");
                return null;
            }
            if (_canvasMaterials == null)
                _canvasMaterials = new Dictionary<Canvas, Material>();
            Material canvasMaterial;

            if (!_canvasMaterials.ContainsKey(canvas) || _canvasMaterials[canvas] == null)
                canvasMaterial = (_canvasMaterials[canvas] = GameObject.Instantiate(layerMaterial));
            else
                canvasMaterial = _canvasMaterials[canvas];

            canvasMaterial.CopyPropertiesFromMaterial(layerMaterial);
            canvas.SetMaterialProperties(canvasMaterial);
            return canvasMaterial;
        }

        public abstract StyleLayer CopyLayer(StyleLayer toCopy=null);

    }
}

