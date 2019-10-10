
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace VTK_API {
class API_vtkDataSetAlgorithm {

// static vtkDataSetAlgorithm* New()
// static vtkDataSetAlgorithm *New()
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_New_0")] public static extern 
bool New_0(IntPtr /*(IntPtr**)*/ return_value);

// static vtkTypeBool IsTypeOf(const char * type)
// static vtkTypeBool IsTypeOf(const char *type)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_IsTypeOf_0")] public static extern 
bool IsTypeOf_0(IntPtr /*(IntPtr*)*/ return_value, string /*(char*)*/ type);

// vtkTypeBool IsA(const char * type)
// vtkTypeBool IsA(const char *type)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_IsA_0")] public static extern 
bool IsA_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, string /*(char*)*/ type);

// static vtkDataSetAlgorithm* SafeDownCast(vtkObjectBase * o)
// static vtkDataSetAlgorithm* SafeDownCast(vtkObjectBase *o)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_SafeDownCast_0")] public static extern 
bool SafeDownCast_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkObjectBase*)*/ o);

// vtkDataSetAlgorithm* NewInstance()
// vtkDataSetAlgorithm *NewInstance()
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_NewInstance_0")] public static extern 
bool NewInstance_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject);

// vtkDataSet* GetOutput()
// vtkDataSet* GetOutput()
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_GetOutput_0")] public static extern 
bool GetOutput_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject);

// vtkDataSet* GetOutput(int ARG_0)
// vtkDataSet* GetOutput(int)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_GetOutput_1")] public static extern 
bool GetOutput_1(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, int /*(int)*/ ARG_0);

// vtkDataObject* GetInput()
// vtkDataObject* GetInput()
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_GetInput_0")] public static extern 
bool GetInput_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject);

// vtkPolyData* GetPolyDataOutput()
// vtkPolyData *GetPolyDataOutput()
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_GetPolyDataOutput_0")] public static extern 
bool GetPolyDataOutput_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject);

// vtkStructuredPoints* GetStructuredPointsOutput()
// vtkStructuredPoints *GetStructuredPointsOutput()
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_GetStructuredPointsOutput_0")] public static extern 
bool GetStructuredPointsOutput_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject);

// vtkImageData* GetImageDataOutput()
// vtkImageData *GetImageDataOutput()
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_GetImageDataOutput_0")] public static extern 
bool GetImageDataOutput_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject);

// vtkStructuredGrid* GetStructuredGridOutput()
// vtkStructuredGrid *GetStructuredGridOutput()
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_GetStructuredGridOutput_0")] public static extern 
bool GetStructuredGridOutput_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject);

// vtkUnstructuredGrid* GetUnstructuredGridOutput()
// vtkUnstructuredGrid *GetUnstructuredGridOutput()
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_GetUnstructuredGridOutput_0")] public static extern 
bool GetUnstructuredGridOutput_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject);

// void SetInputData(vtkDataObject * ARG_0)
// void SetInputData(vtkDataObject*)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_SetInputData_0")] public static extern 
bool SetInputData_0(IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, IntPtr /*(vtkDataObject*)*/ ARG_0);

// void SetInputData(int ARG_0, vtkDataObject * ARG_1)
// void SetInputData(int, vtkDataObject*)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_SetInputData_1")] public static extern 
bool SetInputData_1(IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, int /*(int)*/ ARG_0, IntPtr /*(vtkDataObject*)*/ ARG_1);

// void SetInputData(vtkDataSet * ARG_0)
// void SetInputData(vtkDataSet*)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_SetInputData_2")] public static extern 
bool SetInputData_2(IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, IntPtr /*(vtkDataSet*)*/ ARG_0);

// void SetInputData(int ARG_0, vtkDataSet * ARG_1)
// void SetInputData(int, vtkDataSet*)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_SetInputData_3")] public static extern 
bool SetInputData_3(IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, int /*(int)*/ ARG_0, IntPtr /*(vtkDataSet*)*/ ARG_1);

// void AddInputData(vtkDataObject * ARG_0)
// void AddInputData(vtkDataObject *)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_AddInputData_0")] public static extern 
bool AddInputData_0(IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, IntPtr /*(vtkDataObject*)*/ ARG_0);

// void AddInputData(vtkDataSet * ARG_0)
// void AddInputData(vtkDataSet*)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_AddInputData_1")] public static extern 
bool AddInputData_1(IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, IntPtr /*(vtkDataSet*)*/ ARG_0);

// void AddInputData(int ARG_0, vtkDataSet * ARG_1)
// void AddInputData(int, vtkDataSet*)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_AddInputData_2")] public static extern 
bool AddInputData_2(IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, int /*(int)*/ ARG_0, IntPtr /*(vtkDataSet*)*/ ARG_1);

// void AddInputData(int ARG_0, vtkDataObject * ARG_1)
// void AddInputData(int, vtkDataObject*)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_AddInputData_3")] public static extern 
bool AddInputData_3(IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, int /*(int)*/ ARG_0, IntPtr /*(vtkDataObject*)*/ ARG_1);

// int ProcessRequest(vtkInformation * request, vtkInformationVector ** inputVector, vtkInformationVector * outputVector)
// int ProcessRequest(vtkInformation* request, vtkInformationVector** inputVector, vtkInformationVector* outputVector)
[DllImport("vtkplugin", EntryPoint="vtkDataSetAlgorithm_ProcessRequest_0")] public static extern 
bool ProcessRequest_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkDataSetAlgorithm*)*/ callingObject, IntPtr /*(vtkInformation*)*/ request, IntPtr /*(vtkInformationVector**)*/ inputVector, IntPtr /*(vtkInformationVector*)*/ outputVector);

}
};
