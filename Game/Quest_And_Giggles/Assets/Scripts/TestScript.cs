using Unity.Netcode;
using UnityEngine;

public class TestScript : NetworkBehaviour
{
    [SerializeField] private GameObject image;
    private NetworkVariable<bool> opened = new NetworkVariable<bool>();
    // private NetworkVariable<List<int>> list = new NetworkVariable<List<int>>();

    public override void OnNetworkSpawn()
    {
        opened.OnValueChanged += OnOpenedChanged;
    }

    private void OnOpenedChanged(bool oldValue, bool newValue)
    {
        image.SetActive(newValue);
    }

    public void SpawnImage()
    {
        ToggleImageRpc();
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void ToggleImageRpc()
    {
        opened.Value = !opened.Value;
    }
}