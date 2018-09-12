using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    public class StyleColorModifier : StyleModifier
    {
        public override string GetLabel()
        {
         
            return "Color Modifier";
        }


        public StyleTypeSocket<Objectify<Color>> _colorSocket; 
        public StyleTypeSocket<Colormap> _colormapSocket; 
        public StyleTypeSocket<Range<bool>> _flipColormapSocket;


        public StyleModifier Init(VariableSocket anchorSocket )
        {
            base.Init(anchorSocket);
            _colorSocket = (new StyleTypeSocket<Objectify<Color>>()).Init("Constant Color", this);
            _colorSocket.SetDefaultInputObject(new Objectify< Color>(Color.white));
            _colorSocket.HideIfTrue(_useVariable);

            AddSubmodule(_colorSocket);


            _colormapSocket = (new StyleTypeSocket<Colormap>()).Init("Colormap",this);
            _colormapSocket.SetDefaultInputObject(Colormap.DefaultColormap());
            _colormapSocket.HideIfFalse(_useVariable);
            AddSubmodule(_colormapSocket);

            _flipColormapSocket = (new StyleTypeSocket<Range<bool>>()).Init("Flip Colormap", this);
            _flipColormapSocket.SetDefaultInputObject(new Range< bool>(false,true,false));
            _flipColormapSocket.HideIfFalse(_useVariable);
            AddSubmodule(_flipColormapSocket);


            return this;

        }


    }

}

        