using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject selectionContainer;
    public GameObject LobbyContainer;

    bool opened = false;

    private void Start()
    {
        LobbyContainer.SetActive(false);
    }

    public void ShowLobby()
    {
        opened = !opened;

        LobbyContainer.SetActive(opened);
        selectionContainer.SetActive(!opened);
    }

}
