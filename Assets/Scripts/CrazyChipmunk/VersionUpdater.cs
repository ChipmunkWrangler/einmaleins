using UnityEngine;

namespace CrazyChipmunk
{
    public abstract class VersionUpdater : MonoBehaviour
    {
        public abstract void UpdateVersion(string fromVersion, string toVersion);
    }
}