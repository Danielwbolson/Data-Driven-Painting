using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis{
	public class StyleModule : ScriptableObject{


		public System.Guid InstanceID {get; protected set;}
		// Other properties, etc.
		public virtual string GetUniqueIdentifier() {
			return InstanceID + "";
		}



			

		[SerializeField]
		List<StyleSocket> _sockets;

		List<StyleModule> _subModules;


		List<StyleSocket> GetSockets() {
			if(_sockets == null) 
				_sockets = new List<StyleSocket>();

			return _sockets;
		}

		List<StyleModule> GetSubmodules() {
			if(_subModules == null) _subModules = new List<StyleModule>();
			return _subModules;
		}

		public virtual int GetNumberOfSubmodules() {
			return GetSubmodules().Count;
		}

		public virtual StyleModule GetSubmodule(int i) {
			return GetSubmodules()[i];
		}
		public virtual void AddSubmodule(StyleModule module) {
		 	GetSubmodules().Add(module);
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

		public virtual void UpdateModule() {
			foreach(var socket in GetSockets()) {
				foreach(var link in socket.GetLinks()) {
					link.UpdateLink();
				}
			}
		}

	}
}

