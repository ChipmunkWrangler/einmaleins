using UnityEngine;

namespace CrazyChipmunk
{
    // can't be generic or an interface, otherwise fields of this type can't be assigned in the editor
    public class VariableString : ReadOnlyString
    {
        public string Value
        {
            get { return Get(); }
            set { Set(value); }
        }
    }
}
