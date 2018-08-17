
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SculptingVis;
using System.IO;



namespace SculptingVis.SmartData {
    public abstract class DataStrategy : StyleModule{
        public virtual void Update() {

        }

        public abstract int GetNumberOfCells();

        public abstract float GetWeightForCell(int cellID);
        public abstract int GetNumberOfPointsForCell(int cellID);


        public abstract int GetPointInCell(int cellPointsIndex, int cellID);

        public abstract int GetNumberOfPoints();


        public abstract Vector3 GetCellData(int pointID);

        public abstract Vector3 GetPointData(int pointID);

    }

}