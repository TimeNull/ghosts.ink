using SO.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAGENTA : EnemyController
{
    [SerializeField] private BoolReference canMove;

    public bool CanMove { get => canMove; set => canMove.SetValue(value); }

    protected override void HandleMovement()
    {
        base.HandleMovement();

        agent.isStopped = !canMove;

        if (!canMove)
            agent.velocity = Vector3.zero;
    }

}
