
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SculptingVis;
using System.IO;



namespace SculptingVis.SmartData {
    public class Variable : SmartDataObject {
        public Variable(SmartData.Dataset dataset) {
            _dataset = dataset;
        }
        public int components;
        
        SmartData.Dataset _dataset;

        public void SetDataset(SmartData.Dataset dataset) {
            _dataset = dataset;
        }

        public SmartData.Dataset GetDataset() {
            return _dataset;
        }

        public void TriggerUpdate() {
            if(GetDataset() != null) GetDataset().TriggerUpdateVariable(this);
        }

        


    }

}