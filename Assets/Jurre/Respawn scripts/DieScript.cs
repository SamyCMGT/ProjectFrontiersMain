using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        RespawnManager.Instance.Die();
    }
}
