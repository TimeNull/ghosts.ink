using UnityEngine;

namespace SO.Variables
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "Variable/FloatVariable")]
    public class FloatVariable : Variable
    {
        [SerializeField] private float value;

        public float Value 
        {
            get => value;
            set 
            {
                this.value = value;
                ValueChanged();
            } 
        }

        public void SetValue(FloatVariable value)
        {
            ValueChanged();
            this.value = value.Value;
        }

        public void ApplyChange(float amount)
        {
            ValueChanged();
            value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            ValueChanged();
            value += amount.Value;
        }
    }
}
