using SO.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CYAN : EnemyController
{
    [SerializeField] protected FloatReference attackCooldown;
    [SerializeField] protected FloatReference attackDamage;
    [SerializeField] protected FloatReference radius;
    [SerializeField] protected FloatReference explodeMinDistance;
    [SerializeField] protected FloatReference explosionForce;
    [SerializeField] protected SphereCollider explosion;
    [SerializeField] protected LayerMask targetLayers;



    protected void HandleAttack()
    {
        if (agent.hasPath && agent.remainingDistance < explodeMinDistance)
        {
            agent.isStopped = true;

            StartCoroutine(Explosion());

            
        }
    }

    protected IEnumerator Explosion()
    {
        explosion.enabled = true;

        Collider[] results = new Collider[2];

        if (Physics.OverlapSphereNonAlloc(transform.position, radius, results, targetLayers) > 0)
        {
            Debug.Log(target.GetComponent<Health>().TakeDamage(-attackDamage));
        }

        while (explosion.radius < radius)
        {
            explosion.radius += Time.deltaTime * explosionForce;
            yield return null;
        }

        gameObject.SetActive(false);

    }

    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }

}
