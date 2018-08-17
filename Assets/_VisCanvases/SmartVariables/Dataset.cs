
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SculptingVis;
using System.IO;


namespace SculptingVis.SmartData {

    [System.Serializable]
    public class Dataset : SmartDataObject {
        
        [SerializeField]
        List<SmartData.Anchor> _anchors; 

        [SerializeField]
        List<SmartData.AnchoredVariable> _anchoredvariables; 

        [SerializeField]
        List<SmartData.ContinuousVariable> _continuousVariables;

        [SerializeField]
        SmartData.Anchor _sourceAnchor;

        Dictionary<SmartData.Anchor, Dictionary<SmartData.Variable,SmartData.Datastream> > _anchoredDatastreams;
        Dictionary<SmartData.Variable,SmartData.Datastream> _continuousDatastreams;
        Dictionary<SmartData.Anchor,SmartData.Datastream> _anchorStreams;



        public void SetSourceAnchor(SmartData.Anchor sourceAnchor) {
            _sourceAnchor = sourceAnchor;

        }
        
        public SmartData.Anchor GetSourceAnchor() {
            return _sourceAnchor;
        }
        public List<SmartData.Anchor> GetAnchors() {
            if(_anchors == null) _anchors = new List<SmartData.Anchor>();
            return _anchors;
        }
        
        public List<SmartData.AnchoredVariable> GetAnchoredVariables() {
            if(_anchoredvariables == null) _anchoredvariables = new List<AnchoredVariable>();
            return _anchoredvariables;
        }

        public List<SmartData.ContinuousVariable> GetContinuousVariables() {
            if(_continuousVariables == null) _continuousVariables = new List<SmartData.ContinuousVariable>();
            return _continuousVariables;
        }

        public void AddVariable(SmartData.Variable newVariable) {
            if(newVariable is SmartData.ContinuousVariable)
                AddVariable((SmartData.ContinuousVariable)newVariable);
            else if(newVariable is SmartData.AnchoredVariable)
                AddVariable((SmartData.AnchoredVariable)newVariable);
            

        }
        public void AddVariable(SmartData.ContinuousVariable newVariable) {
            if(GetContinuousVariables().Contains(newVariable)) {
                Debug.Log("That Variable already exists in this dataset.");
                return;
            }
            GetContinuousVariables().Add(newVariable);
            
            Dictionary<SmartData.Variable,SmartData.Datastream> streams = 
                GetContinuousDatastreams();

            Debug.Log("Populating variable " + newVariable );
            streams[newVariable] = generateDatastream(newVariable);

        

        }

        public void AddVariable(SmartData.AnchoredVariable newVariable) {
            if(GetAnchoredVariables().Contains(newVariable)) {
                Debug.Log("That Variable already exists in this dataset.");
                return;
            }
            GetAnchoredVariables().Add(newVariable);
        }
        
        public void AddAnchor(SmartData.Anchor newAnchor) {
            if(GetAnchors().Contains(newAnchor)) {
                Debug.Log("That anchor already exists in this dataset.");
                return;
            }
            GetAnchors().Add(newAnchor);
            populateVariablesForAnchor(newAnchor);
        }


        

        protected Dictionary<SmartData.Anchor, Dictionary<SmartData.Variable,SmartData.Datastream> > GetAnchoredDatastreams() {
            if(_anchoredDatastreams == null)
                _anchoredDatastreams = new Dictionary<Anchor, Dictionary<Variable, Datastream>>();
            return _anchoredDatastreams;
        }

        protected Dictionary<SmartData.Variable,SmartData.Datastream> GetContinuousDatastreams() {
            if(_continuousDatastreams == null)
                _continuousDatastreams = new Dictionary<SmartData.Variable,SmartData.Datastream>();
            return _continuousDatastreams;
        }

        protected Dictionary<SmartData.Anchor,SmartData.Datastream> GetAnchorDatastreams() {
            if(_anchorStreams == null)
                _anchorStreams = new Dictionary<SmartData.Anchor,SmartData.Datastream>();
            return _anchorStreams;
        }

        protected Dictionary<SmartData.Variable,SmartData.Datastream> GetAnchoredVariableStreams(Anchor anchor) {
            if(!GetAnchoredDatastreams().ContainsKey(anchor)) {
                populateVariablesForAnchor(anchor);
            }
            return GetAnchoredDatastreams()[anchor];
        }
         protected virtual Datastream generateDatastream(SmartData.Variable variable,SmartData.Anchor anchor = null) {
            if(variable is SmartData.AnchoredVariable && anchor != null)
                generateDatastream((SmartData.AnchoredVariable) variable,anchor ); 
            if(variable is SmartData.ContinuousVariable && anchor == null) 
                generateDatastream((SmartData.ContinuousVariable) variable ); 

            return null;
         }
        protected virtual Datastream generateDatastream(SmartData.AnchoredVariable variable, SmartData.Anchor anchor) {
            Debug.Log("Generating datastream for variable " + variable + " and anchor " + anchor );
            SmartData.Datastream ds = new Datastream(variable, anchor);
            ds.SetName("Stream for " + variable.GetName() + " -- " + anchor.GetName());
            return ds;

        } 

        protected virtual Datastream generateDatastream(SmartData.ContinuousVariable variable) {
            Debug.Log("Generating datastream for variable " + variable );
            SmartData.Datastream ds = new Datastream(variable, null);
            ds.SetName("Stream for " + variable.GetName() );
            return ds;

        } 
        protected void populateVariablesForAnchor(SmartData.Anchor anchor) {
            Dictionary<SmartData.Variable,SmartData.Datastream> streams = 
                GetAnchoredDatastreams()[anchor] = new Dictionary<Variable, Datastream>();

            for(int i = 0; i < GetAnchoredVariables().Count; i++) {
                Debug.Log("Populating variable " + GetAnchoredVariables()[i] + " for anchor " + anchor );
                streams[GetAnchoredVariables()[i]] = generateDatastream(GetAnchoredVariables()[i],anchor);

            }
        }



        protected void triggerUpdateVariablesForAnchor(SmartData.Anchor anchor) {
            Dictionary<SmartData.Variable,SmartData.Datastream> streams = 
                GetAnchoredDatastreams()[anchor];

             for(int i = 0; i < GetAnchoredVariables().Count; i++) {
                Debug.Log("Updating variable " + GetAnchoredVariables()[i] + " for anchor " + anchor );
                streams[GetAnchoredVariables()[i]].TriggerUpdate();

            }
        }


        public SmartData.Datastream GetDatastream(SmartData.Variable variable,SmartData.Anchor anchor = null) {
            if(anchor == null && variable is ContinuousVariable) {
                return GetDatastream((ContinuousVariable)variable);
            }
            if(anchor != null && variable is AnchoredVariable) {
                return GetDatastream((AnchoredVariable)variable, anchor);
            }

            return null;
        }


        public SmartData.Datastream GetDatastream(SmartData.AnchoredVariable variable,SmartData.Anchor anchor) {
            if(!GetAnchoredVariableStreams(anchor).ContainsKey(variable)) {
                populateVariablesForAnchor(anchor);
                 if(!GetAnchoredVariableStreams(anchor).ContainsKey(variable)) {
                    Debug.Log("Couldn't find the variable " + variable + " for anchor " + anchor);
                     return null;
                 }
            }
            return GetAnchoredVariableStreams(anchor)[variable];
        }

        public SmartData.Datastream GetDatastream(SmartData.ContinuousVariable variable) {
            if(!GetContinuousDatastreams().ContainsKey(variable)) {
                Debug.Log("Couldn't find a datastream for the continuous variable " + variable);
                return null;
            } else {
                return GetContinuousDatastreams()[variable];
            }
        }

        public SmartData.Datastream GetDatastream(SmartData.Anchor anchor) {
            if(!GetAnchorDatastreams().ContainsKey(anchor)) {
                Debug.Log("Couldn't find a datastream for the anchor " + anchor + ". Creating a new one.");
                return GetAnchorDatastreams()[anchor] = new Datastream(null, anchor);
            } else {
                return GetAnchorDatastreams()[anchor];
            }
        }

        public void TriggerUpdateVariable(SmartData.Variable variable) {
            Debug.Log("Need to update variable " + variable);
        }

        public void TriggerUpdateAnchor(SmartData.Anchor anchor) {
            Debug.Log("Need to update anchor " + anchor);
            triggerUpdateVariablesForAnchor(anchor);
        } 

        public void Update() {
            float dt = Time.deltaTime;
            foreach(var anchorStream in GetAnchorDatastreams().Values) {
                anchorStream.Update();
            }
            foreach(var anchoredVarStreams in GetAnchoredDatastreams().Values) {
                foreach(var anchoredVarStream in anchoredVarStreams.Values) {
                    anchoredVarStream.Update();
                }
            }
            foreach(var continuousVarStream in GetContinuousDatastreams().Values) {
                continuousVarStream.Update();
            }
        }

    }   
}