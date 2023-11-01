using System.Collections;
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

    public void SetupBullet(string targetTag, Vector3 acceleration, float damage, ColorType colorType)
    {
        currentVelocity = Vector3.zero;
        this.acceleration = acceleration;
        this.damage = damage;
        this.colorType = colorType;
        this.targetTag = targetTag;
        meshRenderer.material.color = colorType.color;
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
            other.GetComponent<Health>().TakeDamage(-damage, colorType);
    }

    //V V V V object pooling V V V V

    [SerializeField] private float timeoutDelay = 3f;

    private IObjectPool<Projectile> objectPool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<Projectile> ObjectPool { set => objectPool = value; }

    public void Deactivate()
    {
        StartCoroutine(DeactivateRoutine(timeoutDelay));
    }

    private IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        // reset the moving Rigidbody
        Rigidbody rBody = GetComponent<Rigidbody>();
        rBody.velocity = new Vector3(0f, 0f, 0f);
        rBody.angularVelocity = new Vector3(0f, 0f, 0f);

        // release the projectile back to the pool
        objectPool.Release(this);
    }

}
