using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    //other scripts can globally access via GameManager.Instance without needing a reference
    public static RespawnManager Instance { get; private set; }

    public GameObject Player;

    public Vector3 SavePosition;

    private void Awake()
    {
        //ensure there's only one instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //add or remove 1 health, update the health UI and Go to lose scene if health is 0 or lower
    public void Die()
    {
        Player.transform.position = SavePosition;
    }
}
