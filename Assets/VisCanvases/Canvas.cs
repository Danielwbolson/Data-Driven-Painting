﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace SculptingVis {


[ExecuteInEditMode]
public class Canvas : MonoBehaviour {
	[SerializeField]
	Bounds _bounds;

	[SerializeField]
	Mesh _areaMesh;
	[SerializeField]
	Material _areaMaterial;

	[SerializeField]
	Mesh _cornerMesh;
	[SerializeField]
	Material _cornerMaterial;


	[SerializeField]
	Matrix4x4 _innerSceneTransform;

	[SerializeField]
	Vector3 _innerSceneOrigin;

	[SerializeField]
	Vector3 _innerSceneScale;


	[SerializeField]
	Style _style;


	[SerializeField]
	bool _fitStyle;

	[SerializeField]
	bool _selected = false;

	[SerializeField]
	public Vector4 _color;

	[SerializeField]
	float _extentThreshold;
	[SerializeField]
	float _boundsThreshold = 0.02f;
	// Use this for initialization
	void Start () {
		
	}
	
	public void SetMaterialProperties(Material canvasMaterial) {
		canvasMaterial.SetColor("_Color", _color);
		canvasMaterial.SetMatrix("_InverseCanvas", transform.worldToLocalMatrix);
		canvasMaterial.SetVector("_CanvasBoundsCenter",_bounds.center);
		canvasMaterial.SetVector("_CanvasBoundsExtent",_bounds.extents);
		canvasMaterial.SetVector("_CanvasBoundsExtentThreshold",new Vector3(_extentThreshold,_extentThreshold,_extentThreshold));
		canvasMaterial.SetVector("_CanvasBoundsThreshold",new Vector3(_boundsThreshold,_boundsThreshold,_boundsThreshold));
		canvasMaterial.SetColor("_CropColor", new Vector4(0,0,0,0.5f));
	} 
	void updateBounds() {
		BoxCollider boxCollider = GetComponent<BoxCollider>();
		boxCollider.center =_bounds.center;
		boxCollider.size = _bounds.size;
	}

	public Matrix4x4 GetBoundsTransformMatrix() {
		Matrix4x4 boundsTransform = Matrix4x4.TRS(_bounds.center, Quaternion.identity, _bounds.size);
		return transform.localToWorldMatrix*boundsTransform;
	}

	public Matrix4x4 GetInnerSceneTransformMatrix() {
		return transform.localToWorldMatrix*_innerSceneTransform;

	}

	public void AddGameObject(GameObject gameobject) {
		gameobject.transform.SetParent(transform.Find("InnerSceneOrigin"),false);

	}
	// Update is called once per frame
	void Update () {
		if(_fitStyle && _style.HasBounds()) {
			Vector3 innerScaleDims = _style.GetBounds().size;
			Vector3 canvasDims = _bounds.size - new Vector3(_boundsThreshold*2 + 0.001f,_boundsThreshold*2+ 0.001f,_boundsThreshold*2+ 0.001f);

			Vector3 ratio = Vector3.Scale(innerScaleDims, canvasDims.reciprocal());
			float maxRatioDim = ratio.maxDimension();


			// _innerSceneTransform.SetTRS(_style.GetBounds().center, Quaternion.identity, new Vector3(maxRatioDim,maxRatioDim,maxRatioDim));
			_innerSceneTransform = Matrix4x4.Scale(new Vector3(1.0f/maxRatioDim,1.0f/maxRatioDim,1.0f/maxRatioDim))*Matrix4x4.Translate(-_style.GetBounds().center);
			//_innerSceneTransform = _innerSceneTransform.inverse;
		} else {
			_innerSceneTransform.SetTRS(_innerSceneOrigin, Quaternion.identity, _innerSceneScale);
		}

		Transform innerSceneOrigin = transform.Find("InnerSceneOrigin");
		innerSceneOrigin.localRotation = Quaternion.identity;
		innerSceneOrigin.localPosition = _innerSceneTransform.GetPosition();
		innerSceneOrigin.localScale = _innerSceneTransform.GetScale();

		Graphics.DrawMesh(_areaMesh,GetBoundsTransformMatrix(),_areaMaterial,0);

		Matrix4x4[] cornerTransforms = new Matrix4x4[8];
		for(int i = 0; i < 8; i++) {
			Vector3 extentModifier = new Vector3(i/4==0?1:-1,i%2==0?1:-1, (i%4)/2==0?1:-1);
			cornerTransforms[i] = Matrix4x4.TRS(_bounds.center + Vector3.Scale(_bounds.extents,extentModifier ),Quaternion.identity, new Vector3(0.1f,0.1f,0.1f));
			Graphics.DrawMesh(_cornerMesh,transform.localToWorldMatrix*cornerTransforms[i],_cornerMaterial,0);
		}

		if(_style) {
			_style.ApplyStyle(this);
		}

		
	}
}
}