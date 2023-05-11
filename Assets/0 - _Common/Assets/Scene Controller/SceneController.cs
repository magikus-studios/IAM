using UnityEngine;
using UnityEngine.SceneManagement;

namespace IAM
{
    public class SceneController : MonoBehaviour
    {
        public void ReloadScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
        public void LoadScene(int buildIndex) { SceneManager.LoadScene(buildIndex); }
        public void AddScene(int buildIndex) 
        {
            if (IsSceneLoaded(buildIndex)) { print("scene is loaded"); return; }
            SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive); 
        }
        public void CloseScene(int buildIndex) 
        {
            if (!IsSceneLoaded(buildIndex)) { return; }
            SceneManager.UnloadSceneAsync(buildIndex); 
        }

        public void ExitApp() { Application.Quit(); }

        private bool IsSceneLoaded(int buildIndex) { return SceneManager.GetSceneByBuildIndex(buildIndex).isLoaded; }
    }
}