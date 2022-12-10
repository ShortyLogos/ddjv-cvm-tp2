using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ref : https://www.youtube.com/watch?v=7c68z05vaX4

public class LookTowardMouse : MonoBehaviour
{
    public float vitesse = 5.0f;
    public float offsetRotation = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 positionSouris = Input.mousePosition;
        positionSouris.z = 0.0f;
        Vector3 positionArme = Camera.main.WorldToScreenPoint(transform.position);

        positionSouris.x = positionSouris.x - positionArme.x;
        positionSouris.y = positionSouris.y - positionArme.y;

        float angle = Mathf.Atan2(positionSouris.y, positionSouris.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + offsetRotation));

        //vector3 positioncible = camera.main.screentoworldpoint(input.mouseposition);
        //positioncible.z = 0.0f;
        //transform.position = vector3.movetowards(transform.position, positioncible, vitesse * time.deltatime);
    }
}
