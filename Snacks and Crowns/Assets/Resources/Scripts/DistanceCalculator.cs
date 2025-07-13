using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// class for calculating distances between vectors
public class DistanceCalculator
{
    public static double CalculateDistance(Vector3 a, Vector3 b)
    {
        return (a - b).magnitude;
    }
}
