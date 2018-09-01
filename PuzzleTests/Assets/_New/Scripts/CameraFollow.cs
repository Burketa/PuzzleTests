using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool shake = false, follow_player = true;
    public Transform player;

    public float smoothSpeed = 0.125f;
    [Range(0.1f, 1.1f)]
    public float smoothFactor = 1;
    public Vector3 offset;

    private Vector2 _desired_position, _smoothed_position;

    void FixedUpdate()
    {
        PositionCamera(Vector2.zero, follow_player);
    }

    public void PositionCamera(Vector2 pos, bool followPLayer)
    {
        if (followPLayer)
        {
            _desired_position = player.position + offset;
        }
        else
        {
            _desired_position = pos;
        }
        _smoothed_position = Vector2.Lerp(transform.position, _desired_position, smoothSpeed);
        transform.position = (Vector3)_smoothed_position + new Vector3(0, 0, -10);
        offset = new Vector2(Mathf.Clamp(-transform.position.x, -8, 8), Mathf.Clamp(-transform.position.y, -4, 4)) * smoothFactor;
    }
}