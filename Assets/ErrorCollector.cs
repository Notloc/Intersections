using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ErrorCollector
{
    private List<Exception> errors = new List<Exception>();
    public int Count { get { return errors.Count; } }

    public void Add(Exception error)
    {
        errors.Add(error);
    }

    public void Clear()
    {
        errors.Clear();
    }

    public string GetErrorMessages()
    {
        string errorText = "";
        foreach (Exception e in errors)
            errorText += e.ToString() + "   \n";
        
        return errorText;
    }

    public void GoBoom()
    {
        if (errors.Count > 0)
        {
            throw new Exception(errors.Count + " errors encounter: " + GetErrorMessages());
        }
    }
}
