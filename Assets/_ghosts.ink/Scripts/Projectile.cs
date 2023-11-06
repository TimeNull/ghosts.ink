using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{

    [SerializeField] private Rigidbody body;
    [SerializeField] private MeshRenderer meshRenderer;
    private Vector3 acceleration;
    private Vector3 currentVelocity;
    private float damage;
    private ColorType colorType;
    
    private string targetTag;

    public void SetupBullet(string targetTag, Vector3 acceleration, float damage, ColorType colorType, int layer)
    {
        currentVelocity = Vector3.zero;
        this.damage = damage;
        this.acceleration = acceleration;
        this.colorType = colorType;
        this.targetTag = targetTag;
        meshRenderer.material.color = colorType.color;
        gameObject.layer = layer;
    }

    private void FixedUpdate()
    {
        HandleMovement();    
    }

    private void HandleMovement()
    {
        body.AddForce(acceleration, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if(other.TryGetComponent(out Health health))
            {
                health.TakeDamage(-damage, colorType);
                Deactivate();
            }
        }
        else
        {
            Deactivate();
        }
    }

    //V V V V object pooling V V V V

    [SerializeField] private float timeoutDelay = 3f;

    private IObjectPool<Projectile> projectilePool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<Projectile> ProjectilePool { set => projectilePool = value; }

    public void StartDeactivate()
    {
        Invoke(nameof(Deactivate), timeoutDelay);
    }

    private void Deactivate()
    {
        CancelInvoke(nameof(Deactivate));

        // reset the moving Rigidbody
        body = GetComponent<Rigidbody>();
        body.velocity = new Vector3(0f, 0f, 0f);
        body.angularVelocity = new Vector3(0f, 0f, 0f);

        // release the projectile back to the pool
        projectilePool.Release(this);
    }

}
