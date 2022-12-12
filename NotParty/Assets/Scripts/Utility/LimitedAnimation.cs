using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedAnimation : MonoBehaviour
{
    public void onEndAnimation()
    {
        Destroy(this.gameObject);
    }
}
