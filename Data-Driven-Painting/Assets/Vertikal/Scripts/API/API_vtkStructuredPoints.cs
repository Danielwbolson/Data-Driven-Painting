
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace VTK_API {
class API_vtkStructuredPoints {

// static vtkStructuredPoints* New()
// static vtkStructuredPoints *New()
[DllImport("vtkplugin", EntryPoint="vtkStructuredPoints_New_0")] public static extern 
bool New_0(IntPtr /*(IntPtr**)*/ return_value);

// static vtkTypeBool IsTypeOf(const char * type)
// static vtkTypeBool IsTypeOf(const char *type)
[DllImport("vtkplugin", EntryPoint="vtkStructuredPoints_IsTypeOf_0")] public static extern 
bool IsTypeOf_0(IntPtr /*(IntPtr*)*/ return_value, string /*(char*)*/ type);

// vtkTypeBool IsA(const char * type)
// vtkTypeBool IsA(const char *type)
[DllImport("vtkplugin", EntryPoint="vtkStructuredPoints_IsA_0")] public static extern 
bool IsA_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkStructuredPoints*)*/ callingObject, string /*(char*)*/ type);

// static vtkStructuredPoints* SafeDownCast(vtkObjectBase * o)
// static vtkStructuredPoints* SafeDownCast(vtkObjectBase *o)
[DllImport("vtkplugin", EntryPoint="vtkStructuredPoints_SafeDownCast_0")] public static extern 
bool SafeDownCast_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkObjectBase*)*/ o);

// vtkStructuredPoints* NewInstance()
// vtkStructuredPoints *NewInstance()
[DllImport("vtkplugin", EntryPoint="vtkStructuredPoints_NewInstance_0")] public static extern 
bool NewInstance_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkStructuredPoints*)*/ callingObject);

// int GetDataObjectType()
// int GetDataObjectType()
[DllImport("vtkplugin", EntryPoint="vtkStructuredPoints_GetDataObjectType_0")] public static extern 
bool GetDataObjectType_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkStructuredPoints*)*/ callingObject);

}
};
