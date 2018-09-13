using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis
{
    public class StyleCustomDataset : StyleDataset
    {

        public StyleCustomDataset Init()
        {
            return this;
        }


        public override string GetLabel()
        {
            return "Custom Dataset";
        }

        public virtual void ComputeDataset() {
            
        }



      
    }
}

