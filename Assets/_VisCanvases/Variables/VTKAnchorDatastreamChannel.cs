using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SculptingVis {
public class VTKAnchorDatastreamChannel : DatastreamChannel {

    public void Init(VTKDataset dataset) {
        _dataset = dataset;
    }

    VTKDataset  _dataset ;
    public VTK.vtkDataSet GetVTKDataSet() {
        return _dataset.GetVTKDataset();
    }
    public override int GetNumberOfElements() {
        return (int)_dataset.GetVTKDataset().GetNumberOfPoints();
    }
    
    public override int GetNumberOfComponents() {
        return 3;
    }
    
}
}