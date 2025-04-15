using UnityEngine;

public class PanelController : MonoBehaviour
{
    public Camera mainCamera;
    public float offset=1;
   
    private void LateUpdate()
    {
        if(mainCamera == null)
        mainCamera = Camera.main;

        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;


        transform.position = new Vector3(cameraPosition.x, cameraPosition.y - 0.1f, cameraPosition.z) + cameraForward * (mainCamera.nearClipPlane + 0.25f)*offset;


        transform.LookAt(cameraPosition);
    }
}