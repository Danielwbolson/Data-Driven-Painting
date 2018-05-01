

using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace VTK {
public partial class vtkCellLinks : vtkAbstractCellLinks {

// static vtkCellLinks* New()
// "static vtkCellLinks *New()"
public new static vtkCellLinks New() {
	int return_elements = 1;
	IntPtr returnPointer = Marshal.AllocHGlobal(Marshal.SizeOf(new IntPtr())*return_elements);
	API_vtkCellLinks.New_0(returnPointer);
	return Ptr.deref(returnPointer);
}


// void BuildLinks(vtkDataSet * data)
// "void BuildLinks(vtkDataSet *data)"
public new void BuildLinks(vtkDataSet /*(vtkDataSet*)*/ data) {
	API_vtkCellLinks.BuildLinks_0(this, data);
}


// void BuildLinks(vtkDataSet * data, vtkCellArray * Connectivity)
// "void BuildLinks(vtkDataSet *data, vtkCellArray *Connectivity)"
public new void BuildLinks(vtkDataSet /*(vtkDataSet*)*/ data, vtkCellArray /*(vtkCellArray*)*/ Connectivity) {
	API_vtkCellLinks.BuildLinks_1(this, data, Connectivity);
}


// void Allocate(vtkIdType numLinks, vtkIdType ext = 1000)
// "void Allocate(vtkIdType numLinks, vtkIdType ext=1000)"
public new void Allocate(long /*(vtkIdType)*/ numLinks, long /*(vtkIdType)*/ ext) {
	API_vtkCellLinks.Allocate_0(this, numLinks, ext);
}


// void Initialize()
// "void Initialize()"
public new void Initialize() {
	API_vtkCellLinks.Initialize_0(this);
}


// Link& GetLink(vtkIdType ptId)
// "Link &GetLink(vtkIdType ptId)"
public new Link GetLink(long /*(vtkIdType)*/ ptId) {
	int return_elements = 1;
	IntPtr returnPointer = Marshal.AllocHGlobal(Marshal.SizeOf(new Link())*return_elements);
	API_vtkCellLinks.GetLink_0(this, returnPointer, ptId);
	return Ptr.deref(returnPointer);
}


// short GetNcells(vtkIdType ptId)
// "unsigned short GetNcells(vtkIdType ptId)"
public new short GetNcells(long /*(vtkIdType)*/ ptId) {
	int return_elements = 1;
	IntPtr returnPointer = Marshal.AllocHGlobal(Marshal.SizeOf(new short())*return_elements);
	API_vtkCellLinks.GetNcells_0(this, returnPointer, ptId);
	return Ptr.deref(returnPointer);
}


// vtkIdType* GetCells(vtkIdType ptId)
// "vtkIdType *GetCells(vtkIdType ptId)"
public new long GetCells(long /*(vtkIdType)*/ ptId) {
	int return_elements = 1;
	IntPtr returnPointer = Marshal.AllocHGlobal(Marshal.SizeOf(new long())*return_elements);
	API_vtkCellLinks.GetCells_0(this, returnPointer, ptId);
	return Ptr.deref(returnPointer);
}


// vtkIdType InsertNextPoint(int numLinks)
// "vtkIdType InsertNextPoint(int numLinks)"
public new long InsertNextPoint(int /*(int)*/ numLinks) {
	int return_elements = 1;
	IntPtr returnPointer = Marshal.AllocHGlobal(Marshal.SizeOf(new long())*return_elements);
	API_vtkCellLinks.InsertNextPoint_0(this, returnPointer, numLinks);
	return Ptr.deref(returnPointer);
}


// void InsertNextCellReference(vtkIdType ptId, vtkIdType cellId)
// "void InsertNextCellReference(vtkIdType ptId, vtkIdType cellId)"
public new void InsertNextCellReference(long /*(vtkIdType)*/ ptId, long /*(vtkIdType)*/ cellId) {
	API_vtkCellLinks.InsertNextCellReference_0(this, ptId, cellId);
}


// void DeletePoint(vtkIdType ptId)
// "void DeletePoint(vtkIdType ptId)"
public new void DeletePoint(long /*(vtkIdType)*/ ptId) {
	API_vtkCellLinks.DeletePoint_0(this, ptId);
}


// void RemoveCellReference(vtkIdType cellId, vtkIdType ptId)
// "void RemoveCellReference(vtkIdType cellId, vtkIdType ptId)"
public new void RemoveCellReference(long /*(vtkIdType)*/ cellId, long /*(vtkIdType)*/ ptId) {
	API_vtkCellLinks.RemoveCellReference_0(this, cellId, ptId);
}


// void AddCellReference(vtkIdType cellId, vtkIdType ptId)
// "void AddCellReference(vtkIdType cellId, vtkIdType ptId)"
public new void AddCellReference(long /*(vtkIdType)*/ cellId, long /*(vtkIdType)*/ ptId) {
	API_vtkCellLinks.AddCellReference_0(this, cellId, ptId);
}


// void ResizeCellList(vtkIdType ptId, int size)
// "void ResizeCellList(vtkIdType ptId, int size)"
public new void ResizeCellList(long /*(vtkIdType)*/ ptId, int /*(int)*/ size) {
	API_vtkCellLinks.ResizeCellList_0(this, ptId, size);
}


// void Squeeze()
// "void Squeeze()"
public new void Squeeze() {
	API_vtkCellLinks.Squeeze_0(this);
}


// void Reset()
// "void Reset()"
public new void Reset() {
	API_vtkCellLinks.Reset_0(this);
}


// long GetActualMemorySize()
// "unsigned long GetActualMemorySize()"
public new ulong GetActualMemorySize() {
	int return_elements = 1;
	IntPtr returnPointer = Marshal.AllocHGlobal(Marshal.SizeOf(new ulong())*return_elements);
	API_vtkCellLinks.GetActualMemorySize_0(this, returnPointer);
	return Ptr.deref(returnPointer);
}


// void DeepCopy(vtkCellLinks * src)
// "void DeepCopy(vtkCellLinks *src)"
public new void DeepCopy(vtkCellLinks /*(vtkCellLinks*)*/ src) {
	API_vtkCellLinks.DeepCopy_0(this, src);
}


}
};
