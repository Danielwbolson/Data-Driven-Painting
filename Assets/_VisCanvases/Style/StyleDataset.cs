using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis{
	public class StyleDataset : StyleModule {
		[SerializeField]
		protected SmartData.Dataset _dataset;

        public void SetDataset(SmartData.Dataset dataset) {
            _dataset = dataset;
        }

        public SmartData.Dataset GetDataset() {
            return _dataset;
        }
	    public override string GetLabel() {
			return _dataset + "";
		}

    }
}

