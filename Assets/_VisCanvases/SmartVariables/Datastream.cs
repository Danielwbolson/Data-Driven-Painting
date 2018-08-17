
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SculptingVis;
using System.IO;



namespace SculptingVis.SmartData {
    public class Datastream : SmartDataObject{

        SmartData.DataStrategy _strategy;
        
        
        
        
		public override void Update() {
			if(_strategy != null) {
                _strategy.Update();
            }
		}
        
        public void SetStrategy(SmartData.DataStrategy strategy) {
            _strategy = strategy;
        }
        public SmartData.DataStrategy GetStrategy() {
            return _strategy;
        }


        public virtual int GetNumberOfCells() {
            return GetStrategy().GetNumberOfCells();
        }

        public virtual float GetWeightForCell(int cellID) {
            return GetStrategy().GetWeightForCell(cellID);
        }
        public virtual int GetNumberOfPointsForCell(int cellID) {
            return GetStrategy().GetNumberOfPointsForCell(cellID);    
        }


        public virtual int GetPointInCell(int cellPointsIndex, int cellID) {
            return GetStrategy().GetPointInCell(cellPointsIndex,cellID);
        }

        public virtual int GetNumberOfPoints() {
            return GetStrategy().GetNumberOfPoints();
        }

        public virtual Vector3 GetCellData(int pointID) {
            return GetStrategy().GetCellData(pointID);
        }

        public virtual Vector3 GetPointData(int pointID) {
            return GetStrategy().GetPointData(pointID);;
        }



        public void TriggerUpdate() {


            if(!IsContinuous()) {
                Debug.Log("Updating datastream \"" + this + "\" from dataset " + GetDataset() + " with anchor " + _anchor);
                switch(_anchor.GetDomainDimenionality()) {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2: 
                        break;
                    case 3: 
                        break;
                    default:
                        break;
                    
                }


            }



        }

        public Datastream(SmartData.Variable variable, SmartData.Anchor anchor) {
            _variable = variable;
            _anchor = anchor;
        }


        protected SmartData.Dataset GetDataset() {
            return _variable.GetDataset();
        }

        protected bool IsContinuous() {
            if(_variable != null)
                return _variable is SmartData.ContinuousVariable;
            else
                return false;
        }
        SmartData.Variable _variable;
        SmartData.Anchor _anchor;

    }

}