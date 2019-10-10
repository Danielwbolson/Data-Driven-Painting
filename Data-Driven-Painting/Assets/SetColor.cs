using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<MeshRenderer>().material.SetColor("_Color",((SculptingVis.StyleController)FindObjectOfType(typeof(SculptingVis.StyleController))).backdropColor );
	}
}
