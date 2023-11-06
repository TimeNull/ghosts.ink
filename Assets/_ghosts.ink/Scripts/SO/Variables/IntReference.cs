using System;

namespace SO.Variables
{
    [Serializable]
    public class IntReference
    {
        public bool UseConstant = true;
        public int ConstantValue;
        public IntVariable Variable;

        public IntReference()
        { }

        public IntReference(int value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public int Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public void SetValue(int value)
        {
            if (UseConstant)
                ConstantValue = value;
            else
                Variable.Value = value;
        }

        public void ApplyChange(int value)
        {
            if (UseConstant)
                ConstantValue += value;
            else
                Variable.ApplyChange(value);
        }

        public void ApplyChange(IntVariable value)
        {
            if (UseConstant)
                ConstantValue += value.Value;
            else
                Variable.ApplyChange(value);
        }

        public static implicit operator int(IntReference reference)
        {
            return reference.Value;
        }
    }
}
