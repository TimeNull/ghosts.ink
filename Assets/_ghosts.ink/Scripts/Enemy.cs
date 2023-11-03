using SO.Variables;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private FloatReference attackCooldown;
    [SerializeField] private FloatReference attackDamage;

    private IObjectPool<Projectile> enemyPool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<Projectile> EnemyPool { set => enemyPool = value; }

    public abstract void SetupEnemy();

    public abstract void Attack();

    public abstract void Move();

}
