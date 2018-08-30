

using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace VTK {
public partial class vtkStructuredPoints : vtkImageData {
		public vtkStructuredPoints(IntPtr p) : base(p) {}
		public static implicit operator  vtkStructuredPoints(IntPtr p) {return new vtkStructuredPoints(p);}
		public static implicit operator  IntPtr(vtkStructuredPoints o) {return o.GetPtr();}

// static vtkStructuredPoints* New()
// "static vtkStructuredPoints *New()"
public static vtkStructuredPoints New() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkStructuredPoints.New_0(returnPointer.GetPtr());
	return (vtkStructuredPoints)(IntPtr)returnPointer;
}


// static vtkTypeBool IsTypeOf(const char * type)
// "static vtkTypeBool IsTypeOf(const char *type)"
public static bool IsTypeOf(string /*(char*)*/ type) {
	ReturnPointer returnPointer = new ReturnPointer(new bool());
	VTK_API.API_vtkStructuredPoints.IsTypeOf_0(returnPointer.GetPtr(), type);
	return (bool)returnPointer;
}


// vtkTypeBool IsA(const char * type)
// "vtkTypeBool IsA(const char *type)"
public bool IsA(string /*(char*)*/ type) {
	ReturnPointer returnPointer = new ReturnPointer(new bool());
	VTK_API.API_vtkStructuredPoints.IsA_0(returnPointer.GetPtr(), this, type);
	return (bool)returnPointer;
}


// static vtkStructuredPoints* SafeDownCast(vtkObjectBase * o)
// "static vtkStructuredPoints* SafeDownCast(vtkObjectBase *o)"
public static vtkStructuredPoints SafeDownCast(vtkObjectBase /*(vtkObjectBase*)*/ o) {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkStructuredPoints.SafeDownCast_0(returnPointer.GetPtr(), o);
	return (vtkStructuredPoints)(IntPtr)returnPointer;
}


// vtkStructuredPoints* NewInstance()
// "vtkStructuredPoints *NewInstance()"
public vtkStructuredPoints NewInstance() {
	ReturnPointer returnPointer = new ReturnPointer(new IntPtr());
	VTK_API.API_vtkStructuredPoints.NewInstance_0(returnPointer.GetPtr(), this);
	return (vtkStructuredPoints)(IntPtr)returnPointer;
}


// int GetDataObjectType()
// "int GetDataObjectType()"
public int GetDataObjectType() {
	ReturnPointer returnPointer = new ReturnPointer(new int());
	VTK_API.API_vtkStructuredPoints.GetDataObjectType_0(returnPointer.GetPtr(), this);
	return (int)returnPointer;
}


}
};
