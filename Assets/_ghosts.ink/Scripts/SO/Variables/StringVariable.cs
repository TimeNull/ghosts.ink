using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Variables
{
    [CreateAssetMenu(fileName = "StringVariable", menuName = "Variable/StringVariable")]
    public class StringVariable : Variable
    {
        [SerializeField] private string value = "";

        public string Value
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
