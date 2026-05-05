using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraController : NetworkBehaviour
{
    public GameObject cameraHolder;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            cameraHolder.SetActive(true);
        }
        else
        {
            cameraHolder.SetActive(false);
        }
    }   
}
