using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    public class StyleModifier : StyleModule
    {
        float map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s-a1)*(b2-b1)/(a2-a1);
        }
        [SerializeField]
        public StyleTypeSocket<Range<bool>> _useVariable;

        [SerializeField]
        public VariableSocket _variable;


        [SerializeField]
        public StyleTypeSocket<Objectify<float>> _variableDataMin;
        
        [SerializeField]
        public StyleTypeSocket<Objectify<float>> _variableDataMax;
        
        [SerializeField]
        public StyleTypeSocket<MinMax<float>> _variableRange;

        
        public override void UpdateModule(string updatedSocket = null) {
            Variable v = null;
            v = ((Variable)_variable.GetInput());
            if(updatedSocket!= null) {

            
            if(updatedSocket  == _variableDataMax.GetUniqueIdentifier() ) {
                    

                if(_variable.GetInput() != null)  {
                    ((Objectify<float>)_variableDataMax.GetInput()).value = Mathf.Min(((Objectify<float>)(_variableDataMax.GetInput())).value,v.GetMax().x);

                    ((MinMax<float>)_variableRange.GetInput()).upperValue = map(((Objectify<float>)_variableDataMax.GetInput()).value,v.GetMin().x,v.GetMax().x,0,1);
                }
            }
            if(updatedSocket == _variableDataMin.GetUniqueIdentifier()) {
                    
                if(_variable.GetInput() != null)  {
                    float newVal = ((Objectify<float>)(_variableDataMin.GetInput())).value;
                    float minVal = v.GetMin().x;
                    float best = Mathf.Max(newVal,minVal);
                    ((Objectify<float>)_variableDataMin.GetInput()).value = best;


                    float newnormalized = map(((Objectify<float>)_variableDataMin.GetInput()).value,v.GetMin().x,v.GetMax().x,0,1);

                    ((MinMax<float>)_variableRange.GetInput()).lowerValue = map(((Objectify<float>)_variableDataMin.GetInput()).value,v.GetMin().x,v.GetMax().x,0,1);
                }
            }
            if(updatedSocket  == _variableRange.GetUniqueIdentifier() ) {
                    
                if(_variable.GetInput() != null)  {

                    ((Objectify<float>)_variableDataMin.GetInput()).value = map(((MinMax<float>)_variableRange.GetInput()).lowerValue,0,1,v.GetMin().x,v.GetMax().x);
                    ((Objectify<float>)_variableDataMax.GetInput()).value = map(((MinMax<float>)_variableRange.GetInput()).upperValue,0,1,v.GetMin().x,v.GetMax().x);
                }
            }
            if(updatedSocket  == _variable.GetUniqueIdentifier() ) {
                if(_variable.GetInput() != null)  {
                    ((Objectify<float>)_variableDataMin.GetInput()).value = map(((MinMax<float>)_variableRange.GetInput()).lowerValue,0,1,v.GetMin().x,v.GetMax().x);
                    ((Objectify<float>)_variableDataMax.GetInput()).value = map(((MinMax<float>)_variableRange.GetInput()).upperValue,0,1,v.GetMin().x,v.GetMax().x);
                }
            }
            }
            base.UpdateModule(updatedSocket);

        }
        public StyleModifier Init(VariableSocket _anchorVariableSocket, StyleModule module, int slot = -1 )
        {

            _useVariable = (new StyleTypeSocket<Range<bool>>()).Init("Use Variable", this);
            _useVariable.SetDefaultInputObject(new Range< bool>(false,true,false));
            AddSubmodule(_useVariable);

            _variable = new VariableSocket();
            _variable.Init("Variable",this,slot);
            _variable.SetAnchorVariableSocket(_anchorVariableSocket);
			_variable.RequireScalar();
            _variable.HideIfFalse(_useVariable);
            AddSubmodule(_variable);

            _variableRange = new StyleTypeSocket<MinMax<float>>();
            _variableRange.Init("Data Range",this);
            _variableRange.SetDefaultInputObject(new MinMax<float>(0,1));
            _variableRange.HideIfFalse(_useVariable);

            AddSubmodule(_variableRange);

            _variableDataMin = new StyleTypeSocket<Objectify<float>>();
            _variableDataMin.Init("Data min",this);
            _variableDataMin.SetDefaultInputObject(new Objectify<float>(0));
            _variableDataMin.HideIfFalse(_useVariable);

            AddSubmodule(_variableDataMin);

            _variableDataMax = new StyleTypeSocket<Objectify<float>>();
            _variableDataMax.Init("Data max",this);
            _variableDataMax.SetDefaultInputObject(new Objectify<float>(1));
            _variableDataMax.HideIfFalse(_useVariable);

            AddSubmodule(_variableDataMax);

            return this;
        }
    }
}

        