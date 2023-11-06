using SO.Variables;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Rigidbody rb;

    [SerializeField] protected Transform target;

    [SerializeField] protected Transform player;

    protected IObjectPool<EnemyController> enemyPool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<EnemyController> EnemyPool { set => enemyPool = value; }

    public virtual void SetupEnemy(Vector3 startPos, Transform target, Transform player)
    {
        this.player = player;
        this.target = target;
        agent.isStopped = false;
        agent.Warp(startPos);
        gameObject.SetActive(true);
    }

    public virtual void SetupEnemy(Vector3 startPos, Transform player)
    {
        this.player = player;
        this.target = player;
        agent.isStopped = false;
        agent.Warp(startPos);
        gameObject.SetActive(true);
    }

    protected virtual void HandleMovement()
    {
        agent.SetDestination(target.position);
    }

    
    protected virtual void Update()
    {
        HandleMovement();
    }

    public void Deactivate()
    {

        // reset the moving Rigidbody
        agent.velocity = Vector3.zero;
        agent.isStopped = false;
        agent.ResetPath();
        agent.Warp(Vector3.zero);

        // release the projectile back to the pool
        enemyPool.Release(this);
    }

    private void OnEnable()
    {
        PlayerController.playerDied += Stop;
    }

    private void OnDisable()
    {
        PlayerController.playerDied -= Stop;
    }

    private void Stop()
    {
        Deactivate();
    }
}
