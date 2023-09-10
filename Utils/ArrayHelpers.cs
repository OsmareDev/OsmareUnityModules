using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArrayHelpers
{
    public static void CopyElements<T>(T[,] sourceArray, T[,] destinationArray)
    {
        int colsToCopy = Math.Min(sourceArray.GetLength(0), destinationArray.GetLength(0));
        int rowsToCopy = Math.Min(sourceArray.GetLength(1), destinationArray.GetLength(1));

        // Asegurarse de que ambos arrays no sean nulos
        if (sourceArray != null && destinationArray != null)
        {
            for (int i = 0; i < colsToCopy; ++i)
            {
                for (int j = 0; j < rowsToCopy; ++j) {
                    destinationArray[i,j] = sourceArray[i,j];
                }
            }
        }
    }
}
