namespace CrazyChipmunk
{
    [System.Serializable]
    public abstract class SavedDataVersionModel : UnityEngine.MonoBehaviour
    {
        public abstract string Version
        {
            get;
            set;
        }
    }
}