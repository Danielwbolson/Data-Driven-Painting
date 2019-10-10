

using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace VTK {
public partial class vtkDataSetAlgorithm : vtkAlgorithm {
		public vtkDataSetAlgorithm(IntPtr p) : base(p) {}
		public static implicit operator  vtkDataSetAlgorithm(IntPtr p) {return new vtkDataSetAlgorithm(p);}
		public static implicit operator  IntPtr(vtkDataSetAlgorithm o) {return o.GetPtr();}

// static vtkDataSetAlgorithm* New()
// "static vtkDataSetAlgorithm *New()"
public static vtkDataSetAlgorithm New() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.New_0(returnPointer.GetPtr());
	return (vtkDataSetAlgorithm)(IntPtr)returnPointer;
}


// static vtkTypeBool IsTypeOf(const char * type)
// "static vtkTypeBool IsTypeOf(const char *type)"
public static bool IsTypeOf(string /*(char*)*/ type) {
	ReturnPointer returnPointer = new ReturnPointer(new bool());
	VTK_API.API_vtkDataSetAlgorithm.IsTypeOf_0(returnPointer.GetPtr(), type);
	return (bool)returnPointer;
}


// vtkTypeBool IsA(const char * type)
// "vtkTypeBool IsA(const char *type)"
public bool IsA(string /*(char*)*/ type) {
	ReturnPointer returnPointer = new ReturnPointer(new bool());
	VTK_API.API_vtkDataSetAlgorithm.IsA_0(returnPointer.GetPtr(), this, type);
	return (bool)returnPointer;
}


// static vtkDataSetAlgorithm* SafeDownCast(vtkObjectBase * o)
// "static vtkDataSetAlgorithm* SafeDownCast(vtkObjectBase *o)"
public static vtkDataSetAlgorithm SafeDownCast(vtkObjectBase /*(vtkObjectBase*)*/ o) {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.SafeDownCast_0(returnPointer.GetPtr(), o);
	return (vtkDataSetAlgorithm)(IntPtr)returnPointer;
}


// vtkDataSetAlgorithm* NewInstance()
// "vtkDataSetAlgorithm *NewInstance()"
public vtkDataSetAlgorithm NewInstance() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.NewInstance_0(returnPointer.GetPtr(), this);
	return (vtkDataSetAlgorithm)(IntPtr)returnPointer;
}


// vtkDataSet* GetOutput()
// "vtkDataSet* GetOutput()"
public vtkDataSet GetOutput() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.GetOutput_0(returnPointer.GetPtr(), this);
	return (vtkDataSet)(IntPtr)returnPointer;
}


// vtkDataSet* GetOutput(int ARG_0)
// "vtkDataSet* GetOutput(int)"
public vtkDataSet GetOutput(int /*(int)*/ ARG_0) {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.GetOutput_1(returnPointer.GetPtr(), this, ARG_0);
	return (vtkDataSet)(IntPtr)returnPointer;
}


// vtkDataObject* GetInput()
// "vtkDataObject* GetInput()"
public vtkDataObject GetInput() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.GetInput_0(returnPointer.GetPtr(), this);
	return (vtkDataObject)(IntPtr)returnPointer;
}


// vtkPolyData* GetPolyDataOutput()
// "vtkPolyData *GetPolyDataOutput()"
public vtkPolyData GetPolyDataOutput() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.GetPolyDataOutput_0(returnPointer.GetPtr(), this);
	return (vtkPolyData)(IntPtr)returnPointer;
}


// vtkStructuredPoints* GetStructuredPointsOutput()
// "vtkStructuredPoints *GetStructuredPointsOutput()"
public vtkStructuredPoints GetStructuredPointsOutput() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.GetStructuredPointsOutput_0(returnPointer.GetPtr(), this);
	return (vtkStructuredPoints)(IntPtr)returnPointer;
}


// vtkImageData* GetImageDataOutput()
// "vtkImageData *GetImageDataOutput()"
public vtkImageData GetImageDataOutput() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.GetImageDataOutput_0(returnPointer.GetPtr(), this);
	return (vtkImageData)(IntPtr)returnPointer;
}


// vtkStructuredGrid* GetStructuredGridOutput()
// "vtkStructuredGrid *GetStructuredGridOutput()"
public vtkStructuredGrid GetStructuredGridOutput() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.GetStructuredGridOutput_0(returnPointer.GetPtr(), this);
	return (vtkStructuredGrid)(IntPtr)returnPointer;
}


// vtkUnstructuredGrid* GetUnstructuredGridOutput()
// "vtkUnstructuredGrid *GetUnstructuredGridOutput()"
public vtkUnstructuredGrid GetUnstructuredGridOutput() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkDataSetAlgorithm.GetUnstructuredGridOutput_0(returnPointer.GetPtr(), this);
	return (vtkUnstructuredGrid)(IntPtr)returnPointer;
}


// void SetInputData(vtkDataObject * ARG_0)
// "void SetInputData(vtkDataObject*)"
public void SetInputData(vtkDataObject /*(vtkDataObject*)*/ ARG_0) {
	VTK_API.API_vtkDataSetAlgorithm.SetInputData_0(this, ARG_0);
}


// void SetInputData(int ARG_0, vtkDataObject * ARG_1)
// "void SetInputData(int, vtkDataObject*)"
public void SetInputData(int /*(int)*/ ARG_0, vtkDataObject /*(vtkDataObject*)*/ ARG_1) {
	VTK_API.API_vtkDataSetAlgorithm.SetInputData_1(this, ARG_0, ARG_1);
}


// void SetInputData(vtkDataSet * ARG_0)
// "void SetInputData(vtkDataSet*)"
public void SetInputData(vtkDataSet /*(vtkDataSet*)*/ ARG_0) {
	VTK_API.API_vtkDataSetAlgorithm.SetInputData_2(this, ARG_0);
}


// void SetInputData(int ARG_0, vtkDataSet * ARG_1)
// "void SetInputData(int, vtkDataSet*)"
public void SetInputData(int /*(int)*/ ARG_0, vtkDataSet /*(vtkDataSet*)*/ ARG_1) {
	VTK_API.API_vtkDataSetAlgorithm.SetInputData_3(this, ARG_0, ARG_1);
}


// void AddInputData(vtkDataObject * ARG_0)
// "void AddInputData(vtkDataObject *)"
public void AddInputData(vtkDataObject /*(vtkDataObject*)*/ ARG_0) {
	VTK_API.API_vtkDataSetAlgorithm.AddInputData_0(this, ARG_0);
}


// void AddInputData(vtkDataSet * ARG_0)
// "void AddInputData(vtkDataSet*)"
public void AddInputData(vtkDataSet /*(vtkDataSet*)*/ ARG_0) {
	VTK_API.API_vtkDataSetAlgorithm.AddInputData_1(this, ARG_0);
}


// void AddInputData(int ARG_0, vtkDataSet * ARG_1)
// "void AddInputData(int, vtkDataSet*)"
public void AddInputData(int /*(int)*/ ARG_0, vtkDataSet /*(vtkDataSet*)*/ ARG_1) {
	VTK_API.API_vtkDataSetAlgorithm.AddInputData_2(this, ARG_0, ARG_1);
}


// void AddInputData(int ARG_0, vtkDataObject * ARG_1)
// "void AddInputData(int, vtkDataObject*)"
public void AddInputData(int /*(int)*/ ARG_0, vtkDataObject /*(vtkDataObject*)*/ ARG_1) {
	VTK_API.API_vtkDataSetAlgorithm.AddInputData_3(this, ARG_0, ARG_1);
}


// int ProcessRequest(vtkInformation * request, vtkInformationVector ** inputVector, vtkInformationVector * outputVector)
// "int ProcessRequest(vtkInformation* request, vtkInformationVector** inputVector, vtkInformationVector* outputVector)"
public int ProcessRequest(vtkInformation /*(vtkInformation*)*/ request, vtkInformationVector /*(vtkInformationVector**)*/ inputVector, vtkInformationVector /*(vtkInformationVector*)*/ outputVector) {
	ReturnPointer returnPointer = new ReturnPointer(new int());
	VTK_API.API_vtkDataSetAlgorithm.ProcessRequest_0(returnPointer.GetPtr(), this, request, inputVector, outputVector);
	return (int)returnPointer;
}


}
};
