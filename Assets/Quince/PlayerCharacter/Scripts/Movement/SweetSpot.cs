using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetSpot : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;
    private PlayerMovementAdvanced pm;
    public Transform orientation;
    public Transform playerCam;

    [Header("Boosting")]
    public float boostForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is inside the sweet spot
        if (other.CompareTag("Sweet Spot") /*&& (Input.GetKeyDown(KeyCode.Space))*/)
        {
            // Detect jump input
                BoostingPlayer();    
        }
    }

    private void BoostingPlayer()
    {
        //pm.boosting = true;

        Transform forwardT;

        forwardT = playerCam;

        Vector3 direction = GetDirection(forwardT);

        // Calculate the force to apply
        Vector3 forceToApply = direction * boostForce;

        // Apply the force to the Rigidbody
        rb.AddForce(forceToApply, ForceMode.Impulse);  // Impulse for an instant boost

        Debug.Log("Boosting");
    }

    private void StopBoost()
    {
        //pm.boosting = false;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}
