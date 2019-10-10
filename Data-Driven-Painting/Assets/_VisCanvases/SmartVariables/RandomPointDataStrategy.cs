
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SculptingVis;
using System.IO;



namespace SculptingVis.SmartData {
    public class RandomPointDataStrategy : DataStrategy {
        int _numPoints;
        int _seed = 0;
        public void SetNumberOfPoints(int n) {
            _numPoints = n;
        }
        public void SetSeed(int s) {
            _seed = s;
        }
        List<Vector3> _points;
        List<Vector3> GetPoints() {

            if(_points == null) _points = new List<Vector3>(); 
            return _points;
        }


         public override int GetNumberOfCells() {
            return 1;
        }

        public override float GetWeightForCell(int cellID) {
            return 1;
        }
        public override int GetNumberOfPointsForCell(int cellID) {
            return GetNumberOfPoints();    
        }


        public override int GetPointInCell(int cellPointsIndex, int cellID) {
            return cellPointsIndex;
        }

        public override int GetNumberOfPoints() {
            return GetPoints().Count;
        }

        public override Vector3 GetPointData(int pointID) {
            return GetPoints()[pointID];
        }

        public override Vector3 GetCellData(int pointID) {
            return Vector3.zero;
        }
        

		public override void Update() {
            //Debug.Log("Randomizing!");
			base.Update();
            GetPoints().Clear();
            Random.InitState(_seed);
            for(int i = 0; i < _numPoints; i++)
                GetPoints().Add(new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f)));
		}

    }

}