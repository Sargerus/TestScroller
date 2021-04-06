using UnityEngine;

public class CameraScript : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.up * 2f * Time.deltaTime, Space.World);
    }
}
