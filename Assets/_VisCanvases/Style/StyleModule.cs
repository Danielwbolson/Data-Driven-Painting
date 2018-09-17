using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SculptingVis{
	public class StyleModule : ScriptableObject{


		public virtual JSONObject GetSerializedJSONObject() { 
			JSONObject json = new JSONObject();
			json.AddField("typetag",GetTypeTag());
			json.AddField("id",GetUniqueIdentifier());
			JSONObject serialized = serialize();
			if(serialized!= null && !serialized.IsNull)
				json.Absorb(serialized);
			JSONObject submoduleList = new JSONObject();
			
			for(int i = 0; i < GetNumberOfSubmodules(); i++) {
				JSONObject submodule = GetSubmodule(i).GetSerializedJSONObject();
				if(submodule!=null)
					submoduleList.Add(submodule);	
			}
			if(GetNumberOfSubmodules() > 0)
				json.AddField("submodules",submoduleList);
			return json;
		}

		public virtual JSONObject serialize() {
			JSONObject json = new JSONObject();
			return json;
		}

		public virtual string GetTypeTag() {
			return "Didn't set a TypeTag for " + GetLabel();
		}
		public void ApplySerialization(JSONObject json) {
			SetInstanceID(json.GetField("id").str);
			applySeralization(json);
			if(json.HasField("submodules")) {
				var submods = json["submodules"].list;
				for(int i = 0; i <GetNumberOfSubmodules(); i++) {
					if(submods[i].GetField("typetag").str != GetSubmodule(i).GetTypeTag()) {
						Debug.LogError("MISMATCH!");
					} else {
						GetSubmodule(i).ApplySerialization(submods[i]);
					}
				}

			}
		}
		public virtual void applySeralization(JSONObject json) {

		}

		static Dictionary<string, StyleModule> _moduleMap;
		public static Dictionary<string, StyleModule> GetModuleMap() {
			if(_moduleMap == null) _moduleMap = new Dictionary<string, StyleModule>();
			return _moduleMap;
		}
		public void SetInstanceID(string id ) {
			if(InstanceID != "")
				GetModuleMap().Remove(InstanceID);
			InstanceID = id;
				GetModuleMap()[InstanceID] = this;
		}
		public string GenerateInstanceID() {
			long ticks = DateTime.Now.Ticks;
			byte[] bytes = BitConverter.GetBytes(ticks);
			string id = Convert.ToBase64String(bytes)
									.Replace('+', '_')
									.Replace('/', '-')
									.TrimEnd('=');
			return id;
		}
		public string InstanceID = "";

		// Other properties, etc.
		public virtual string GetUniqueIdentifier() {
			if(InstanceID == "") SetInstanceID(GenerateInstanceID());
			return InstanceID + "";
		}

		

		StyleSocket _triggerSocket;
		bool triggerSocketAssigned;
		bool _hideCondition;

		public void HideIfTrue(StyleSocket triggerSocket) {
			_triggerSocket = triggerSocket;
			 triggerSocketAssigned = true;
			_hideCondition = true;
		}

		public void HideIfFalse(StyleSocket triggerSocket) {
			_triggerSocket = triggerSocket;
			triggerSocketAssigned = true;
			_hideCondition = false;
		}

		public bool IsEnabled() {
			if(triggerSocketAssigned) {
				if(_triggerSocket.IsEnabled() == false)
					return false;
				//if(_triggerSocket.GetInput() != null) {
					if(_triggerSocket.GetInput() is Range<bool>) {
						if((Range<bool>)(_triggerSocket.GetInput()) == _hideCondition) {
							return false;
						} else {
							return true;
						}
					} else {
						return !_hideCondition;
					}
				//} else {
				//	return _hideCondition;
				//}
			} else {
				return true;
			}
 		}

		[SerializeField]
		List<StyleSocket> _sockets;

		List<StyleModule> _subModules;

		Dictionary<string, StyleModule> _subModulesByName;



		List<StyleSocket> GetSockets() {
			if(_sockets == null) 
				_sockets = new List<StyleSocket>();

			return _sockets;
		}

		List<StyleModule> GetSubmodules() {
			if(_subModules == null) _subModules = new List<StyleModule>();
			return _subModules;
		}

		public StyleModule GetSubmoduleByLabel(string name) {
			if(_subModulesByName == null) _subModulesByName = new Dictionary<string, StyleModule>();
			if(_subModulesByName.ContainsKey(name)) {
				return _subModulesByName[name];

			} else return null;
		}

		public virtual int GetNumberOfSubmodules() {
			return GetSubmodules().Count;
		}

		public virtual StyleModule GetSubmodule(int i) {
			return GetSubmodules()[i];
		}
		public virtual void AddSubmodule(StyleModule module) {
			if(_subModulesByName == null) _subModulesByName = new Dictionary<string, StyleModule>();
		 	GetSubmodules().Add(module);
			_subModulesByName[module.GetLabel()] = module;
		}


		// public virtual int GetNumberOfSockets() {
		// 	return GetSockets().Count;
		// }

		// public virtual StyleSocket GetSocket(int i) {
		// 	return GetSockets()[i];
		// }

		// public virtual void AddSubmodule(StyleSocket socket) {
		// 	GetSockets().Add(socket);
		// }

	
		public virtual string GetLabel() {
			return "Module";
		}

		public virtual void UpdateModule(string updatedSocket = null) {
			foreach(var socket in GetSockets()) {
				foreach(var link in socket.GetLinks()) {
					link.UpdateLink();
				}
			}
		}

	}
}

