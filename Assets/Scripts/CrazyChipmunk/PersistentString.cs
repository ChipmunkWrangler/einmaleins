using System;
using UnityEngine;

namespace CrazyChipmunk
{
    public abstract class PersistentString : VariableString
    {
        bool initialized;

        public override string Value
        {
            get
            {
                if (!initialized)
                {
                    base.Value = Load();
                    initialized = true;
                }
                return base.Value;
            }
            set
            {
                base.Value = value;
                Save(value);
                initialized = true;
            }
        }

        abstract protected string Load();
        abstract protected void Save(string val);
    }
}