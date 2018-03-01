using UnityEngine;

namespace CrazyChipmunk
{
    // can't be generic or an interface, otherwise fields of this type can't be assigned in the editor
    [CreateAssetMenu(menuName = "CrazyChipmunk/String")]
    public class StringVariable : ScriptableObject
    {
        [SerializeField] string val;

        public string Value
        {
            get { return val; }
            set { val = value; }
        }
    }
}
