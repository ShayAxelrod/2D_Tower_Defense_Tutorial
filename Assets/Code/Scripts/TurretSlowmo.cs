using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class TurretSlowmo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] public float targetingRange = 3f;
    [SerializeField] private float aps = 4f; // Attacks Per Second
    [SerializeField] private float effectTime = 1f;

    private bool targetsInRange = false;
    private float timeUntilFire;
        

    private void OnDrawGizmosSelected() {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    // Update is called once per frame
    void Update() {
        if (!targetsInRange) {
            FindTargets();
            return;
        }
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire > 1f / aps) {
            Cast();
            timeUntilFire = 0f;
        }
    }

    private void Cast() {
        RaycastHit2D[] targetsInRange = FindTargets();
        for (int i = 0; i < targetsInRange.Length; ++i) {
            EnemyMovement em = targetsInRange[i].transform.GetComponent<EnemyMovement>();
            em.UpdateSpeed(0.5f);
            StartCoroutine(ResetEnemySpeed(em));
        }
    }
    
    private IEnumerator ResetEnemySpeed(EnemyMovement em) {
        yield return new WaitForSeconds(effectTime);
        em.ResetSpeed();
    }

    private RaycastHit2D[] FindTargets() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position,
            targetingRange, (Vector2)transform.position, 0f, enemyMask);
        targetsInRange = (hits.Length > 0) ? true : false;
        return hits;
    }
}
