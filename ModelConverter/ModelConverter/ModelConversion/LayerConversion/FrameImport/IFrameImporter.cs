namespace ModelConversion.LayerConversion.FrameImport
{
    interface IFrameImporter
    {
        Frame Import(string inputPath, string outputRootDir, string dataType);
    }
}
