using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static float getRandomGuassian(float mean, float std)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
        float randNormal = mean + std * randStdNormal;
        return randNormal;
    }
}
