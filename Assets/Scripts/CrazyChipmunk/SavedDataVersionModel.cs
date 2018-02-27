namespace CrazyChipmunk
{
    public abstract class SavedDataVersionModel : UnityEngine.MonoBehaviour
    {
        public abstract string Version
        {
            get;
            set;
        }
    }
}