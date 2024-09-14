using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera mainCamera;

    private void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene. Make sure there is an active camera.");
        }
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            // Calculate the rotation to face the camera
            Quaternion lookRotation = Quaternion.LookRotation(mainCamera.transform.forward, mainCamera.transform.up);

            // Apply the rotation to the object
            transform.rotation = lookRotation;
        }
    }
}
