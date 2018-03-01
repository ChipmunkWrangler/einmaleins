using System;
using UnityEngine;

namespace CrazyChipmunk
{
    // split into StringReference and PersistentStringReference
    [Serializable]
    public abstract class PersistentStringReference
    {
        [SerializeField] StringVariable Variable = null;
        bool initialized;

        public string Value
        {
            get
            {
                if (!initialized)
                {
                    Variable.Value = Load();
                    initialized = true;
                }
                return Variable.Value;
            }
            set
            {
                Variable.Value = value;
                Save(Variable.Value);
                initialized = true;
            }
        }

        public PersistentStringReference() { }

        public PersistentStringReference(string value)
        {
            Value = value;
        }

        //public static implicit operator string(PersistentStringReference reference)
        //{
        //    return reference.Value;
        //}

        abstract protected string Load();
        abstract protected void Save(string val);
    }
}