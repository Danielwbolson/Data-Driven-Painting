
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SculptingVis;
using System.IO;



namespace SculptingVis.SmartData {
    public class Anchor : SmartDataObject {
        public Anchor(SmartData.Dataset dataset) {
            _dataset = dataset;
        }

        public Anchor Init() {
			AddSubmodule((new StyleSocket()).Init("",this,false,true,this));
			// AddSubmodule((new StyleSocket()).Init("Hook for deriving",this,false,true,_variable));

            return this;
        }

        public override int GetNumberOfSubmodules() {
			return base.GetNumberOfSubmodules() +  GetDataStrategy().GetNumberOfSubmodules();
		}

		public override StyleModule GetSubmodule(int i) {
            if(i < base.GetNumberOfSubmodules()) {
                return base.GetSubmodule(i);
            } else 
                return GetDataStrategy().GetSubmodule(i-base.GetNumberOfSubmodules());
		}



        SmartData.DataStrategy _dataStrategy;

        public void SetDataStrategy(SmartData.DataStrategy strategy) {
            GetDatastream().SetStrategy(strategy);
        }
        public SmartData.DataStrategy GetDataStrategy() {
            return GetDatastream().GetStrategy();
        }
        public SmartData.Datastream GetDatastream() {
            return _dataset.GetDatastream(this);
        }

        public virtual int GetNumberOfCells() {
            return GetDatastream().GetNumberOfCells();
        }

        public virtual float GetWeightForCell(int cellID) {
            return GetDatastream().GetWeightForCell(cellID);;
        }
        public virtual int GetNumberOfPointsForCell(int cellID) {
            return GetDatastream().GetNumberOfPointsForCell(cellID);    
        }


        public virtual int GetPointInCell(int cellPointsIndex, int cellID) {
            return GetDatastream().GetPointInCell(cellPointsIndex,cellID);
        }

        public virtual int GetNumberOfPoints() {
            return GetDatastream().GetNumberOfPoints();
        }

        public virtual Vector3 GetPoint(int pointID) {
            return GetDatastream().GetPointData(pointID);
        }



        // Domain Dimensions refers to the dimensionality of
        // the topography of the anchor. Examples:
        // - Points: 0D
        // - Paths: 1D
        // - PolygonMesh: 2D
        // - PolyhedraMesh: 3D
        int _domainDimensions;

        public int GetDomainDimenionality() {
            return _domainDimensions;
        }

        SmartData.Dataset _dataset;
        public void SetDataset(SmartData.Dataset dataset) {
            _dataset = dataset;
        }
        public void TriggerUpdate() {
            Update();
            if(_dataset != null) _dataset.TriggerUpdateAnchor(this);
        }

        

    }

}