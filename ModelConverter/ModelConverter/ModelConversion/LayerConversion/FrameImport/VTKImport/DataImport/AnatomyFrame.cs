using System;
using Kitware.VTK;

namespace ModelConversion.LayerConversion.FrameImport.VTKImport
{
    [Serializable()]
    class AnatomyFrame : VTKFrame
    {
        public AnatomyFrame(vtkDataSet vtkModel) : base(vtkModel)
        {
            LoadVertices(vtkModel);
            LoadIndices(vtkModel);
        }
    }
}
