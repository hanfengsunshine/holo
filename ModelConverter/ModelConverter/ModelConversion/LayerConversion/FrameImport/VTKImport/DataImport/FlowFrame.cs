using System;
using Kitware.VTK;

namespace ModelConversion.LayerConversion.FrameImport.VTKImport
{
    class FlowFrame : VTKFrame
    {
        private int numberOfVertices;

        public FlowFrame(vtkDataSet vtkModel) : base(vtkModel)
        {
            numberOfVertices = vtkModel.GetNumberOfPoints() / 2;
            GetLineVerticesAndVectors(vtkModel);
            ComputePointIndices(numberOfVertices);
            GetFlowColors(vtkModel);
        }

        private void GetLineVerticesAndVectors(vtkDataSet vtkModel)
        {
            Vertices = new double[numberOfVertices][];
            Vectors = new double[numberOfVertices][];
            int currentVertexNumber = 0;
            for (int i = 0; i < numberOfVertices * 2; i+=2)
            {
                Vertices[currentVertexNumber] = vtkModel.GetPoint(i);
                Vectors[currentVertexNumber] = vtkModel.GetPoint(i+1);
                currentVertexNumber += 1;
            }
        }

        private void GetFlowColors(vtkDataSet vtkModel)
        {
            // Kitware.VTK.dll automatically scales colours to 0-255 range.
            Scalars = new double[numberOfVertices][];
            vtkDataArray colors = vtkModel.GetCellData().GetScalars("Colors");
            for(int i = 0; i < numberOfVertices; i++)
            {
                Scalars[i] = colors.GetTuple3(i);
            }
        }
    }
}
