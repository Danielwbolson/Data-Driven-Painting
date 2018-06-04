﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderStrategy {

    protected GameObject _parent;
    protected GameObject _obj;
    protected Material _objMat;
    protected Material[] _objMatArray;
    protected Mesh[] _objMeshArray;
    protected List<Vector3> _objPositions;
    protected int TOTALOBJECTS;

    /*
     * CLASS DOCUMENTATION: RenderStrategy
     * This is a parent class utilizing the Strategy pattern for different rendering techniques.
     * This class stores relevent information that any of its strategies could use, while also
     * allowing them to share, and send data further up where is is more accessibly by the user
     */
    public RenderStrategy(GameObject p, GameObject o, Material mat, List<Vector3> poses, int total) {
        _parent = p;
        _obj = o;
        _objMat = mat;
        _objMatArray = new Material[4];
        for (int i = 0; i < 4; i++) {
            _objMatArray[i] = new Material(_objMat);
        }
        _objPositions = poses;
        TOTALOBJECTS = total;

        MeshFilter[] tempArray = _obj.GetComponentsInChildren<MeshFilter>();
        _objMeshArray = new Mesh[tempArray.Length];
        for (int i = 0; i < tempArray.Length; i++) {
            _objMeshArray[i] = tempArray[i].sharedMesh;
        }
    }

    public virtual void UpdateObjects() { }

    public virtual void Destroy() { }

    public List<Vector3> GetPositions() {
        return _objPositions;
    }

    public void SetPositions(List<Vector3> objposes) {
        _objPositions = objposes;
    }

    public void SetNumObjects(int num) {
        TOTALOBJECTS = num;
    }
}