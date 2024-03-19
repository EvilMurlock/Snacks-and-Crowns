using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtentions
{
    public static int GetIndex<T>(this T[] array,T element)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(element)) return i;
        }
        return -1;
    }
}
