using SO.Variables;
using System;

namespace SO.Variables
{
    [Serializable]
    public class FloatReference
    {
        public bool UseConstant = true;
        public float ConstantValue;
        public FloatVariable Variable;

        public FloatReference()
        { }

        public FloatReference(float value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public float Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public void SetValue(float value)
        {
            if (UseConstant)
                ConstantValue = value;
            else
                Variable.Value = value;
        }

        public void ApplyChange(float value)
        {
            if (UseConstant)
                ConstantValue += value;
            else
                Variable.ApplyChange(value);
        }

        public void ApplyChange(FloatVariable value)
        {
            if (UseConstant)
                ConstantValue += value.Value;
            else
                Variable.ApplyChange(value);
        }

        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }
}
