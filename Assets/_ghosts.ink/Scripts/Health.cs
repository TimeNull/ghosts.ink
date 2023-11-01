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


    public void TakeDamage(float variable, ColorType colorType = null)
    {
        if (colorType && colorType != this.colorType)
            return;

        currentHealth.ApplyChange(variable);
    }

    public void TakeDamage(FloatVariable variable, ColorType colorType = null)
    {
        if (colorType && colorType != this.colorType)
            return;

        currentHealth.ApplyChange(variable);
    }

}
