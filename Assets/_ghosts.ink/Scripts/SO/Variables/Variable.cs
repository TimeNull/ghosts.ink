using System;
using UnityEngine;

namespace SO.Variables
{
    public abstract class Variable : ScriptableObject
    {
        public event Action OnValueChanged;

        protected void ValueChanged() => OnValueChanged?.Invoke();
        
    }
}