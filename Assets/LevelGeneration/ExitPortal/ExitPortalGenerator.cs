using System;
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
        [SerializeField] GameObject votingPathPrefab;

        [SerializeField] float portalHeightAbovePlatform = 1f;
        [SerializeField] float timeBetweenPieceAndPortalSpawn = 0.8f;

        float timeLeftBeforeSpawn;

        enum State
        {
            NOT_STARTED,
            COUNTING_DOWN,
            PAUSED,
            DONE

        }

        State state = State.NOT_STARTED;

        public void StartSpawnCountdown(float timeToSpawnPortalAt)
        {
            timeLeftBeforeSpawn = timeToSpawnPortalAt;
            state = State.COUNTING_DOWN;
        }

        public void PauseSpawnCountdown()
        {
            state = State.PAUSED;
        }

        public void ResumeSpawnCountdown()
        {
            state = State.COUNTING_DOWN;
        }

        void Update()
        {
            if (state == State.NOT_STARTED) return;
            if (state == State.PAUSED) return;
            if (state == State.DONE) return;

            timeLeftBeforeSpawn -= Time.deltaTime;

            if (timeLeftBeforeSpawn <= 0f)
            {
                StartCoroutine(StartPortalSpawn());
                state = State.DONE;
            }
        }

        IEnumerator StartPortalSpawn()
        {
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
            if (rules)
            {
                if (rules.portalType == SceneRules.PortalType.QUIZ)
                {
                    GameObject portal = Instantiate(quizPortalPrefab, portalPos, targetRot);
                    SetQuizIfExists(portal);
                }
                else if (rules.portalType == SceneRules.PortalType.VOTING_PATHS)
                {
                    Vector3 spawnPos = piecePositioner.GetFinalWalkOffPoint();
                    GameObject votingPaths = Instantiate(votingPathPrefab, spawnPos, targetRot);
                    // set the question??? or maybe the narrator should just say it? feels like that would make SO MUCH more sense. but maybe it should also be written
                }
                else
                {
                    Instantiate(portalPrefab, portalPos, targetRot);
                }
            }
            else
            {
                Instantiate(portalPrefab, portalPos, targetRot);
            }
        }

        private void SetQuizIfExists(GameObject portal) //TODO: move to the portal class
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

