using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("speed", 1);
        PlayerPrefs.SetFloat("efficiency", 1);
    }
}
