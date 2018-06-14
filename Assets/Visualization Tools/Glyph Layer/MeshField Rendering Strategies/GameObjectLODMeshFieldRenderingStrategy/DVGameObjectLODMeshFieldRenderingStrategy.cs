﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DV
{
public class DVGameObjectLODMeshFieldRenderingStrategy : DVMeshFieldRenderingStrategy {

    public GameObject _GlyphPrefab;
    List<GameObject> _glyphs;

	// Use this for initialization
	public override void UpdateMeshData() {
		  if(_glyphs == null) _glyphs = new  List<GameObject>();
                foreach(var glyph in _glyphs) {
                    Destroy(glyph);
            }
            _glyphs.Clear();

            for(int i =0; i < _samples.Length; i++) {

                int meshIndex = Random.Range(0,_meshes.Length);
                GameObject meshContainer = _meshes[meshIndex];

                GameObject glyph = Instantiate(meshContainer);
                _glyphs.Add(glyph);

                foreach(var m in glyph.GetComponentsInChildren<MeshRenderer>()) {
                    m.material = _material;
                }
                glyph.transform.SetParent(_parent,false);
                glyph.transform.localScale = new Vector3(1,1,1)*1.0f/glyph.GetComponentInChildren<MeshFilter>().mesh.bounds.size.y*4;
                glyph.transform.localPosition = _samples[i].position;
            }
	}
	public override void DrawMeshes() {
		
	}
    
}
}