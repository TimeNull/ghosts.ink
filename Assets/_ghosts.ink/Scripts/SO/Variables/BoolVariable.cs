using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Variables
{
    [CreateAssetMenu(fileName = "BoolVariable", menuName = "Variable/BoolVariable")]
    public class BoolVariable : Variable
    {
        [SerializeField] private bool value = false;

        public bool Value
        {
            get { return value; }
            set 
            {
                this.value = value;
                ValueChanged();
            }
        }

    }
}
