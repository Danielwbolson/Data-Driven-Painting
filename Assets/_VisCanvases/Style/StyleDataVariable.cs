using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis{
	public class StyleDataVariable : StyleVariable {
		// public override JSONObject GetSerializedJSONObject() { 
		// 	return null;
		// }




		public override string GetTypeTag() {
			return "DATAVARIABLE";
		}
		        public StyleDataVariable Init(Variable variable) {
			_variable = variable;
			AddSubmodule((new StyleSocket()).Init("",this,false,true,_variable));
			// AddSubmodule((new StyleSocket()).Init("Hook for deriving",this,false,true,_variable));

            return this;
        }

        
		public override string GetLabel() {
			return _variable + "";
		}

	}
}

