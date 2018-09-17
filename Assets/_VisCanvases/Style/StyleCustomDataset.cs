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


        public virtual void ComputeDataset() {
            
        }

        int version = 0;
        int computedVersion = 0;
        public bool IsUpToDate() {
            return computedVersion >= version;
        }

        public void SetUpToDate() {
            computedVersion = version;
        }
        public override void UpdateModule(string updatedSocket = null) {
            base.UpdateModule(updatedSocket);
            version++;
        }


      
    }
}

