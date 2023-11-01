using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    [SerializeField] private ColorType[] colorTypes;

    [SerializeField] private int currentColorType;

    [Tooltip("Prefab to shoot")]
    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private float damage;

    [Tooltip("Projectile force")]
    [SerializeField] private float muzzleVelocity = 700f;

    [Tooltip("End point of gun where shots appear")]
    [SerializeField] private Transform muzzlePosition;

    [Tooltip("Time between shots / smaller = higher rate of fire")]
    [SerializeField] private float cooldownWindow = 0.1f;

    // stack-based ObjectPool available with Unity 2021 and above
    private IObjectPool<Projectile> projectilePool;

    // throw an exception if we try to return an existing item, already in the pool
    [SerializeField] private bool collectionCheck = true;

    // extra options to control the pool capacity and maximum size
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    [SerializeField, TagSelector] private string targetTag;

    private float nextTimeToShoot;


    private void Awake()
    {
        projectilePool = new ObjectPool<Projectile>(CreateProjectile,
               OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
               collectionCheck, defaultCapacity, maxSize);
    }

    private void Update()
    {
        
    }

    // invoked when creating an item to populate the object pool
    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = Instantiate(projectilePrefab);
        projectileInstance.ObjectPool = projectilePool;
        return projectileInstance;
    }

    // invoked when returning an item to the object pool
    private void OnReleaseToPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // invoked when retrieving the next item from the object pool
    private void OnGetFromPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
    private void OnDestroyPooledObject(Projectile pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }


    public void Fire()
    {
        Projectile bulletObject = projectilePool.Get();

        if (bulletObject == null)
            return;

        // align to gun barrel/muzzle position
        bulletObject.transform.SetPositionAndRotation(muzzlePosition.position, muzzlePosition.rotation);

        // move projectile forward
        bulletObject.SetupBullet(targetTag, muzzlePosition.forward * muzzleVelocity, damage, colorTypes[currentColorType]);

        // turn off after a few seconds
        bulletObject.Deactivate();

        // set cooldown delay
        //nextTimeToShoot = Time.time + cooldownWindow;
    }

    public void ChangeColor()
    {
        if (currentColorType < colorTypes.Length)
            currentColorType++;
        else
            currentColorType = 0;
    }

    public void ChangeGun()
    {

    }

}
