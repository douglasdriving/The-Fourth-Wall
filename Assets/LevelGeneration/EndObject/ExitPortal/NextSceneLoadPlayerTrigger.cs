using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelGeneration
{
    /// <summary>
    /// loads a new scene when the player enters the trigger
    /// </summary>
    public class NextSceneLoadPlayerTrigger : MonoBehaviour
    {
        [SerializeField] string sceneNameOverride = "";
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (sceneNameOverride != "")
                {
                    FindObjectOfType<SceneTransitioner>().EndScene(true, sceneNameOverride);
                }
                else
                {
                    FindObjectOfType<SceneTransitioner>().EndScene(true);
                }

            }
        }
    }
}
