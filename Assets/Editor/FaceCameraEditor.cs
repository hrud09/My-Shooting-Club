using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FaceCamera))]
public class FaceCameraEditor : Editor
{
    private void OnSceneGUI()
    {
        FaceCamera faceCamera = (FaceCamera)target;

        if (faceCamera.mainCamera != null)
        {
            // Calculate the direction from the object to the camera
            Vector3 lookDirection = faceCamera.mainCamera.transform.position - faceCamera.transform.position;

            // Ensure the object stays upright (optional)
            lookDirection.y = 0;

            // Rotate the object to face the camera in the editor
            faceCamera.transform.rotation = Quaternion.LookRotation(lookDirection);
        }
        else
        {
            faceCamera.mainCamera = Camera.main;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
