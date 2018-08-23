using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis {
public class VTKDataset : Dataset {
	
	[SerializeField]
	string _datasetPath;

	VTK.vtkDataSet _dataset;


	public VTK.vtkDataSet GetVTKDataset() {
		if(_dataset == null) LoadDataset();
		return _dataset;
	}
	[SerializeField]
	VTKAnchorDataVariable _anchorVariable;
	
	public override Variable GetAnchor() {
		return _anchorVariable;
	}

	public override void SetAnchor(Variable anchor) {
		if(anchor is VTKAnchorDataVariable)
		_anchorVariable = (VTKAnchorDataVariable)anchor;
	}
	[SerializeField]
	int _instanceID = 0;

	[SerializeField]
	int _timestep = 0;

	public VTKDataset Init(string path, int instanceID, int timestep) {
		this.SetDatasetPath(path);
		_instanceID = instanceID;
		_timestep = timestep;
		return this;
	}

	public VTKDataset Init(VTK.vtkDataSet dataset, int instanceID, int timestep) {
		this.SetDatasetPath("");
		_instanceID = instanceID;
		_timestep = timestep;
		_dataset = dataset;
		return this;
	}
	protected override Datastream lookupDataStream(DataVariable variable, DataVariable anchor, int instanceID = 0, int timeStep = 0) {
        //if (validateVariable(variable)) {
        //    if (anchor == null && GetDataDimensionType() == DataDimensionType.Volume)
        //    {
                
        //    }
        //}


		return null;
	}

	public override bool ContainsInstanceID(int instanceID) {
		return instanceID == _instanceID;
	}
	public override bool ContainsTimestep(int timestep) {
		return timestep == _timestep;
	}

	protected override int queryNumberOfVariables() {
		return  GetVTKDataset().GetPointData().GetNumberOfArrays() + GetVTKDataset().GetCellData().GetNumberOfArrays() ;
	}
	
	protected override bool hasAnchor() {
		return (GetDataDimensionType() != DataDimensionType.Volume);
	}
	protected override AnchorVariable generateAnchor() {
		if(!hasAnchor()) return null;
		VTKAnchorDataVariable var = ScriptableObject.CreateInstance<VTKAnchorDataVariable>();
		var.Init(this);
		return var;
	}
	protected override DataVariable generateVarible(int i) {
		int numPointArrays = GetVTKDataset().GetPointData().GetNumberOfArrays();
		int numCellArrays = GetVTKDataset().GetCellData().GetNumberOfArrays();
		VTK.vtkAbstractArray abstractArray;

		int positionOffset = 0;
		DataVariable result = null;
		if(positionOffset == 1 &&  i == 0 ) {
			

		} else if(i < numPointArrays + positionOffset) {
			abstractArray = GetVTKDataset().GetPointData().GetAbstractArray(i-positionOffset);
			VTKDataVariable var = ScriptableObject.CreateInstance<VTKDataVariable>();
			var.Init(this, VTKDataVariable.ArrayType.Point, i - positionOffset);
			result = var;

		} else if (i < numPointArrays + numCellArrays + positionOffset) {
			VTKDataVariable var = ScriptableObject.CreateInstance<VTKDataVariable>();
			var.Init(this, VTKDataVariable.ArrayType.Cell,i - numPointArrays - positionOffset);
			result = var;
		} else return null;




		return result;
	}

	protected override DatastreamChannel generateDatastreamChannel(DataVariable variable) {
		if(variable is VTKDataVariable) {
			VTKDatastreamChannel vtkchannel = CreateInstance<VTKDatastreamChannel>();
			int arrayID = ((VTKDataVariable)variable).GetArrayID();
			VTK.vtkAbstractArray array;
			if(((VTKDataVariable)variable).IsCellVariable()) {
				array = GetVTKDataset().GetCellData().GetAbstractArray(arrayID);
			} else {
				array = GetVTKDataset().GetPointData().GetAbstractArray(arrayID);
			}
			vtkchannel.Init(GetVTKDataset(), array);
			return vtkchannel;
		} else if(variable is VTKAnchorDataVariable) {
			VTKAnchorDatastreamChannel vtkchannel = CreateInstance<VTKAnchorDatastreamChannel>();

			vtkchannel.Init(GetVTKDataset());
			return vtkchannel;;
		} else {
			return null;
		}
	}
	public void Print() {
		if(GetVTKDataset() != null) {
			Debug.Log(GetVTKDataset().GetClassName());
			// VTK.vtkAbstractArray abstractArray = _dataset.GetPointData()
			VTK.vtkPointSet pointSet = null;
			if(GetVTKDataset().IsA("vtkPointSet"))
				pointSet = VTK.vtkPointSet.SafeDownCast(GetVTKDataset());
			
			if(pointSet != null)
				Debug.Log("<Position> " + pointSet.GetNumberOfPoints() );
			for(int i = 0; i < GetVTKDataset().GetPointData().GetNumberOfArrays(); i++) {
				Debug.Log(GetVTKDataset().GetPointData().GetArrayName(i) + GetVTKDataset().GetPointData().GetAbstractArray(i).GetNumberOfTuples());
			}
		}
	}
	public void SetDatasetPath(string path) {
		_datasetPath = path;
	}
	public void SetDataset(VTK.vtkDataSet dataset) {
		_dataset = dataset;
	}
	public override bool LoadDataset() {
		if(_datasetPath != "" )
			_dataset = DEPRECATED.DataLoader.LoadVTKDataSet(_datasetPath);
		if(!GetVTKDataset().IsVoid())
			populateVariables();
		return !GetVTKDataset().IsVoid();
	}
	protected override bool validateVariable(DataVariable variable) {
		return base.validateVariable(variable) && (variable is VTKDataVariable || variable is VTKAnchorDataVariable);
	} 


	public override void Bind(DataVariable variable, Material material, int bindSlot, int instanceID = 0, int timeStep = 0) {
		if(!validateVariable(variable)) {
			Debug.LogError("Variable is not a valid VTK variable for this VTK dataset.");
		}
	}

	public override bool IsPath() {
		return GetVTKDataset().IsA("vtkPolyData") &&  (VTK.vtkPolyData.SafeDownCast(GetVTKDataset()).GetNumberOfLines()) > 0;
	}

	public override bool IsMesh() {
		if(GetVTKDataset().IsA("vtkPolyData") && (VTK.vtkPolyData.SafeDownCast(GetVTKDataset())).GetNumberOfPolys() > 0) return true;
		else if(GetVTKDataset().IsA("vtkUnstructuredGrid")) return true;
		else return false;
	}

	public override bool IsVolume() {
		if(GetVTKDataset() == null) return false;
		return GetVTKDataset().IsA("vtkImageData"); // /* Perhaps verify the dimensionality of the image?*/ && VTK.vtkImageData.SafeDownCast(_dataset).GetDi; 
	}


	public override Vector3 GetMin(DataVariable variable) {
		Vector3 result = new Vector3();
		if(variable is VTKDataVariable)
		if(GetAbstractArray((VTKDataVariable)variable).IsA("vtkDataArray")) {
			VTK.vtkDataArray da = VTK.vtkDataArray.SafeDownCast(GetAbstractArray((VTKDataVariable)variable));
			double[] r = new double[2];
			da.GetRange(r,0);
			result.x = (float)r[0];
			da.GetRange(r,1);
			result.y = (float)r[0];
			da.GetRange(r,2);
			result.z = (float)r[0];
		}

		return result;
	}

	public override Vector3 GetMax(DataVariable variable) {
		Vector3 result = new Vector3();
		if(variable is VTKDataVariable && GetAbstractArray((VTKDataVariable)variable).IsA("vtkDataArray")) {
			VTK.vtkDataArray da = VTK.vtkDataArray.SafeDownCast(GetAbstractArray((VTKDataVariable)variable));
			double[] r = new double[2];
			da.GetRange(r,0);
			result.x = (float)r[1];
			da.GetRange(r,1);
			result.y = (float)r[1];
			da.GetRange(r,2);
			result.z = (float)r[1];
		}

		return result;
	}
	VTK.vtkAbstractArray GetAbstractArray(VTKDataVariable variable ) {
		if(variable is VTKDataVariable) {
			VTKDataVariable vtkVariable = (VTKDataVariable)(variable);

			if(vtkVariable.IsCellVariable()) {
				VTK.vtkAbstractArray abstractArray = GetVTKDataset().GetCellData().GetAbstractArray(vtkVariable.GetArrayID());
				if(abstractArray.IsVoid()) {
					return null;
				}
				return abstractArray;
			} 

			if(vtkVariable.IsPointVariable()) {
				VTK.vtkAbstractArray abstractArray = GetVTKDataset().GetPointData().GetAbstractArray(vtkVariable.GetArrayID());
				if(abstractArray.IsVoid()) {
					return null;
				}
				return abstractArray;
			} 
		}
		return null;
	}
	public override string GetVariableName(DataVariable variable) {
		// if(variable.IsAnchor()) return "Anchor";
		if(!validateVariable(variable)) {
			Debug.LogError("Variable is not a valid VTK variable for this VTK dataset.");
			return "ERROR";
		}

		if(variable is VTKDataVariable) {
			VTKDataVariable vtkVariable = (VTKDataVariable)(variable);
			
			VTK.vtkAbstractArray abstractArray = GetAbstractArray(vtkVariable);
			if(abstractArray == null) {
				return "";
			} else {
				return abstractArray.GetName();
			}
		} else if(variable is VTKAnchorDataVariable) {
			VTKAnchorDataVariable positionVariable = (VTKAnchorDataVariable)variable;
			return "Position";
		}
		
		return "";
	}

	public override int GetVariableComponents(DataVariable variable) {
			if(!validateVariable(variable)) {
			Debug.LogError("Variable is not a valid VTK variable for this VTK dataset.");
			return -1;
		}

		if(variable is VTKDataVariable) {
			VTKDataVariable vtkVariable = (VTKDataVariable)(variable);
			
			VTK.vtkAbstractArray abstractArray = GetAbstractArray(vtkVariable);
			if(abstractArray != null) {
				return abstractArray.GetNumberOfComponents();
			}
		} else if(variable is VTKAnchorDataVariable) {
			VTKAnchorDataVariable positionVariable = (VTKAnchorDataVariable)variable;
			return 3;
		}
		
		return 0;
	}
	public override bool IsVector(DataVariable variable) {
		if(variable is VTKDataVariable) {
			VTKDataVariable vtkVariable = (VTKDataVariable)(variable);

			if(vtkVariable.IsCellVariable()) {
				VTK.vtkAbstractArray abstractArray = GetVTKDataset().GetCellData().GetAbstractArray(vtkVariable.GetArrayID());
				if(abstractArray.IsVoid()) {
					return false;
				}
				return abstractArray.GetNumberOfComponents() > 1;
			} 

			if(vtkVariable.IsPointVariable()) {
				VTK.vtkAbstractArray abstractArray = GetVTKDataset().GetPointData().GetAbstractArray(vtkVariable.GetArrayID());
				if(abstractArray.IsVoid()) {
					return false;
				}
				return abstractArray.GetNumberOfComponents() > 1;
			} 
		} else if(variable is VTKAnchorDataVariable) {
			VTKAnchorDataVariable positionVariable = (VTKAnchorDataVariable)variable;
			return true;
		}  

		return false;
	}


	public override Bounds GetBounds() {
		if(GetVTKDataset() == null) return new Bounds();
		return GetVTKDataset().GetBounds();
	}
}
}
