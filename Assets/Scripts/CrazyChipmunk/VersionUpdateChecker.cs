using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrazyChipmunk
{
    class VersionUpdateChecker : MonoBehaviour
    {
        [SerializeField] SavedDataVersionModel versionModel = null;
        [SerializeField] SavedDataUpdater updater = null;
        [SerializeField] string updateScene = "updateVersion";
        [SerializeField] string postUpdateScene = "";

        bool isUpdating;

        void Start()
        {
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

            return versionModel.Version != Application.version;
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
            if (postUpdateScene != "")
            {
                SceneManager.LoadScene(postUpdateScene);
            }
        }

        void DoUpdate()
        {
            isUpdating = true;
            if (updater.UpdateData(versionModel.Version, Application.version))
            {
                versionModel.Version = Application.version;
                isUpdating = false;
            }
            else
            {
                postUpdateScene = "";
            }
        }
    }
}