using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyStartingHealth, enemyAttack, enemyAttackCoolDown;

    public float enemyHealth;

    public int playerCloseEnemy;

    void Start()
    {
        enemyHealth = enemyStartingHealth;
        playerCloseEnemy = 0;
    }

    void Update()
    {
        if (enemyHealth < 1)
        {
            UIController.Instance.SetGameOver("You Won");
            Destroy(gameObject);
        }
    }
}
