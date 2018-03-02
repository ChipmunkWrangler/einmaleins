using UnityEngine;

namespace CrazyChipmunk
{
    // can't be generic or an interface, otherwise fields of this type can't be assigned in the editor
    [CreateAssetMenu(menuName = "CrazyChipmunk/String")]
    public class VariableString : ReadOnlyString
    {
        public new virtual string Value
        {
            get { return base.Value; }
            set { val = value; }
        }
    }
}
