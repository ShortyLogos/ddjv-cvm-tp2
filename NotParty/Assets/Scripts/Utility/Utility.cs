using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static IEnumerator WaitForRealSeconds(float time)
    {
        // Source: https://answers.unity.com/questions/301868/yield-waitforseconds-outside-of-timescale.html
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

}
