using System.Collections;
using QuizPortal;
using UnityEngine;

namespace LevelGeneration
{
    /// <summary>
    /// generates an exit portal at the end of the level after a delay
    /// </summary>
    public class ExitPortalGenerator : MonoBehaviour
    {
        [SerializeField] GameObject portalPrefab;
        [SerializeField] GameObject quizPortalPrefab;
        [SerializeField] float portalHeightAbovePlatform = 1f;
        [SerializeField] float timeBetweenPieceAndPortalSpawn = 0.8f;

        public IEnumerator GenerateExitPortalAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            GameObject portalLevelPiece = FindObjectOfType<LevelGenerator>().SpawnNextPiece("", false);
            yield return new WaitForSeconds(timeBetweenPieceAndPortalSpawn);
            SpawnPortal(portalLevelPiece);
        }

        private void SpawnPortal(GameObject portalLevelPiece)
        {
            LevelPiece.Positioner piecePositioner = portalLevelPiece.GetComponent<LevelPiece.Positioner>();
            Vector3 portalPos = piecePositioner.targetPos + Vector3.up * portalHeightAbovePlatform;
            Quaternion targetRot = piecePositioner.targetRot;

            SceneRules rules = FindObjectOfType<SceneRules>();
            if (rules && rules.endQuiz)
            {
                GameObject portal = Instantiate(quizPortalPrefab, portalPos, targetRot);
                SetQuizIfExists(portal);
            }
            else
            {
                Instantiate(portalPrefab, portalPos, targetRot);
            }
        }

        private void SetQuizIfExists(GameObject portal)
        {
            EndQuizSetter endQuizSetter = GetComponent<EndQuizSetter>();
            PortalQuizSetter portalQuizSetter = portal.GetComponent<PortalQuizSetter>();
            if (endQuizSetter != null && portalQuizSetter != null)
            {
                endQuizSetter.SetQuestion(portal);
            }
            else if (endQuizSetter != null)
            {
                Debug.LogWarning("EndQuizSetter found but no PortalQuizSetter found on the portal");
            }
            else if (portalQuizSetter != null)
            {
                Debug.LogWarning("PortalQuizSetter found but no EndQuizSetter found on the ExitPortalGenerator");
            }
        }
    }
}

