using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SculptingVis.SmartData{

	public class SmartDataObject : StyleModule  {
		string _name;
		public string GetName() {
			return _name;
		}

		public void SetName(string name) {
			_name = name;
		}
		public override string GetLabel() {
			return GetName();
		}
        public override string ToString() {
            return "(" + base.ToString() +") " + GetName();
        }

		public virtual void Update() {

		}
	}
}