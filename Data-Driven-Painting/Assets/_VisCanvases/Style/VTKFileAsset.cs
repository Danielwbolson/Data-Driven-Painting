using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VTKFileAsset:FileAsset  {
	
	VTK.vtkDataSet _dataset;

	public VTK.vtkDataSet GetDataset() {
		if(!IsValid()) LoadFile();
		return _dataset;
	}

	public virtual  bool IsValid() {
		return _dataset!= null && !_dataset.IsVoid();
	}

	public virtual void LoadFile() {
		_dataset = DEPRECATED.DataLoader.LoadVTKDataSet(GetPath());
	}
	
}
