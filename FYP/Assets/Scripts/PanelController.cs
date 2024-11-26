using UnityEngine;

public class PanelController : MonoBehaviour
{
    public Camera mainCamera;

    private void LateUpdate()
    {

        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;


        transform.position = new Vector3(cameraPosition.x, cameraPosition.y - 0.1f, cameraPosition.z) + cameraForward * (mainCamera.nearClipPlane + 0.25f);


        transform.LookAt(cameraPosition);
    }
}