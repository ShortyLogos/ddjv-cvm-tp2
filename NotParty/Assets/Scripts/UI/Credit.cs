using System.Collections;
using UnityEngine;

public class Credit : MonoBehaviour
{
    [SerializeField]
    private float tickDelai;
    [SerializeField]
    private float difference = 500;

    private Vector3 originalPosition;
    private RectTransform body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<RectTransform>();
        originalPosition = body.position;
        body.SetPositionAndRotation(new Vector3(originalPosition.x, originalPosition.y - difference, originalPosition.z), Quaternion.identity);
        StartCoroutine(RollUp());
    }

    private IEnumerator RollUp()
    {
        while(difference>0)
        {
            difference--;
            body.SetPositionAndRotation(new Vector3(originalPosition.x, originalPosition.y - difference, originalPosition.z), Quaternion.identity);
            yield return new WaitForSecondsRealtime(tickDelai);
        }
    }

    
}
