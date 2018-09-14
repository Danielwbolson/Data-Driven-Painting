using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis{
	public class StyleDataset : StyleModule {
		[SerializeField]
		protected Dataset _dataset;

        public void SetDataset(Dataset dataset) {
            _dataset = dataset;
        }
        Dataset _cachedDataset;

        public Dataset GetDataset() {
            return _dataset;
        }
	    public override string GetLabel() {
			return _dataset + "";
		}

        public virtual StyleDataset CopyDataset(StyleDataset toCopy)
        {
            return toCopy;
        }

        public virtual bool IsValid()
        {
            return true;
        }
        
        StyleVariable _anchorVariable;
        List<StyleVariable> _variables;
        
        public override void UpdateModule(string updatedSocket = null) {
            if(GetDataset() == null) return;
            if(GetDataset()!= null && _cachedDataset!= null && GetDataset().GetHashCode() != _cachedDataset.GetHashCode()){
                _anchorVariable = null;
                _variables = null;
            }
            if(GetDataset() != null && GetDataset().GetAnchor() != null)
                if(_anchorVariable == null) _anchorVariable = ScriptableObject.CreateInstance<StyleDataVariable>().Init(GetDataset().GetAnchor());
            if(_variables == null){
                _variables = new List<StyleVariable>();
                for(int i =0; i < GetDataset().GetVariables().Length; i++) {
                    if(GetDataset().GetVariables()[i].IsAnchor())
                        continue;
                    _variables.Add(ScriptableObject.CreateInstance<StyleDataVariable>().Init(GetDataset().GetVariables()[i]));
                }
            } 
            int index =0;
            for(int i =0; i < GetDataset().GetVariables().Length; i++) {
                if(GetDataset().GetVariables()[i].IsAnchor())
                    continue;
                _variables[index].SetVariable(GetDataset().GetVariables()[index]);
                index++;
            }
            _cachedDataset = GetDataset();
            base.UpdateModule();
        }
        public override int GetNumberOfSubmodules() {
            if(GetDataset() != null)
                return base.GetNumberOfSubmodules()+ GetDataset().GetVariables().Length;
            return base.GetNumberOfSubmodules();
			//return GetAnchors().Count + GetAnchoredVariables().Count + GetContinuousVariables().Count;
		}


		public override StyleModule GetSubmodule(int i) {
            if(i < base.GetNumberOfSubmodules()) {
                return base.GetSubmodule(i);
            }
            if(_anchorVariable != null) {
                if(i == base.GetNumberOfSubmodules() )
                    return _anchorVariable;
                else
                    return _variables[(i-base.GetNumberOfSubmodules()-1)];
            
            } else {
                return _variables[i - base.GetNumberOfSubmodules()];
            } 
           
		}
		// public override void AddSubmodule(StyleModule module) {
        //     // if(module is Anchor) 
		//  	//     AddAnchor((Anchor)module);
        //     // else if (module is Variable) 
        //     //     AddVariable((Variable)module);
		// }


    }
}

