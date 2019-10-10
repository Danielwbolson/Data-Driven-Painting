using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteCurve : MonoBehaviour {

    private Streamline _streamLine;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Streamline MakeCurve() {
        _streamLine = new Streamline();
        return _streamLine;
    }
}
