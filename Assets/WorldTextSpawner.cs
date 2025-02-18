using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldTextSpawner : MonoBehaviour
{
    [SerializeField] GameObject worldTextPrefab;
    [SerializeField] float spawnAtSecond = 10;
    [SerializeField] Transform objectToSpawnOn;
    [SerializeField] Vector3 offset;
    [SerializeField] string text;
    [SerializeField] float scalePerDistance = 0.5f;
    [SerializeField] bool followObject = true;
    [SerializeField] float destroyAfterSeconds = -1;
    GameObject worldText;
    Vector3 initialScale;

    void Start()
    {
        StartCoroutine(SpawnWorldTextAfterDelay());
    }

    void Update()
    {
        if (worldText != null && followObject)
        {
            worldText.transform.position = objectToSpawnOn.position + offset;
            ScaleTextBasedOnDistanceToCamera();
        }
        ;
    }

    IEnumerator SpawnWorldTextAfterDelay()
    {
        yield return new WaitForSeconds(spawnAtSecond);
        Vector3 spawnPosition = objectToSpawnOn.position + offset;
        worldText = Instantiate(worldTextPrefab, spawnPosition, Quaternion.identity);
        initialScale = worldText.transform.localScale;
        ScaleTextBasedOnDistanceToCamera();
        worldText.GetComponentInChildren<TMP_Text>().text = text;
        if (destroyAfterSeconds > 0)
        {
            Destroy(worldText, destroyAfterSeconds);
        }
    }

    private void ScaleTextBasedOnDistanceToCamera()
    {
        float distanceToCamera = Vector3.Distance(Camera.main.transform.position, worldText.transform.position);
        Vector3 newScale = initialScale * distanceToCamera * scalePerDistance;
        worldText.transform.localScale = newScale;
    }
}
