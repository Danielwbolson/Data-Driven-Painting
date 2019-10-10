
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace VTK_API {
class API_vtkMeshQuality {

// static vtkTypeBool IsTypeOf(const char * type)
// static vtkTypeBool IsTypeOf(const char *type)
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_IsTypeOf_0")] public static extern 
bool IsTypeOf_0(IntPtr /*(IntPtr*)*/ return_value, string /*(char*)*/ type);

// vtkTypeBool IsA(const char * type)
// vtkTypeBool IsA(const char *type)
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_IsA_0")] public static extern 
bool IsA_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkMeshQuality*)*/ callingObject, string /*(char*)*/ type);

// static vtkMeshQuality* SafeDownCast(vtkObjectBase * o)
// static vtkMeshQuality* SafeDownCast(vtkObjectBase *o)
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SafeDownCast_0")] public static extern 
bool SafeDownCast_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkObjectBase*)*/ o);

// vtkMeshQuality* NewInstance()
// vtkMeshQuality *NewInstance()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_NewInstance_0")] public static extern 
bool NewInstance_0(IntPtr /*(IntPtr**)*/ return_value, IntPtr /*(vtkMeshQuality*)*/ callingObject);

// static vtkMeshQuality* New()
// static vtkMeshQuality* New()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_New_0")] public static extern 
bool New_0(IntPtr /*(IntPtr**)*/ return_value);

// virtual void SetSaveCellQuality(int _arg)
// virtual void SetSaveCellQuality (int _arg)
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetSaveCellQuality_0")] public static extern 
bool SetSaveCellQuality_0(IntPtr /*(vtkMeshQuality*)*/ callingObject, int /*(int)*/ _arg);

// virtual int GetSaveCellQuality()
// virtual int GetSaveCellQuality ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_GetSaveCellQuality_0")] public static extern 
bool GetSaveCellQuality_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void SaveCellQualityOn()
// virtual void SaveCellQualityOn ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SaveCellQualityOn_0")] public static extern 
bool SaveCellQualityOn_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void SaveCellQualityOff()
// virtual void SaveCellQualityOff ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SaveCellQualityOff_0")] public static extern 
bool SaveCellQualityOff_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void SetTriangleQualityMeasure(int _arg)
// virtual void SetTriangleQualityMeasure (int _arg)
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasure_0")] public static extern 
bool SetTriangleQualityMeasure_0(IntPtr /*(vtkMeshQuality*)*/ callingObject, int /*(int)*/ _arg);

// virtual int GetTriangleQualityMeasure()
// virtual int GetTriangleQualityMeasure ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_GetTriangleQualityMeasure_0")] public static extern 
bool GetTriangleQualityMeasure_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToArea()
// void SetTriangleQualityMeasureToArea()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToArea_0")] public static extern 
bool SetTriangleQualityMeasureToArea_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToEdgeRatio()
// void SetTriangleQualityMeasureToEdgeRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToEdgeRatio_0")] public static extern 
bool SetTriangleQualityMeasureToEdgeRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToAspectRatio()
// void SetTriangleQualityMeasureToAspectRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToAspectRatio_0")] public static extern 
bool SetTriangleQualityMeasureToAspectRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToRadiusRatio()
// void SetTriangleQualityMeasureToRadiusRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToRadiusRatio_0")] public static extern 
bool SetTriangleQualityMeasureToRadiusRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToAspectFrobenius()
// void SetTriangleQualityMeasureToAspectFrobenius()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToAspectFrobenius_0")] public static extern 
bool SetTriangleQualityMeasureToAspectFrobenius_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToMinAngle()
// void SetTriangleQualityMeasureToMinAngle()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToMinAngle_0")] public static extern 
bool SetTriangleQualityMeasureToMinAngle_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToMaxAngle()
// void SetTriangleQualityMeasureToMaxAngle()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToMaxAngle_0")] public static extern 
bool SetTriangleQualityMeasureToMaxAngle_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToCondition()
// void SetTriangleQualityMeasureToCondition()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToCondition_0")] public static extern 
bool SetTriangleQualityMeasureToCondition_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToScaledJacobian()
// void SetTriangleQualityMeasureToScaledJacobian()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToScaledJacobian_0")] public static extern 
bool SetTriangleQualityMeasureToScaledJacobian_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToRelativeSizeSquared()
// void SetTriangleQualityMeasureToRelativeSizeSquared()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToRelativeSizeSquared_0")] public static extern 
bool SetTriangleQualityMeasureToRelativeSizeSquared_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToShape()
// void SetTriangleQualityMeasureToShape()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToShape_0")] public static extern 
bool SetTriangleQualityMeasureToShape_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToShapeAndSize()
// void SetTriangleQualityMeasureToShapeAndSize()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToShapeAndSize_0")] public static extern 
bool SetTriangleQualityMeasureToShapeAndSize_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTriangleQualityMeasureToDistortion()
// void SetTriangleQualityMeasureToDistortion()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTriangleQualityMeasureToDistortion_0")] public static extern 
bool SetTriangleQualityMeasureToDistortion_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void SetQuadQualityMeasure(int _arg)
// virtual void SetQuadQualityMeasure (int _arg)
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasure_0")] public static extern 
bool SetQuadQualityMeasure_0(IntPtr /*(vtkMeshQuality*)*/ callingObject, int /*(int)*/ _arg);

// virtual int GetQuadQualityMeasure()
// virtual int GetQuadQualityMeasure ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_GetQuadQualityMeasure_0")] public static extern 
bool GetQuadQualityMeasure_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToEdgeRatio()
// void SetQuadQualityMeasureToEdgeRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToEdgeRatio_0")] public static extern 
bool SetQuadQualityMeasureToEdgeRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToAspectRatio()
// void SetQuadQualityMeasureToAspectRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToAspectRatio_0")] public static extern 
bool SetQuadQualityMeasureToAspectRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToRadiusRatio()
// void SetQuadQualityMeasureToRadiusRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToRadiusRatio_0")] public static extern 
bool SetQuadQualityMeasureToRadiusRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToMedAspectFrobenius()
// void SetQuadQualityMeasureToMedAspectFrobenius()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToMedAspectFrobenius_0")] public static extern 
bool SetQuadQualityMeasureToMedAspectFrobenius_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToMaxAspectFrobenius()
// void SetQuadQualityMeasureToMaxAspectFrobenius()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToMaxAspectFrobenius_0")] public static extern 
bool SetQuadQualityMeasureToMaxAspectFrobenius_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToMaxEdgeRatios()
// void SetQuadQualityMeasureToMaxEdgeRatios()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToMaxEdgeRatios_0")] public static extern 
bool SetQuadQualityMeasureToMaxEdgeRatios_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToSkew()
// void SetQuadQualityMeasureToSkew()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToSkew_0")] public static extern 
bool SetQuadQualityMeasureToSkew_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToTaper()
// void SetQuadQualityMeasureToTaper()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToTaper_0")] public static extern 
bool SetQuadQualityMeasureToTaper_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToWarpage()
// void SetQuadQualityMeasureToWarpage()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToWarpage_0")] public static extern 
bool SetQuadQualityMeasureToWarpage_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToArea()
// void SetQuadQualityMeasureToArea()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToArea_0")] public static extern 
bool SetQuadQualityMeasureToArea_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToStretch()
// void SetQuadQualityMeasureToStretch()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToStretch_0")] public static extern 
bool SetQuadQualityMeasureToStretch_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToMinAngle()
// void SetQuadQualityMeasureToMinAngle()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToMinAngle_0")] public static extern 
bool SetQuadQualityMeasureToMinAngle_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToMaxAngle()
// void SetQuadQualityMeasureToMaxAngle()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToMaxAngle_0")] public static extern 
bool SetQuadQualityMeasureToMaxAngle_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToOddy()
// void SetQuadQualityMeasureToOddy()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToOddy_0")] public static extern 
bool SetQuadQualityMeasureToOddy_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToCondition()
// void SetQuadQualityMeasureToCondition()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToCondition_0")] public static extern 
bool SetQuadQualityMeasureToCondition_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToJacobian()
// void SetQuadQualityMeasureToJacobian()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToJacobian_0")] public static extern 
bool SetQuadQualityMeasureToJacobian_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToScaledJacobian()
// void SetQuadQualityMeasureToScaledJacobian()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToScaledJacobian_0")] public static extern 
bool SetQuadQualityMeasureToScaledJacobian_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToShear()
// void SetQuadQualityMeasureToShear()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToShear_0")] public static extern 
bool SetQuadQualityMeasureToShear_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToShape()
// void SetQuadQualityMeasureToShape()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToShape_0")] public static extern 
bool SetQuadQualityMeasureToShape_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToRelativeSizeSquared()
// void SetQuadQualityMeasureToRelativeSizeSquared()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToRelativeSizeSquared_0")] public static extern 
bool SetQuadQualityMeasureToRelativeSizeSquared_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToShapeAndSize()
// void SetQuadQualityMeasureToShapeAndSize()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToShapeAndSize_0")] public static extern 
bool SetQuadQualityMeasureToShapeAndSize_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToShearAndSize()
// void SetQuadQualityMeasureToShearAndSize()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToShearAndSize_0")] public static extern 
bool SetQuadQualityMeasureToShearAndSize_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetQuadQualityMeasureToDistortion()
// void SetQuadQualityMeasureToDistortion()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetQuadQualityMeasureToDistortion_0")] public static extern 
bool SetQuadQualityMeasureToDistortion_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void SetTetQualityMeasure(int _arg)
// virtual void SetTetQualityMeasure (int _arg)
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasure_0")] public static extern 
bool SetTetQualityMeasure_0(IntPtr /*(vtkMeshQuality*)*/ callingObject, int /*(int)*/ _arg);

// virtual int GetTetQualityMeasure()
// virtual int GetTetQualityMeasure ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_GetTetQualityMeasure_0")] public static extern 
bool GetTetQualityMeasure_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToEdgeRatio()
// void SetTetQualityMeasureToEdgeRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToEdgeRatio_0")] public static extern 
bool SetTetQualityMeasureToEdgeRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToAspectRatio()
// void SetTetQualityMeasureToAspectRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToAspectRatio_0")] public static extern 
bool SetTetQualityMeasureToAspectRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToRadiusRatio()
// void SetTetQualityMeasureToRadiusRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToRadiusRatio_0")] public static extern 
bool SetTetQualityMeasureToRadiusRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToAspectFrobenius()
// void SetTetQualityMeasureToAspectFrobenius()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToAspectFrobenius_0")] public static extern 
bool SetTetQualityMeasureToAspectFrobenius_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToMinAngle()
// void SetTetQualityMeasureToMinAngle()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToMinAngle_0")] public static extern 
bool SetTetQualityMeasureToMinAngle_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToCollapseRatio()
// void SetTetQualityMeasureToCollapseRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToCollapseRatio_0")] public static extern 
bool SetTetQualityMeasureToCollapseRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToAspectBeta()
// void SetTetQualityMeasureToAspectBeta()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToAspectBeta_0")] public static extern 
bool SetTetQualityMeasureToAspectBeta_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToAspectGamma()
// void SetTetQualityMeasureToAspectGamma()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToAspectGamma_0")] public static extern 
bool SetTetQualityMeasureToAspectGamma_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToVolume()
// void SetTetQualityMeasureToVolume()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToVolume_0")] public static extern 
bool SetTetQualityMeasureToVolume_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToCondition()
// void SetTetQualityMeasureToCondition()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToCondition_0")] public static extern 
bool SetTetQualityMeasureToCondition_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToJacobian()
// void SetTetQualityMeasureToJacobian()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToJacobian_0")] public static extern 
bool SetTetQualityMeasureToJacobian_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToScaledJacobian()
// void SetTetQualityMeasureToScaledJacobian()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToScaledJacobian_0")] public static extern 
bool SetTetQualityMeasureToScaledJacobian_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToShape()
// void SetTetQualityMeasureToShape()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToShape_0")] public static extern 
bool SetTetQualityMeasureToShape_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToRelativeSizeSquared()
// void SetTetQualityMeasureToRelativeSizeSquared()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToRelativeSizeSquared_0")] public static extern 
bool SetTetQualityMeasureToRelativeSizeSquared_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToShapeAndSize()
// void SetTetQualityMeasureToShapeAndSize()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToShapeAndSize_0")] public static extern 
bool SetTetQualityMeasureToShapeAndSize_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetTetQualityMeasureToDistortion()
// void SetTetQualityMeasureToDistortion()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetTetQualityMeasureToDistortion_0")] public static extern 
bool SetTetQualityMeasureToDistortion_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void SetHexQualityMeasure(int _arg)
// virtual void SetHexQualityMeasure (int _arg)
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasure_0")] public static extern 
bool SetHexQualityMeasure_0(IntPtr /*(vtkMeshQuality*)*/ callingObject, int /*(int)*/ _arg);

// virtual int GetHexQualityMeasure()
// virtual int GetHexQualityMeasure ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_GetHexQualityMeasure_0")] public static extern 
bool GetHexQualityMeasure_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToEdgeRatio()
// void SetHexQualityMeasureToEdgeRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToEdgeRatio_0")] public static extern 
bool SetHexQualityMeasureToEdgeRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToMedAspectFrobenius()
// void SetHexQualityMeasureToMedAspectFrobenius()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToMedAspectFrobenius_0")] public static extern 
bool SetHexQualityMeasureToMedAspectFrobenius_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToMaxAspectFrobenius()
// void SetHexQualityMeasureToMaxAspectFrobenius()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToMaxAspectFrobenius_0")] public static extern 
bool SetHexQualityMeasureToMaxAspectFrobenius_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToMaxEdgeRatios()
// void SetHexQualityMeasureToMaxEdgeRatios()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToMaxEdgeRatios_0")] public static extern 
bool SetHexQualityMeasureToMaxEdgeRatios_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToSkew()
// void SetHexQualityMeasureToSkew()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToSkew_0")] public static extern 
bool SetHexQualityMeasureToSkew_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToTaper()
// void SetHexQualityMeasureToTaper()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToTaper_0")] public static extern 
bool SetHexQualityMeasureToTaper_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToVolume()
// void SetHexQualityMeasureToVolume()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToVolume_0")] public static extern 
bool SetHexQualityMeasureToVolume_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToStretch()
// void SetHexQualityMeasureToStretch()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToStretch_0")] public static extern 
bool SetHexQualityMeasureToStretch_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToDiagonal()
// void SetHexQualityMeasureToDiagonal()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToDiagonal_0")] public static extern 
bool SetHexQualityMeasureToDiagonal_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToDimension()
// void SetHexQualityMeasureToDimension()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToDimension_0")] public static extern 
bool SetHexQualityMeasureToDimension_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToOddy()
// void SetHexQualityMeasureToOddy()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToOddy_0")] public static extern 
bool SetHexQualityMeasureToOddy_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToCondition()
// void SetHexQualityMeasureToCondition()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToCondition_0")] public static extern 
bool SetHexQualityMeasureToCondition_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToJacobian()
// void SetHexQualityMeasureToJacobian()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToJacobian_0")] public static extern 
bool SetHexQualityMeasureToJacobian_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToScaledJacobian()
// void SetHexQualityMeasureToScaledJacobian()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToScaledJacobian_0")] public static extern 
bool SetHexQualityMeasureToScaledJacobian_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToShear()
// void SetHexQualityMeasureToShear()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToShear_0")] public static extern 
bool SetHexQualityMeasureToShear_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToShape()
// void SetHexQualityMeasureToShape()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToShape_0")] public static extern 
bool SetHexQualityMeasureToShape_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToRelativeSizeSquared()
// void SetHexQualityMeasureToRelativeSizeSquared()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToRelativeSizeSquared_0")] public static extern 
bool SetHexQualityMeasureToRelativeSizeSquared_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToShapeAndSize()
// void SetHexQualityMeasureToShapeAndSize()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToShapeAndSize_0")] public static extern 
bool SetHexQualityMeasureToShapeAndSize_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToShearAndSize()
// void SetHexQualityMeasureToShearAndSize()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToShearAndSize_0")] public static extern 
bool SetHexQualityMeasureToShearAndSize_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// void SetHexQualityMeasureToDistortion()
// void SetHexQualityMeasureToDistortion()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetHexQualityMeasureToDistortion_0")] public static extern 
bool SetHexQualityMeasureToDistortion_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// static double TriangleArea(vtkCell * cell)
// static double TriangleArea( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleArea_0")] public static extern 
bool TriangleArea_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleEdgeRatio(vtkCell * cell)
// static double TriangleEdgeRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleEdgeRatio_0")] public static extern 
bool TriangleEdgeRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleAspectRatio(vtkCell * cell)
// static double TriangleAspectRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleAspectRatio_0")] public static extern 
bool TriangleAspectRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleRadiusRatio(vtkCell * cell)
// static double TriangleRadiusRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleRadiusRatio_0")] public static extern 
bool TriangleRadiusRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleAspectFrobenius(vtkCell * cell)
// static double TriangleAspectFrobenius( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleAspectFrobenius_0")] public static extern 
bool TriangleAspectFrobenius_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleMinAngle(vtkCell * cell)
// static double TriangleMinAngle( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleMinAngle_0")] public static extern 
bool TriangleMinAngle_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleMaxAngle(vtkCell * cell)
// static double TriangleMaxAngle( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleMaxAngle_0")] public static extern 
bool TriangleMaxAngle_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleCondition(vtkCell * cell)
// static double TriangleCondition( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleCondition_0")] public static extern 
bool TriangleCondition_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleScaledJacobian(vtkCell * cell)
// static double TriangleScaledJacobian( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleScaledJacobian_0")] public static extern 
bool TriangleScaledJacobian_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleRelativeSizeSquared(vtkCell * cell)
// static double TriangleRelativeSizeSquared( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleRelativeSizeSquared_0")] public static extern 
bool TriangleRelativeSizeSquared_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleShape(vtkCell * cell)
// static double TriangleShape( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleShape_0")] public static extern 
bool TriangleShape_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleShapeAndSize(vtkCell * cell)
// static double TriangleShapeAndSize( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleShapeAndSize_0")] public static extern 
bool TriangleShapeAndSize_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TriangleDistortion(vtkCell * cell)
// static double TriangleDistortion( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TriangleDistortion_0")] public static extern 
bool TriangleDistortion_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadEdgeRatio(vtkCell * cell)
// static double QuadEdgeRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadEdgeRatio_0")] public static extern 
bool QuadEdgeRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadAspectRatio(vtkCell * cell)
// static double QuadAspectRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadAspectRatio_0")] public static extern 
bool QuadAspectRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadRadiusRatio(vtkCell * cell)
// static double QuadRadiusRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadRadiusRatio_0")] public static extern 
bool QuadRadiusRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadMedAspectFrobenius(vtkCell * cell)
// static double QuadMedAspectFrobenius( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadMedAspectFrobenius_0")] public static extern 
bool QuadMedAspectFrobenius_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadMaxAspectFrobenius(vtkCell * cell)
// static double QuadMaxAspectFrobenius( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadMaxAspectFrobenius_0")] public static extern 
bool QuadMaxAspectFrobenius_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadMinAngle(vtkCell * cell)
// static double QuadMinAngle( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadMinAngle_0")] public static extern 
bool QuadMinAngle_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadMaxEdgeRatios(vtkCell * cell)
// static double QuadMaxEdgeRatios( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadMaxEdgeRatios_0")] public static extern 
bool QuadMaxEdgeRatios_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadSkew(vtkCell * cell)
// static double QuadSkew( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadSkew_0")] public static extern 
bool QuadSkew_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadTaper(vtkCell * cell)
// static double QuadTaper( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadTaper_0")] public static extern 
bool QuadTaper_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadWarpage(vtkCell * cell)
// static double QuadWarpage( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadWarpage_0")] public static extern 
bool QuadWarpage_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadArea(vtkCell * cell)
// static double QuadArea( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadArea_0")] public static extern 
bool QuadArea_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadStretch(vtkCell * cell)
// static double QuadStretch( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadStretch_0")] public static extern 
bool QuadStretch_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadMaxAngle(vtkCell * cell)
// static double QuadMaxAngle( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadMaxAngle_0")] public static extern 
bool QuadMaxAngle_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadOddy(vtkCell * cell)
// static double QuadOddy( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadOddy_0")] public static extern 
bool QuadOddy_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadCondition(vtkCell * cell)
// static double QuadCondition( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadCondition_0")] public static extern 
bool QuadCondition_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadJacobian(vtkCell * cell)
// static double QuadJacobian( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadJacobian_0")] public static extern 
bool QuadJacobian_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadScaledJacobian(vtkCell * cell)
// static double QuadScaledJacobian( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadScaledJacobian_0")] public static extern 
bool QuadScaledJacobian_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadShear(vtkCell * cell)
// static double QuadShear( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadShear_0")] public static extern 
bool QuadShear_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadShape(vtkCell * cell)
// static double QuadShape( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadShape_0")] public static extern 
bool QuadShape_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadRelativeSizeSquared(vtkCell * cell)
// static double QuadRelativeSizeSquared( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadRelativeSizeSquared_0")] public static extern 
bool QuadRelativeSizeSquared_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadShapeAndSize(vtkCell * cell)
// static double QuadShapeAndSize( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadShapeAndSize_0")] public static extern 
bool QuadShapeAndSize_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadShearAndSize(vtkCell * cell)
// static double QuadShearAndSize( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadShearAndSize_0")] public static extern 
bool QuadShearAndSize_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double QuadDistortion(vtkCell * cell)
// static double QuadDistortion( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_QuadDistortion_0")] public static extern 
bool QuadDistortion_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetEdgeRatio(vtkCell * cell)
// static double TetEdgeRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetEdgeRatio_0")] public static extern 
bool TetEdgeRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetAspectRatio(vtkCell * cell)
// static double TetAspectRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetAspectRatio_0")] public static extern 
bool TetAspectRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetRadiusRatio(vtkCell * cell)
// static double TetRadiusRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetRadiusRatio_0")] public static extern 
bool TetRadiusRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetAspectFrobenius(vtkCell * cell)
// static double TetAspectFrobenius( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetAspectFrobenius_0")] public static extern 
bool TetAspectFrobenius_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetMinAngle(vtkCell * cell)
// static double TetMinAngle( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetMinAngle_0")] public static extern 
bool TetMinAngle_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetCollapseRatio(vtkCell * cell)
// static double TetCollapseRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetCollapseRatio_0")] public static extern 
bool TetCollapseRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetAspectBeta(vtkCell * cell)
// static double TetAspectBeta( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetAspectBeta_0")] public static extern 
bool TetAspectBeta_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetAspectGamma(vtkCell * cell)
// static double TetAspectGamma( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetAspectGamma_0")] public static extern 
bool TetAspectGamma_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetVolume(vtkCell * cell)
// static double TetVolume( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetVolume_0")] public static extern 
bool TetVolume_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetCondition(vtkCell * cell)
// static double TetCondition( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetCondition_0")] public static extern 
bool TetCondition_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetJacobian(vtkCell * cell)
// static double TetJacobian( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetJacobian_0")] public static extern 
bool TetJacobian_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetScaledJacobian(vtkCell * cell)
// static double TetScaledJacobian( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetScaledJacobian_0")] public static extern 
bool TetScaledJacobian_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetShape(vtkCell * cell)
// static double TetShape( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetShape_0")] public static extern 
bool TetShape_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetRelativeSizeSquared(vtkCell * cell)
// static double TetRelativeSizeSquared( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetRelativeSizeSquared_0")] public static extern 
bool TetRelativeSizeSquared_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetShapeandSize(vtkCell * cell)
// static double TetShapeandSize( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetShapeandSize_0")] public static extern 
bool TetShapeandSize_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double TetDistortion(vtkCell * cell)
// static double TetDistortion( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_TetDistortion_0")] public static extern 
bool TetDistortion_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexEdgeRatio(vtkCell * cell)
// static double HexEdgeRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexEdgeRatio_0")] public static extern 
bool HexEdgeRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexMedAspectFrobenius(vtkCell * cell)
// static double HexMedAspectFrobenius( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexMedAspectFrobenius_0")] public static extern 
bool HexMedAspectFrobenius_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexMaxAspectFrobenius(vtkCell * cell)
// static double HexMaxAspectFrobenius( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexMaxAspectFrobenius_0")] public static extern 
bool HexMaxAspectFrobenius_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexMaxEdgeRatio(vtkCell * cell)
// static double HexMaxEdgeRatio( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexMaxEdgeRatio_0")] public static extern 
bool HexMaxEdgeRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexSkew(vtkCell * cell)
// static double HexSkew( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexSkew_0")] public static extern 
bool HexSkew_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexTaper(vtkCell * cell)
// static double HexTaper( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexTaper_0")] public static extern 
bool HexTaper_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexVolume(vtkCell * cell)
// static double HexVolume( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexVolume_0")] public static extern 
bool HexVolume_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexStretch(vtkCell * cell)
// static double HexStretch( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexStretch_0")] public static extern 
bool HexStretch_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexDiagonal(vtkCell * cell)
// static double HexDiagonal( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexDiagonal_0")] public static extern 
bool HexDiagonal_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexDimension(vtkCell * cell)
// static double HexDimension( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexDimension_0")] public static extern 
bool HexDimension_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexOddy(vtkCell * cell)
// static double HexOddy( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexOddy_0")] public static extern 
bool HexOddy_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexCondition(vtkCell * cell)
// static double HexCondition( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexCondition_0")] public static extern 
bool HexCondition_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexJacobian(vtkCell * cell)
// static double HexJacobian( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexJacobian_0")] public static extern 
bool HexJacobian_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexScaledJacobian(vtkCell * cell)
// static double HexScaledJacobian( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexScaledJacobian_0")] public static extern 
bool HexScaledJacobian_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexShear(vtkCell * cell)
// static double HexShear( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexShear_0")] public static extern 
bool HexShear_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexShape(vtkCell * cell)
// static double HexShape( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexShape_0")] public static extern 
bool HexShape_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexRelativeSizeSquared(vtkCell * cell)
// static double HexRelativeSizeSquared( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexRelativeSizeSquared_0")] public static extern 
bool HexRelativeSizeSquared_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexShapeAndSize(vtkCell * cell)
// static double HexShapeAndSize( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexShapeAndSize_0")] public static extern 
bool HexShapeAndSize_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexShearAndSize(vtkCell * cell)
// static double HexShearAndSize( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexShearAndSize_0")] public static extern 
bool HexShearAndSize_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// static double HexDistortion(vtkCell * cell)
// static double HexDistortion( vtkCell* cell )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_HexDistortion_0")] public static extern 
bool HexDistortion_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkCell*)*/ cell);

// virtual void SetRatio(int r)
// virtual void SetRatio( int r )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetRatio_0")] public static extern 
bool SetRatio_0(IntPtr /*(vtkMeshQuality*)*/ callingObject, int /*(int)*/ r);

// int GetRatio()
// int GetRatio()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_GetRatio_0")] public static extern 
bool GetRatio_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void RatioOn()
// virtual void RatioOn ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_RatioOn_0")] public static extern 
bool RatioOn_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void RatioOff()
// virtual void RatioOff ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_RatioOff_0")] public static extern 
bool RatioOff_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void SetVolume(int cv)
// virtual void SetVolume( int cv )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetVolume_0")] public static extern 
bool SetVolume_0(IntPtr /*(vtkMeshQuality*)*/ callingObject, int /*(int)*/ cv);

// int GetVolume()
// int GetVolume()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_GetVolume_0")] public static extern 
bool GetVolume_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void VolumeOn()
// virtual void VolumeOn ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_VolumeOn_0")] public static extern 
bool VolumeOn_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void VolumeOff()
// virtual void VolumeOff ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_VolumeOff_0")] public static extern 
bool VolumeOff_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void SetCompatibilityMode(int cm)
// virtual void SetCompatibilityMode( int cm )
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_SetCompatibilityMode_0")] public static extern 
bool SetCompatibilityMode_0(IntPtr /*(vtkMeshQuality*)*/ callingObject, int /*(int)*/ cm);

// virtual int GetCompatibilityMode()
// virtual int GetCompatibilityMode ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_GetCompatibilityMode_0")] public static extern 
bool GetCompatibilityMode_0(IntPtr /*(IntPtr*)*/ return_value, IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void CompatibilityModeOn()
// virtual void CompatibilityModeOn ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_CompatibilityModeOn_0")] public static extern 
bool CompatibilityModeOn_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

// virtual void CompatibilityModeOff()
// virtual void CompatibilityModeOff ()
[DllImport("vtkplugin", EntryPoint="vtkMeshQuality_CompatibilityModeOff_0")] public static extern 
bool CompatibilityModeOff_0(IntPtr /*(vtkMeshQuality*)*/ callingObject);

}
};
