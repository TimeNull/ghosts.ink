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
            this.value = value.Value;
            ValueChanged();
        }

        public void ApplyChange(int amount)
        {
            value += amount;
            ValueChanged();
        }

        public void ApplyChange(IntVariable amount)
        {
            value += amount.Value;
            ValueChanged();
        }

    }
}
