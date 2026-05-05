using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Vector3 offset;

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

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            cameraHolder.transform.position = transform.position + offset;      
        }
    }
}
