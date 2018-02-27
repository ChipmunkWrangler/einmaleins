using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrazyChipmunk
{
    class VersionUpdateChecker : MonoBehaviour
    {
        bool isUpdating;
        [SerializeField] VersionUpdater updater = null;
        [SerializeField] string updateScene = null;
        [SerializeField] Scene postUpdateScene;

        void Start()
        {
            if (updater == null)
            {
                throw new UnassignedReferenceException("updater not set in VersionUpdateChecker");
            }
            CheckVersion();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                CheckVersion();
            }
        }

        void CheckVersion()
        {
            if (!ShouldUpdate())
            {
                return;
            }
            if (!IsInUpdateScene())
            {
                EnterUpdateScene(); // this will cause VersionUpdateChecker.Start() and hence CheckVersion to be called again, since they should also be instantiated in the update scene
                return;
            }
            DoUpdate();
            LeaveUpdateScene();
        }

        bool ShouldUpdate()
        {
            if (isUpdating)
            {
                Debug.Log("isUpdating");
                return false;
            }
            return VersionNumber.GetSavedVersion() != VersionNumber.GetCurrentVersion();
        }

        bool IsInUpdateScene()
        {
            return updateScene == "" || SceneManager.GetActiveScene().name == updateScene;
        }

        void EnterUpdateScene()
        {
            SceneManager.LoadScene(updateScene);
        }

        void LeaveUpdateScene()
        {
            if (postUpdateScene != null)
            {
                SceneManager.LoadScene(postUpdateScene.buildIndex);
            }
        }

        void DoUpdate()
        {
            isUpdating = true;
            updater.UpdateVersion(VersionNumber.GetSavedVersion(), VersionNumber.GetCurrentVersion());
            VersionNumber.WriteNewVersion();
            isUpdating = false;
        }
    }
}
