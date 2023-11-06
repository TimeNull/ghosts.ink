using SO.Variables;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Rigidbody rb;

    [SerializeField] protected Transform target;

    [SerializeField] protected Transform player;

    protected IObjectPool<Projectile> enemyPool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<Projectile> EnemyPool { set => enemyPool = value; }

    public virtual void SetupEnemy(Transform target, Transform player)
    {
        this.player = player;
        this.target = target;
        agent.isStopped = false;
    }

    protected virtual void HandleMovement()
    {
        agent.SetDestination(target.position);
    }

    
    protected virtual void Update()
    {
        HandleMovement();
    }


}
