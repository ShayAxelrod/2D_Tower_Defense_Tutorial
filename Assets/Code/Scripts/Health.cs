using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    private bool isDestroyed = false;

    public void TakeDamage(int dmg) {
        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestroyed) {
            isDestroyed = true;
            EnemySpawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
            LevelManager.main.IncreaseCurrency(currencyWorth);
        }
    }
}
