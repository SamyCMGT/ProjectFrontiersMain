using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPickup : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private MeshRenderer meshRenderer;
    private bool shouldRespawn = false;

    private void Start()
    {
        // Cache components
        sphereCollider = GetComponent<SphereCollider>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Register with the RespawnManager
        FindObjectOfType<RespawnManager>().RegisterPickup(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.dashCount >= GameManager.Instance.maxDashes)
        {
            // Do nothing if dash count is maxed
        }
        else
        {
            GameManager.Instance.DashPickup();
            sphereCollider.enabled = false;
            meshRenderer.enabled = false;
            Debug.Log($"{gameObject.name}: Disabled the collider and mesh renderer");
        }
    }

    public void TriggerRespawn()
    {
        // Mark this pickup for respawn
        shouldRespawn = true;
    }

    private void Update()
    {
        if (shouldRespawn)
        {
            DoRespawn();
        }
    }

    private void DoRespawn()
    {
        sphereCollider.enabled = true;
        meshRenderer.enabled = true;
        shouldRespawn = false;
        Debug.Log($"{gameObject.name}: Enabled the collider and mesh renderer");
    }
}
