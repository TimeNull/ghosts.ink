using SO.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private ColorType colorType;
    [SerializeField] private FloatReference maxHealth;
    [SerializeField] private FloatReference currentHealth;

    public UnityEvent OnDie;

    private void Start()
    {
        currentHealth.SetValue(maxHealth.Value); 
    }

    private void Update()
    {
        if(currentHealth.Value < 0.01f)
        {
            currentHealth.SetValue(0);
            Die();
        }
    }

    private void Die()
    {
        OnDie.Invoke();
    }


    public bool TakeDamage(float variable, ColorType colorType = null)
    {
        if (this.colorType && colorType && colorType != this.colorType)
            return false; //didn't match the color

        currentHealth.ApplyChange(variable);
        return true; //did match the color
    }

    public bool TakeDamage(FloatVariable variable, ColorType colorType = null)
    {
        if (this.colorType && colorType && colorType != this.colorType)
            return false; //didn't match the color

        currentHealth.ApplyChange(variable);
        return true; //did match the color
    }

    private Collider lastCollider;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //on collision enter
        if(hit.collider != lastCollider)
        {
            Debug.Log(hit.collider);
            lastCollider = hit.collider;

            if (hit.collider.TryGetComponent(out DealDamage damage))
                TakeDamage(-damage.Damage, damage.ColorType);

        }
        
    }
}
