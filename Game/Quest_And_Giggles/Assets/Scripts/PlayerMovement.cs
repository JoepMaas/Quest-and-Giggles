using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsOwner) return;


        float horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector3(horizontal, 0f, 0f) * 5f;    

    }
}
