using SO.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAGENTA : EnemyController
{
    [SerializeField] protected FloatReference attackCooldown;
    [SerializeField] protected FloatReference attackDamage;
    [SerializeField] private BoolReference canMove;
    [SerializeField] protected FloatReference radius;
    [SerializeField] protected LayerMask targetLayers;

    private float currentAttackCooldown;

    public bool CanMove { get => canMove; set => canMove.SetValue(value); }

    protected override void HandleMovement()
    {
        base.HandleMovement();

        agent.isStopped = !canMove;

        if (!canMove)
            agent.velocity = Vector3.zero;
    }

    private void HandleAttack()
    {
        Collider[] results = new Collider[1];

        if (Physics.OverlapSphereNonAlloc(transform.position, radius, results, targetLayers) > 0 && currentAttackCooldown <= 0)
        {
            if (results[0].TryGetComponent(out Health health))
            {
                currentAttackCooldown = attackCooldown;
                Debug.Log(health.TakeDamage(-attackDamage));
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if(currentAttackCooldown > 0)
            currentAttackCooldown -= Time.deltaTime;

    }

    private void FixedUpdate()
    {
        HandleAttack();
    }
}
