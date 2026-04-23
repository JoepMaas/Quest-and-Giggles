using Unity.Netcode;
using UnityEngine;

public class TestScript : NetworkBehaviour
{
    public GameObject lightObject;

    public NetworkVariable<bool> on = new NetworkVariable<bool>();

    public override void OnNetworkSpawn()
    {
        on.OnValueChanged += (oldValue, newValue) =>
        {
            lightObject.SetActive(newValue);
        };
    }


    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void ButtonPressedRpc()
    {
        on.Value = !on.Value;
    }
}