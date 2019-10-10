
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SculptingVis;
using System.IO;



namespace SculptingVis.SmartData {
    public class SimplePointAnchor : Anchor {
        public SimplePointAnchor(SmartData.Dataset dataset) : base(dataset){}

        int _numPoints;
        public void SetNumberOfPoints(int n) {
            _numPoints = n;
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

        public override Vector3 GetPoint(int pointID) {
            return GetPoints()[pointID];
        }

		public override void Update() {
			base.Update();
            GetPoints().Clear();
            for(int i = 0; i < _numPoints; i++)
                GetPoints().Add(new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f)));
		}

        public override string ToString() {
            string s = base.ToString() + " (";
            for(int i = 0; i < GetPoints().Count; i++) {
                s += " " + GetPoints()[i] + " ";

            }
            return s + ")";
        } 


        

    }

}