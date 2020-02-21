using Kitware.VTK;

namespace ModelConversion.LayerConversion.FrameImport.VTKImport
{
    abstract class VTKFrame
    {
        public double[] BoundingBox { get; protected set; }
        public double[][] Vertices { get; protected set; }
        public int NumberOfFacetEdges { get; protected set; }
        public int[] Indices { get; protected set; }
        public double[][] Vectors { get; protected set; } = null;
        public double[][] Scalars { get; protected set; } = null;

        public VTKFrame(vtkDataSet vtkModel)
        {
            LoadBounds(vtkModel);
        }

        private void LoadBounds(vtkDataSet vtkModel)
        {
            double[] boundingCoordinates = vtkModel.GetBounds();
            BoundingBox = new double[6] {boundingCoordinates[0], boundingCoordinates[2], -boundingCoordinates[4],
            boundingCoordinates[1], boundingCoordinates[3], -boundingCoordinates[5]};
        }

        protected void LoadVertices(vtkDataSet vtkModel)
        {
            int numberOfPoints = vtkModel.GetNumberOfPoints();
            Vertices = new double[numberOfPoints][];
            for (int i = 0; i < numberOfPoints; i++)
            {
                Vertices[i] = vtkModel.GetPoint(i);
                Vertices[i][2] = -Vertices[i][2];
            }
        }

        protected virtual void LoadIndices(vtkDataSet vtkModel)
        {
            int numberOfCells = vtkModel.GetNumberOfCells();
            NumberOfFacetEdges = vtkModel.GetMaxCellSize();
            Indices = new int[NumberOfFacetEdges * numberOfCells];
            int currentIndexNumber = 0;
            for (int i = 0; i < numberOfCells; i++)
            {
                currentIndexNumber = LoadCellIndices(currentIndexNumber, vtkModel.GetCell(i).GetPointIds());
            }
        }

        protected void ComputePointIndices(int numberOfPoints)
        {
            NumberOfFacetEdges = 3;
            Indices = new int[numberOfPoints * 3];
            int currentVertexNumber = 0;
            for (int i = 0; i < Indices.Length; i += 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    Indices[i + j] = currentVertexNumber;
                }
                currentVertexNumber++;
            }
        }

        private int LoadCellIndices(int currentIndexNumber, vtkIdList cellIndices)
        {
            int numberOfIndices = cellIndices.GetNumberOfIds();
            for (int j = 0; j < numberOfIndices; j++)
            {
                Indices[currentIndexNumber] = cellIndices.GetId(j);
                currentIndexNumber += 1;
            }
            return currentIndexNumber;
        }
    }
}
