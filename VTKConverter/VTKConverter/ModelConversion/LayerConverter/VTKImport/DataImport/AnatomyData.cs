using System;
using Kitware.VTK;

namespace VTKConverter.DataImport
{
    [Serializable()]
    class AnatomyData : ModelData
    {
        public AnatomyData(vtkDataSet vtkModel) : base(vtkModel)
        {
            LoadVertices(vtkModel);
            LoadIndices(vtkModel);
        }
    }
}
