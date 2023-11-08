using SO.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damageCooldown;
    [SerializeField] private FloatReference damage;
    [SerializeField] private ColorType colorType;
    [SerializeField, TagSelector] string targetTag;
    private float currentDmgCooldown;
    public float Damage
    {
        get
        {
            if (currentDmgCooldown <= 0)
                return damage;
            else
                return 0;
        }
    }
    public ColorType ColorType => colorType;

    private void Update()
    {
        if (currentDmgCooldown > 0)
            currentDmgCooldown -= Time.deltaTime;
        else
            currentDmgCooldown = damageCooldown;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentDmgCooldown > 0) return;

        if (collision.gameObject.CompareTag(targetTag))
        {
            collision.transform.GetComponent<Health>().TakeDamage(-damage.Value, colorType);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentDmgCooldown > 0) return;

        if (other.CompareTag(targetTag))
            other.GetComponent<Health>().TakeDamage(-damage.Value, colorType);
    }
}
