using UnityEngine;

namespace CrazyChipmunk
{
    public abstract class SavedDataUpdater : MonoBehaviour
    {
        public abstract void UpdateData(string fromVersion, string toVersion);
    }
}