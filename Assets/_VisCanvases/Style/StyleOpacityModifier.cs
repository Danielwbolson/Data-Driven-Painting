using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    public class StyleOpacityModifier : StyleModifier
    {
        public override string GetLabel()
        {
         
            return "Opacity Modifier";
        }


        public StyleTypeSocket<Range<float>> _opacitySocket; 
        public StyleTypeSocket<Colormap> _opacitymapSocket; 
        public StyleTypeSocket<Range<bool>> _flipOpacityMapSocket;


        public StyleModifier Init(VariableSocket anchorSocket, StyleModule module, int slot = -1 )
        {
            base.Init(anchorSocket, module, slot);
            _opacitySocket = (new StyleTypeSocket<Range<float>>()).Init("Constant Opacity", this);
            _opacitySocket.SetDefaultInputObject(new Range<float>(0,1,1));
            _opacitySocket.HideIfTrue(_useVariable);

            AddSubmodule(_opacitySocket);


            _opacitymapSocket = (new StyleTypeSocket<Colormap>()).Init("OpacityMap",this);
            _opacitymapSocket.SetDefaultInputObject(Colormap.DefaultColormap());
            _opacitymapSocket.HideIfFalse(_useVariable);
            AddSubmodule(_opacitymapSocket);

            _flipOpacityMapSocket = (new StyleTypeSocket<Range<bool>>()).Init("Flip OpacityMap", this);
            _flipOpacityMapSocket.SetDefaultInputObject(new Range< bool>(false,true,false));
            _flipOpacityMapSocket.HideIfFalse(_useVariable);
            AddSubmodule(_flipOpacityMapSocket);


            return this;

        }


    }

}

        