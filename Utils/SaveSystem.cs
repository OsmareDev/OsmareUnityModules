using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    public static void Save<T>(string filename, T data)
    {
        using (FileStream fileStream = new FileStream(filename, FileMode.Create))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, data);
        }
    }

    public static bool Load<T>(string filename, ref T data)
    {
        if (File.Exists(filename))
        {
            using (FileStream fileStream = new FileStream(filename, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                data = (T)binaryFormatter.Deserialize(fileStream);
                return true;
            }
        }
        else
        {
            Debug.LogWarning("File not found: " + filename);
            return false;
        }
    }
}
