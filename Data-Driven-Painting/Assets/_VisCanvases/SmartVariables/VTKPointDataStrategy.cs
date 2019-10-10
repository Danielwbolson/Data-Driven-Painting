
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SculptingVis;
using System.IO;



namespace SculptingVis.SmartData {
    public class VTKPointDataStrategy : DataStrategy {
       
        VTKFileAsset _datasetFile;
        public void SetDatasetFile(VTKFileAsset datasetFile) {
            _datasetFile = datasetFile;
        }   

        public override int GetNumberOfCells() {
            return (int)_datasetFile.GetDataset().GetNumberOfCells();
            
        }

        public override float GetWeightForCell(int cellID) {
            return 1.0f;
        }
        public override int GetNumberOfPointsForCell(int cellID) {
            return (int)_datasetFile.GetDataset().GetCell(cellID).GetNumberOfPoints();    
        }


        public override int GetPointInCell(int cellPointsIndex, int cellID) {
            return (int)_datasetFile.GetDataset().GetCell(cellID).GetPointId(cellPointsIndex);
        }

        public override int GetNumberOfPoints() {
            return (int)_datasetFile.GetDataset().GetNumberOfPoints();
        }

        public override Vector3 GetPointData(int pointID) {
            return _datasetFile.GetDataset().GetPoint(pointID);
        }

        public override Vector3 GetCellData(int pointID) {
            return Vector3.zero;
        }
        

		public override void Update() {
			base.Update();
		}

    }

}