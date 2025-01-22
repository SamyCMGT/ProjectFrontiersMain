using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.dashCount >= GameManager.Instance.maxDashes)
        {

        }
        else
        {
            GameManager.Instance.DashPickup();
            Destroy(gameObject);
        }
    }
}
