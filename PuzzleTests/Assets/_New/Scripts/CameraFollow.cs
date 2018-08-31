using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool shake = false;
    public Transform target;

    public float smoothSpeed = 0.125f;
    [Range(0.1f, 1.1f)]
    public float smoothFactor = 1;
    public Vector3 offset;

    private Transform background;

    private void Start()
    {
        //background = GameObject.FindGameObjectWithTag("Background").transform;
    }
    void FixedUpdate()
    {
        //if (shake)
            //offset = Random.insideUnitCircle;
        //else
            //offset = Vector2.zero;
        Vector2 desiredPosition = target.position + offset;
        Vector2 smoothedPosition = Vector2.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = (Vector3)smoothedPosition + new Vector3(0,0,-10);
        //background.position = new Vector2(Mathf.Clamp(-transform.position.x, -8, 8), Mathf.Clamp(-transform.position.y, -4, 4)) * smoothFactor;
        //background.position = -transform.position * smoothFactor;
        offset = new Vector2(Mathf.Clamp(-transform.position.x, -8, 8), Mathf.Clamp(-transform.position.y, -4, 4)) * smoothFactor;
    }

}