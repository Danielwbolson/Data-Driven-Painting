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

		public string absolute_path = "";
		public string persistent_path = "";

		public string streaming_path = "";

		public static VisualElement[] LoadFile(string filePath) {
			List<VisualElement> resultList = new List<VisualElement>();
			VisualElement result = null;
			VisualElement [] results = null;
			if((results = Colormap.LoadFile(filePath)).Length > 0) {
				foreach(var r in results) {
					resultList.Add(r);

					if(filePath.StartsWith(Application.persistentDataPath) ){
						r.persistent_path = filePath.Substring(Application.persistentDataPath.Length);
					} else if(filePath.StartsWith(Application.streamingAssetsPath) ) {
						r.streaming_path = filePath.Substring( Application.streamingAssetsPath.Length);
					} else {
						r.absolute_path = filePath;
					}
				}
			}

            if ((result = Glyph.LoadFile(filePath)) != null) {
                ((Glyph)result).SetFilePath(filePath);
                resultList.Add(result);
				if(filePath.StartsWith(Application.persistentDataPath) ){
					result.persistent_path = filePath.Substring(Application.persistentDataPath.Length);
				} else if(filePath.StartsWith(Application.streamingAssetsPath) ) {
					result.streaming_path = filePath.Substring( Application.streamingAssetsPath.Length);
				}  else {
					result.absolute_path = filePath;
				}
            }

			return resultList.ToArray();
		}

		public abstract Texture2D GetPreviewImage();

		public abstract float GetPreviewImageAspectRatio();
		
	}
}
