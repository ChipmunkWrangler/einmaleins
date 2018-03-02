using System;
using UnityEngine;

namespace CrazyChipmunk
{
    public abstract class PersistentString : VariableString
    {
        bool initialized;

        public override string Get()
        {
            if (!initialized)
            {
                val = Load();
                initialized = true;
            }
            return base.Get();
        }

        protected override void Set(string value)
        {
            base.Set(value);
            Save(value);
            initialized = true;
        }

        protected abstract string Load();
        protected abstract void Save(string value);

        void OnEnable()
        {
            initialized = false;
        }
    }
}