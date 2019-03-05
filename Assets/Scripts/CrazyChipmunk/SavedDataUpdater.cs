using UnityEngine;

namespace CrazyChipmunk
{
    public abstract class SavedDataUpdater : MonoBehaviour
    {
        public abstract bool UpdateData(string fromVersion, string toVersion);
    }
}