using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelGeneration
{
    /// <summary>
    /// loads a new scene when the player enters the trigger
    /// </summary>
    public class NextSceneLoadPlayerTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<SceneTransitioner>().EndScene(true);
            }
        }
    }
}
