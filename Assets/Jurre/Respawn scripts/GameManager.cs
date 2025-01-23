using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //other scripts can globally access via GameManager.Instance without needing a reference
    public static GameManager Instance { get; private set; }

    public GameObject Player;

    public Vector3 SavePosition;
    public float dashCount = 2;
    public float maxDashes = 99;

    public TextMeshProUGUI textMeshProUGUI;

    private RespawnManager respawnManager;

    private void Start()
    {
        respawnManager = FindObjectOfType<RespawnManager>();

        if (respawnManager == null)
        {
            Debug.LogError("RespawnManager not found in the scene!");
        }
        textMeshProUGUI.text = $"Dashes: {dashCount}";
    }

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

    public void UseDash()
    {
        if (dashCount <= -1)
        {
            dashCount = 0;
            textMeshProUGUI.text = $"Dashes: {dashCount}";
        }
        else
        {
            dashCount--;
            textMeshProUGUI.text = $"Dashes: {dashCount}";
        }
    }

    public void DashPickup()
    {
        if (dashCount < maxDashes)
        {
            dashCount++;
            textMeshProUGUI.text = $"Dashes: {dashCount}";
        }
    }

    public void Die()
    {
        Player.transform.position = SavePosition;
        respawnManager.RespawnAllPickups();
    }
}
