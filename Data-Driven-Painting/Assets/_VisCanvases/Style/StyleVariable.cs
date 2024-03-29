using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis{
	public abstract class StyleVariable : StyleModule {
		[SerializeField]
		protected Variable _variable;

		public override string GetTypeTag() {
			return "VARIABLE";
		}

        public void SetVariable(Variable v) {
            _variable = v;
        }

        
		public override string GetLabel() {
			return _variable + "";
		}

        public virtual StyleVariable CopyVariable(StyleVariable toCopy)
        {
            return this;
        }

        public virtual bool IsValid()
        {
            return true;
        }

    }
}

