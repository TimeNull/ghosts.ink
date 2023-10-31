using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Variables
{
    [CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable")]
    public class IntVariable : Variable
    {
        [SerializeField] private int value;

        public int Value 
        {
            get => value;
            set
            {
                this.value = value;
                ValueChanged();
            }
        }

        public void SetValue(IntVariable value)
        {
            ValueChanged();
            this.value = value.Value;
        }

        public void ApplyChange(int amount)
        {
            ValueChanged();
            value += amount;
        }

        public void ApplyChange(IntVariable amount)
        {
            ValueChanged();
            value += amount.Value;
        }

    }
}
