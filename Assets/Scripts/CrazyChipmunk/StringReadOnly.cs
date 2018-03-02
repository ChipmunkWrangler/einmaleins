using UnityEngine;

namespace CrazyChipmunk
{
    public class StringReadOnly : ScriptableObject
    {
        [SerializeField] protected string val = null;

        public string Value
        {
            get { return val; }
        }

        public static implicit operator string(StringReadOnly reference)
        {
            return reference.Value;
        }
    }
}