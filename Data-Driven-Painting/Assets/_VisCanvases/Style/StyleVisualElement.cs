using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis{

	
	public class StyleVisualElement : StyleModule {


		public override JSONObject serialize() {
			JSONObject json = new JSONObject();
			if(_visualElement.absolute_path != "")
				json.AddField("absolutepath",_visualElement.absolute_path.Replace("\\", "/"));
			if(_visualElement.persistent_path != "")
				json.AddField("persistentpath",_visualElement.persistent_path.Replace("\\", "/"));
			
			if(_visualElement.streaming_path != "")
				json.AddField("streamingpath", _visualElement.streaming_path.Replace("\\", "/"));
			
			return json;
		}

		public override string GetTypeTag() {
			return "VISUALELEMENT";
		}


        public StyleVisualElement Init(VisualElement visualElement) {
			_visualElement = visualElement;
			AddSubmodule((new StyleSocket()).Init("",this,false,true,_visualElement));

            return this;
        }

		[SerializeField]
		VisualElement _visualElement;
        
		public VisualElement GetVisualElement() {return _visualElement;}
		public override string GetLabel() {
			if(_visualElement != null && _visualElement.GetName() != "") return _visualElement.GetName();
			return  "Visual Element";
		}

	}
}

