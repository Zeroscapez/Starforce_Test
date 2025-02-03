using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        // Rotate the text to face the camera
        transform.LookAt(transform.position + mainCamera.forward);
    }
}