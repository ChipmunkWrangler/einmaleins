using UnityEngine;

namespace CrazyChipmunk
{
    public class ReadOnlyString : ScriptableObject
    {
        [SerializeField] protected string val = null;

        public virtual string Get()
        {
            return val;
        }

        public static implicit operator string(ReadOnlyString reference)
        {
            return reference.Get();
        }

        protected virtual void Set(string value) 
        {
            val = value;
        }
    }
}