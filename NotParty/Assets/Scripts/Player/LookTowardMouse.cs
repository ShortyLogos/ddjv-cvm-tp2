using UnityEngine;
using UnityEngine.Events;

// ref : https://www.youtube.com/watch?v=7c68z05vaX4

public class LookTowardMouse : MonoBehaviour
{
    [SerializeField]
    private float offsetRotation = 0;

    private bool gamePaused = false;
    private UnityAction<object> lorsPause;

    private void Awake()
    {
        lorsPause = new UnityAction<object>(LorsPause);
    }

    private void OnEnable()
    {
        EventManager.StartListening("LorsPause", lorsPause);
    }

    private void OnDisable()
    {
        EventManager.StopListening("LorsPause", lorsPause);
    }

    private void LorsPause(object data)
    {
        gamePaused ^= true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused)
        {
            Vector3 positionSouris = Input.mousePosition;
            positionSouris.z = 0.0f;
            Vector3 positionArme = Camera.main.WorldToScreenPoint(transform.position);

            positionSouris.x = positionSouris.x - positionArme.x;
            positionSouris.y = positionSouris.y - positionArme.y;

            float angle = Mathf.Atan2(positionSouris.y, positionSouris.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + offsetRotation));
        }
    }
}
