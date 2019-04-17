﻿using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FileImporter
{
    private STLImporter sTLImporter;
    private VTKImporter vtkImporter;
    private string fileExtension;

    public Vector3[] BaseVertices { get; private set; }
    public Vector3[] Vertices { get; private set; }
    public Vector3[] Normals { get; private set; }
    public int[] Indices { get; private set; }

    //Getting format-specific FileImporter (only STL and VTK for now)
    public FileImporter(string extension)
    {
        fileExtension = extension;
        if (extension == ".stl")
            sTLImporter = new STLImporter();
        else if (extension == ".vtk")
        {
            vtkImporter = new VTKImporter();
        }
        else
        {
            EditorUtility.ClearProgressBar();
            throw new Exception("Type not supported!");
        }
    }

    public void LoadFile(string filePath, bool firstMesh)
    {
        switch (fileExtension)
        {
            case ".stl":
                LoadStlFile(filePath);
                break;
            case ".vtk":
                LoadVtkFile(filePath);
                break;
        }
        if (firstMesh)
            BaseVertices = new Vector3[Vertices.Length];
    }

    private void LoadStlFile(string filePath)
    {
        sTLImporter.LoadFile(filePath);
        Vertices = sTLImporter.Vertices;
        Indices = sTLImporter.Indices;
        Normals = sTLImporter.Normals;
    }

    private void LoadVtkFile(string filePath)
    {
        vtkImporter.LoadFile(filePath);
        Vertices = vtkImporter.Vertices;
        Indices = vtkImporter.Indices;
        Normals = vtkImporter.Normals;
    }
}
