using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    public class StyleAspectRatioModifier : StyleModifier
    {
        public override string GetLabel()
        {
         
            return "Aspect Ratio Modifier";
        }

       public override string GetTypeTag() {
            return "ASPECT_MODIFIER";
        }
        public StyleTypeSocket<Range<float>> _aspectRatioSocket; 
        public StyleTypeSocket<MinMax<float>> _aspectRatioRangeSocket;
        

        public StyleTypeSocket<Range<bool>> _flipAspectRatioRangeSocket; 

        public StyleTypeSocket<Range<bool>> _contrainRadiallySocket; 



        public StyleModifier Init(VariableSocket anchorSocket, StyleModule module, int slot = -1 )
        {
            base.Init(anchorSocket, module, slot);
            _aspectRatioSocket = (new StyleTypeSocket<Range<float>>()).Init("Constant Aspect Ratio", this);
            _aspectRatioSocket.SetDefaultInputObject(new Range<float>(0.1f,2,1));
            _aspectRatioSocket.HideIfTrue(_useVariable);

            AddSubmodule(_aspectRatioSocket);


            _aspectRatioRangeSocket = (new StyleTypeSocket<MinMax<float>>()).Init("Aspect Ratio Range",this);
            _aspectRatioRangeSocket.SetDefaultInputObject(new MinMax<float>(0.1f,2));
            _aspectRatioRangeSocket.HideIfFalse(_useVariable);
            AddSubmodule(_aspectRatioRangeSocket);

            _flipAspectRatioRangeSocket = (new StyleTypeSocket<Range<bool>>()).Init("Flip Aspect Range", this);
            _flipAspectRatioRangeSocket.SetDefaultInputObject(new Range< bool>(false,true,false));
            _flipAspectRatioRangeSocket.HideIfFalse(_useVariable);
            AddSubmodule(_flipAspectRatioRangeSocket);

            _contrainRadiallySocket = (new StyleTypeSocket<Range<bool>>()).Init("Constrain Radially", this);
            _contrainRadiallySocket.SetDefaultInputObject(new Range< bool>(false,true,false));
            AddSubmodule(_contrainRadiallySocket);

            return this;

        }


    }

}

        