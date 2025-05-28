using System;
using System.Collections;
using QuizPortal;
using UnityEngine;

namespace LevelGeneration
{
    /// <summary>
    /// generates an exit portal at the end of the level after a delay
    /// </summary>
    public class EndObjectSpawner : MonoBehaviour //CHANGE THE NAME OF THIS CLASS TO EXITOBJECTGENERATOR OR SOMETHING LIKE THAT
    {
        [Header("Prefabs")]
        [Tooltip("The prefab to spawn at the end of the level.")]
        [SerializeField] GameObject portalPrefab;
        [SerializeField] GameObject quizPortalPrefab;
        [SerializeField] GameObject votingPathPrefab;
        [SerializeField] GameObject thoughtLevitatorPrefab;

        [Header("Settings")]
        [SerializeField] float pointHeightAbovePlatform = 1f;
        [SerializeField] float spawnDelay = 0.8f;

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
            yield return new WaitForSeconds(spawnDelay);
            SpawnPortal(portalLevelPiece);
        }

        private void SpawnPortal(GameObject portalLevelPiece)
        {
            LevelPiece.Positioner piecePositioner = portalLevelPiece.GetComponent<LevelPiece.Positioner>();
            Vector3 pointAbovePlatform = piecePositioner.targetPos + Vector3.up * pointHeightAbovePlatform;
            Quaternion targetRot = piecePositioner.targetRot;

            SceneRules rules = FindObjectOfType<SceneRules>();
            if (rules)
            {
                switch (rules.portalType)
                {
                    case SceneRules.EndSpawnObject.PORTAL:
                        Instantiate(portalPrefab, pointAbovePlatform, targetRot);
                        break;
                    case SceneRules.EndSpawnObject.PORTAL_QUIZ:
                        GameObject portal = Instantiate(quizPortalPrefab, pointAbovePlatform, targetRot);
                        SetQuizIfExists(portal);
                        break;
                    case SceneRules.EndSpawnObject.VOTING_PATHS:
                        Vector3 spawnPos = piecePositioner.GetFinalWalkOffPoint();
                        GameObject votingPaths = Instantiate(votingPathPrefab, spawnPos, targetRot);
                        // set the question??? or maybe the narrator should just say it? feels like that would make SO MUCH more sense. but maybe it should also be written
                        break;
                    case SceneRules.EndSpawnObject.THOUGHT_LEVITATOR:
                        pointAbovePlatform += Vector3.forward * 0.5f;
                        pointAbovePlatform += Vector3.down * 0.5f;
                        Instantiate(thoughtLevitatorPrefab, pointAbovePlatform, targetRot);
                        break;
                }
            }
            else
            {
                Instantiate(portalPrefab, pointAbovePlatform, targetRot);
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

