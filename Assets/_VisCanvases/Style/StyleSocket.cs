﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis {

	
	[System.Serializable]
	public class StyleSocket : StyleModule {

		public override string GetLabel() {
			return _label;	
		}
		public override string ToString() {
			return GetLabel();
		}
		[SerializeField]
		protected string _label;

		[SerializeField]
		StyleModule _module;
		[SerializeField]
		bool _isInput;
		[SerializeField]
		bool _isOutput;
		
		[SerializeField]
		Object _source;

		[SerializeField]
		protected Object _input;

		[SerializeField]
		protected Object _defaultInput;



		[SerializeField]
		List<StyleLink> _links;


		public List<StyleLink> GetLinks() {
			if(_links == null) _links = new List<StyleLink>();
			return _links;
		}
		public StyleSocket Init(string label, StyleModule module, bool isInput, bool isOutput, Object sourceObject = null) {
			this.InstanceID = System.Guid.NewGuid();
			_module = module;
			_isInput = isInput;
			_isOutput = isOutput;
			_label = label;
			SetSourceObject(sourceObject);
			return this;
		}



		public StyleModule GetModule() {
			return _module;
		}
		public void SetSourceObject(Object sourceObject) {
			_source = sourceObject;
		}

		public virtual void SetInputObject(Object inputObject) {
			_input = inputObject;
		}

		public virtual void SetDefaultInputObject(Object defaultInputObject) {
			_defaultInput = defaultInputObject;
		}
		public Object GetOutput() {
			return _source;
		}
		public virtual Object GetInput() {
			return _input!=null? _input : _defaultInput;
		}

		public bool IsOutput() {
			return _isOutput;
		}

		public bool IsInput() {
			return _isInput;
		}
		
		public virtual bool DoesAccept(StyleSocket incoming) {
			return true;
		}

		public virtual void ClearInput() {
			_input = null;
		}
		public virtual void UpdateSocket(){
			if(IsInput()) {
				GetModule().UpdateModule();
			}
		}
	}
}

