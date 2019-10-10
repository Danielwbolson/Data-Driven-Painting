﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DEPRECATED{



    [CustomEditor(typeof(LayerManager))]
    public class LayerManagerEditor : Editor
    {


        
        public override void OnInspectorGUI()
        {
            LayerManager layerManager = (LayerManager)target;

            //DrawDefaultInspector();

            GUILayout.Label("Layer Options: " +  layerManager.GetAvailableLayerTypes().Count.ToString());

            int selected = 0;

             string[] options = new string[layerManager.GetAvailableLayerTypes().Count];

             for(int i  = 0; i < layerManager.GetAvailableLayerTypes().Count; i++) {
                 options[i] = layerManager.GetAvailableLayerTypes()[i].GetName();
             }

            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.Label(layerManager.GetLayers().Count.ToString() + " layers.");
            if(GUILayout.Button("+", GUILayout.Width(20))) {
                layerManager.AddLayer(0);

            }
            GUILayout.EndHorizontal();


            for(int i = 0; i < layerManager.GetLayers().Count; i++) {
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                int s = 0;
                foreach (string x in options)
                {
                    if (x.Equals (layerManager.GetLayers()[i].GetLayerTypeName()))
                    {
                        selected = s; 
                    }
                    s++;
                }
                int s2 = selected;
                selected = EditorGUILayout.Popup("Layer Type", selected, options);
                if(selected != s2){
                    Layer l = layerManager.GetLayers()[i];
                    l.Reset();
                    layerManager.GetLayers()[i].SetLayerType(layerManager.GetAvailableLayerTypes()[selected]);
                   //layerManager.RemoveLayer(layerManager.GetLayers()[i]);
    
                } 
                if(GUILayout.Button("-", GUILayout.Width(20))) {                    
                    layerManager.RemoveLayer(layerManager.GetLayers()[i]);
                    break;
                }
                GUILayout.EndHorizontal();
                Layer layer = layerManager.GetLayers()[i];
                layer.RenderGUI();

                //layerManager.GetLayers()[i].RenderGUI();
                GUILayout.EndVertical();
            }
 

            GUILayout.EndVertical();

            // // GUILayout.EndVertical();
            // for(int i = 0; i < layerManager._layerTypePrefabs.Length; i++) {
            //     if (GUILayout.Button("Add " + layerManager._layerTypePrefabs[i].GetName() + " layer")) {
            //         layerManager.AddLayer(i);
            //     }

            // }

        }
    }




[ExecuteInEditMode]
public class LayerManager : MonoBehaviour {
        public Database GetDatabase() {
            return gameObject.GetComponent<Database>();
        }
    [SerializeField]
    private LayerTypeSet _availableLayerTypes;
    public List<LayerType> GetAvailableLayerTypes() {
        return _availableLayerTypes != null? _availableLayerTypes.layerTypes : new List<LayerType>();
    }
    [SerializeField]
	private List<Layer> _layers;


    public List<Layer> GetLayers() {
        if(_layers == null) _layers = new List<Layer>();
        return _layers;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		foreach(var layer in _layers) {
            layer.Update();
        }
	}

    void OnDrawGizmos() {
        foreach(var layer in _layers) {
            layer.DrawGizmos();
        }
    }
    public void RemoveLayer(Layer toRemove ) {
        toRemove.Destroy();
        print("Removing " + toRemove.GetLayerTypeName() + " layer");
        _layers.Remove(toRemove);
        
        
    }
    public Layer NewLayer(int index) {
        if(index >= GetAvailableLayerTypes().Count)
        {
            Debug.LogError("Tried to access index " + index + " of " + GetAvailableLayerTypes().Count + " available layer types.");
            return null;
        }
        // print("Adding new " + GetAvailableLayerTypes()[index].GetName() + " layer");
         //GameObject layer = GetAvailableLayerTypes().layerTypes[index];
        // print(layer);
        Layer layer = new Layer(this);
        layer.SetLayerType( GetAvailableLayerTypes()[index]);
        return layer;
    }
    public void AddLayer(int index) {
        Layer l = NewLayer(index);
        if(l != null)
            GetLayers().Add(l);

    }


}
}