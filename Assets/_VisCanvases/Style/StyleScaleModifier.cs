using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    public class StyleScaleModifier : StyleModifier
    {
        public override string GetLabel()
        {
         
            return "Scale Modifier";
        }

       public override string GetTypeTag() {
            return "SCALE_MODIFIER";
        }
        public StyleTypeSocket<Range<float>> _scaleSocket; 
        public StyleTypeSocket<MinMax<float>> _scaleRangeSocket;

        public StyleTypeSocket<Range<bool>> _flipScaleSocket; 




        public StyleModifier Init(VariableSocket anchorSocket, StyleModule module, int slot = -1 )
        {
            base.Init(anchorSocket, module, slot);
            _scaleSocket = (new StyleTypeSocket<Range<float>>()).Init("Constant Scale", this);
            _scaleSocket.SetDefaultInputObject(new Range<float>(0,1,1));
            _scaleSocket.HideIfTrue(_useVariable);

            AddSubmodule(_scaleSocket);


            _scaleRangeSocket = (new StyleTypeSocket<MinMax<float>>()).Init("Scale Range",this);
            _scaleRangeSocket.SetDefaultInputObject(new MinMax<float>(0,0.1f));
            _scaleRangeSocket.HideIfFalse(_useVariable);
            AddSubmodule(_scaleRangeSocket);

            _flipScaleSocket = (new StyleTypeSocket<Range<bool>>()).Init("Flip Scale Range", this);
            _flipScaleSocket.SetDefaultInputObject(new Range< bool>(false,true,false));
            _flipScaleSocket.HideIfFalse(_useVariable);
            AddSubmodule(_flipScaleSocket);


            return this;

        }


    }

}

        