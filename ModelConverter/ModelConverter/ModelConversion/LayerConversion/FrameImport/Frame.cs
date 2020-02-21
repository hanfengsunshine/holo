using System;

namespace ModelConversion.LayerConversion.FrameImport
{
    [Serializable()]
    public class Frame
    {
        public string Filename { get; set; }
        public double[] BoundingBox { get; set; }
        public double[][] Vertices { get; set; }
        public int NumberOfFacetEdges { get; set; }
        public int[] Indices { get; set; }
        public double[][] Vectors { get; set; }
        public double[][] Scalars { get; set; }
    }
}
