

using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace VTK {
public partial class vtkXMLUnstructuredDataReader : vtkXMLDataReader {
		public vtkXMLUnstructuredDataReader(IntPtr p) : base(p) {}
		public static implicit operator  vtkXMLUnstructuredDataReader(IntPtr p) {return new vtkXMLUnstructuredDataReader(p);}
		public static implicit operator  IntPtr(vtkXMLUnstructuredDataReader o) {return o.GetPtr();}

// static vtkTypeBool IsTypeOf(const char * type)
// "static vtkTypeBool IsTypeOf(const char *type)"
public static bool IsTypeOf(string /*(char*)*/ type) {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new bool(), return_elements);
	VTK_API.API_vtkXMLUnstructuredDataReader.IsTypeOf_0(returnPointer.GetPtr(), type);
	return (bool)returnPointer;
}


// vtkTypeBool IsA(const char * type)
// "vtkTypeBool IsA(const char *type)"
public bool IsA(string /*(char*)*/ type) {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new bool(), return_elements);
	VTK_API.API_vtkXMLUnstructuredDataReader.IsA_0(returnPointer.GetPtr(), this, type);
	return (bool)returnPointer;
}


// static vtkXMLUnstructuredDataReader* SafeDownCast(vtkObjectBase * o)
// "static vtkXMLUnstructuredDataReader* SafeDownCast(vtkObjectBase *o)"
public static vtkXMLUnstructuredDataReader SafeDownCast(vtkObjectBase /*(vtkObjectBase*)*/ o) {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr(), return_elements);
	VTK_API.API_vtkXMLUnstructuredDataReader.SafeDownCast_0(returnPointer.GetPtr(), o);
	return (vtkXMLUnstructuredDataReader)(IntPtr)returnPointer;
}


// vtkXMLUnstructuredDataReader* NewInstance()
// "vtkXMLUnstructuredDataReader *NewInstance()"
public vtkXMLUnstructuredDataReader NewInstance() {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr(), return_elements);
	VTK_API.API_vtkXMLUnstructuredDataReader.NewInstance_0(returnPointer.GetPtr(), this);
	return (vtkXMLUnstructuredDataReader)(IntPtr)returnPointer;
}


// vtkIdType GetNumberOfPoints()
// "vtkIdType GetNumberOfPoints()"
public long GetNumberOfPoints() {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new long(), return_elements);
	VTK_API.API_vtkXMLUnstructuredDataReader.GetNumberOfPoints_0(returnPointer.GetPtr(), this);
	return (long)returnPointer;
}


// vtkIdType GetNumberOfCells()
// "vtkIdType GetNumberOfCells()"
public long GetNumberOfCells() {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new long(), return_elements);
	VTK_API.API_vtkXMLUnstructuredDataReader.GetNumberOfCells_0(returnPointer.GetPtr(), this);
	return (long)returnPointer;
}


// virtual vtkIdType GetNumberOfPieces()
// "virtual vtkIdType GetNumberOfPieces()"
public long GetNumberOfPieces() {
	int return_elements = 1;
	ReturnPointer returnPointer = new ReturnPointer(new long(), return_elements);
	VTK_API.API_vtkXMLUnstructuredDataReader.GetNumberOfPieces_0(returnPointer.GetPtr(), this);
	return (long)returnPointer;
}


// void SetupUpdateExtent(int piece, int numberOfPieces, int ghostLevel)
// "void SetupUpdateExtent(int piece, int numberOfPieces, int ghostLevel)"
public void SetupUpdateExtent(int /*(int)*/ piece, int /*(int)*/ numberOfPieces, int /*(int)*/ ghostLevel) {
	VTK_API.API_vtkXMLUnstructuredDataReader.SetupUpdateExtent_0(this, piece, numberOfPieces, ghostLevel);
}


}
};
