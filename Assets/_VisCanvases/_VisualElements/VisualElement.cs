using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis {
	public abstract class VisualElement : ScriptableObject {
		[SerializeField]
		string _name = "";

		protected void SetName(string name) {
			_name = name;
		}

		public string GetName() {
			return _name;
		}



		public static VisualElement[] LoadFile(string filePath) {
			List<VisualElement> resultList = new List<VisualElement>();
			VisualElement result = null;
			VisualElement [] results = null;
			if((results = Colormap.LoadFile(filePath)).Length > 0) {
				foreach(var r in results)
				resultList.Add(r);
			}

            if ((result = Glyph.LoadFile(filePath)) != null) {
                ((Glyph)result).SetFilePath(filePath);
                resultList.Add(result);
            }

			return resultList.ToArray();
		}

		public abstract Texture2D GetPreviewImage();

		public abstract float GetPreviewImageAspectRatio();
		
	}
}
