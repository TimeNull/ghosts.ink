using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //singleton

    private PlayerController playerController;

    private List<EnemyController> enemies;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void SpawnEnemy()
    {

    }
}
