

using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace VTK {
public partial class vtkUnstructuredGridBase : vtkPointSet {
		public vtkUnstructuredGridBase(IntPtr p) : base(p) {}
		public static implicit operator  vtkUnstructuredGridBase(IntPtr p) {return new vtkUnstructuredGridBase(p);}
		public static implicit operator  IntPtr(vtkUnstructuredGridBase o) {return o.GetPtr();}

// static vtkTypeBool IsTypeOf(const char * type)
// "static vtkTypeBool IsTypeOf(const char *type)"
public static bool IsTypeOf(string /*(char*)*/ type) {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new bool(), return_elements);
	VTK_API.API_vtkUnstructuredGridBase.IsTypeOf_0(returnPointer.GetPtr(), type);
	return (bool)returnPointer;
}


// vtkTypeBool IsA(const char * type)
// "vtkTypeBool IsA(const char *type)"
public bool IsA(string /*(char*)*/ type) {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new bool(), return_elements);
	VTK_API.API_vtkUnstructuredGridBase.IsA_0(returnPointer.GetPtr(), this, type);
	return (bool)returnPointer;
}


// static vtkUnstructuredGridBase* SafeDownCast(vtkObjectBase * o)
// "static vtkUnstructuredGridBase* SafeDownCast(vtkObjectBase *o)"
public static vtkUnstructuredGridBase SafeDownCast(vtkObjectBase /*(vtkObjectBase*)*/ o) {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr(), return_elements);
	VTK_API.API_vtkUnstructuredGridBase.SafeDownCast_0(returnPointer.GetPtr(), o);
	return (vtkUnstructuredGridBase)(IntPtr)returnPointer;
}


// vtkUnstructuredGridBase* NewInstance()
// "vtkUnstructuredGridBase *NewInstance()"
public vtkUnstructuredGridBase NewInstance() {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr(), return_elements);
	VTK_API.API_vtkUnstructuredGridBase.NewInstance_0(returnPointer.GetPtr(), this);
	return (vtkUnstructuredGridBase)(IntPtr)returnPointer;
}


// int GetDataObjectType()
// "int GetDataObjectType()"
public int GetDataObjectType() {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new int(), return_elements);
	VTK_API.API_vtkUnstructuredGridBase.GetDataObjectType_0(returnPointer.GetPtr(), this);
	return (int)returnPointer;
}


// virtual void Allocate(vtkIdType numCells = 1000, int extSize = 1000)
// "virtual void Allocate(vtkIdType numCells=1000, int extSize=1000)"
public void Allocate(long /*(vtkIdType)*/ numCells, int /*(int)*/ extSize) {
	VTK_API.API_vtkUnstructuredGridBase.Allocate_0(this, numCells, extSize);
}


// void DeepCopy(vtkDataObject * src)
// "void DeepCopy(vtkDataObject *src)"
public void DeepCopy(vtkDataObject /*(vtkDataObject*)*/ src) {
	VTK_API.API_vtkUnstructuredGridBase.DeepCopy_0(this, src);
}


// virtual vtkIdType InsertNextCell(int type, vtkIdType npts, vtkIdType * ptIds)
// "virtual vtkIdType InsertNextCell(int type, vtkIdType npts, vtkIdType *ptIds)"
public long InsertNextCell(int /*(int)*/ type, long /*(vtkIdType)*/ npts, IntPtr /*(vtkIdType*)*/ ptIds) {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new long(), return_elements);
	VTK_API.API_vtkUnstructuredGridBase.InsertNextCell_0(returnPointer.GetPtr(), this, type, npts, ptIds);
	return (long)returnPointer;
}


// virtual vtkIdType InsertNextCell(int type, vtkIdList * ptIds)
// "virtual vtkIdType InsertNextCell(int type, vtkIdList *ptIds)"
public long InsertNextCell(int /*(int)*/ type, vtkIdList /*(vtkIdList*)*/ ptIds) {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new long(), return_elements);
	VTK_API.API_vtkUnstructuredGridBase.InsertNextCell_1(returnPointer.GetPtr(), this, type, ptIds);
	return (long)returnPointer;
}


// virtual vtkIdType InsertNextCell(int type, vtkIdType npts, vtkIdType * ptIds, vtkIdType nfaces, vtkIdType * faces)
// "virtual vtkIdType InsertNextCell(int type, vtkIdType npts, vtkIdType *ptIds, vtkIdType nfaces, vtkIdType *faces)"
public long InsertNextCell(int /*(int)*/ type, long /*(vtkIdType)*/ npts, IntPtr /*(vtkIdType*)*/ ptIds, long /*(vtkIdType)*/ nfaces, IntPtr /*(vtkIdType*)*/ faces) {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new long(), return_elements);
	VTK_API.API_vtkUnstructuredGridBase.InsertNextCell_2(returnPointer.GetPtr(), this, type, npts, ptIds, nfaces, faces);
	return (long)returnPointer;
}


// virtual void ReplaceCell(vtkIdType cellId, int npts, vtkIdType * pts)
// "virtual void ReplaceCell(vtkIdType cellId, int npts, vtkIdType *pts)"
public void ReplaceCell(long /*(vtkIdType)*/ cellId, int /*(int)*/ npts, IntPtr /*(vtkIdType*)*/ pts) {
	VTK_API.API_vtkUnstructuredGridBase.ReplaceCell_0(this, cellId, npts, pts);
}


// virtual void GetIdsOfCellsOfType(int type, vtkIdTypeArray * array)
// "virtual void GetIdsOfCellsOfType(int type, vtkIdTypeArray *array)"
public void GetIdsOfCellsOfType(int /*(int)*/ type, vtkIdTypeArray /*(vtkIdTypeArray*)*/ array) {
	VTK_API.API_vtkUnstructuredGridBase.GetIdsOfCellsOfType_0(this, type, array);
}


// virtual int IsHomogeneous()
// "virtual int IsHomogeneous()"
public int IsHomogeneous() {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new int(), return_elements);
	VTK_API.API_vtkUnstructuredGridBase.IsHomogeneous_0(returnPointer.GetPtr(), this);
	return (int)returnPointer;
}


}
};
