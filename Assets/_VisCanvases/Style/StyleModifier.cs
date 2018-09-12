using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    public class StyleModifier : StyleModule
    {

        [SerializeField]
        public StyleTypeSocket<Range<bool>> _useVariable;

        [SerializeField]
        public VariableSocket _variable;

        [SerializeField]
        public StyleTypeSocket<MinMax<float>> _variableRange;

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



            return this;
        }
    }
}

        