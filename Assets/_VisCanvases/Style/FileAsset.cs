using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileAsset  {
	string _path;
	public void SetPath(string path) {
		_path = path;
	}
	public string GetPath() {
		return _path;
	}
	public virtual  bool IsValid() {
		return false;
	}

	public virtual void LoadFile() {

	}
	
}
