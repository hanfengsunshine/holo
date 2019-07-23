using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/* Classes that contain information about a model, usually deserialized from ModelInfo.json. */

namespace ModelImport
{
    // Information about a whole model, usually deserialized from ModelInfo.json.
    public class ModelInfo
    {
	    public string Caption;
	    public List<ModelLayerInfo> Layers = new List<ModelLayerInfo>();
	}

    // Information about a single layer.
    public class ModelLayerInfo
    {
	    public string Caption;
		public bool Simulation;
		// Directory with VTK models inside.
		// Initially (when deserialized) this is relative to ModelInfo.json,
		// but will be converted to an absolute path in the process.
		public string Directory;
    }
}