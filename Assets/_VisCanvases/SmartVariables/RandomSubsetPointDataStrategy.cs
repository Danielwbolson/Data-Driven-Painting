
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SculptingVis;
using System.IO;



namespace SculptingVis.SmartData {
    public class RandomSubsetPointDataStrategy : DataStrategy {
        SmartData.Anchor _sourceAnchor;
        public void SetSourceAnchor(SmartData.Anchor sourceAnchor) {
            _sourceAnchor = sourceAnchor;
        }

        int _numPoints;

        StyleTypeSocket<Range<int> > _seedInput;
        StyleTypeSocket<Range<int> > _pointCountInput;

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


         public RandomSubsetPointDataStrategy Init(SmartData.Anchor sourceAnchor)
        {
            
            SetSourceAnchor(sourceAnchor);
          
			_seedInput = (new StyleTypeSocket<Range<int>>()).Init("Random Seed",this);
            _seedInput.SetDefaultInputObject(new Range<int>(0,10));
			AddSubmodule(_seedInput);

            _pointCountInput = (new StyleTypeSocket<Range<int>>()).Init("Number of Points",this);
            _pointCountInput.SetDefaultInputObject(new Range<int>(0,_sourceAnchor.GetNumberOfPoints()));
            AddSubmodule(_pointCountInput);

            return this;

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
            Random.InitState( (Range<int>)(_seedInput.GetInput()));
            for(int i = 0; i < (Range<int>)(_pointCountInput.GetInput()); i++) {
                int sourcePointID = Random.RandomRange(0,_sourceAnchor.GetNumberOfPoints());
                GetPoints().Add(new Vector3(_sourceAnchor.GetPoint(sourcePointID).x,_sourceAnchor.GetPoint(sourcePointID).y,_sourceAnchor.GetPoint(sourcePointID).z));
            }
		}

    }

}