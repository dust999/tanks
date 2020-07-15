using UnityEngine;

public class PlaceAtGround : MonoBehaviour
{
    private Vector3 newPosition;
    private void OnEnable()
    {
        newPosition = transform.position;
        newPosition.y = 0.1f;
        transform.position = newPosition;
    }
}
