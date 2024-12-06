using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockchainEffect : MonoBehaviour
{
    public static BlockchainEffect Instance { get; private set; }

    public int coins = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
