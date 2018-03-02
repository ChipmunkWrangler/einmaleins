using UnityEngine;

namespace CrazyChipmunk
{
    public class ReadOnlyString : ScriptableObject
    {
        [SerializeField] protected string val = null;

        public string Value
        {
            get { return val; }
        }

        public static implicit operator string(ReadOnlyString reference)
        {
            return reference.Value;
        }
    }
}