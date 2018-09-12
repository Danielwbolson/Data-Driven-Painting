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

