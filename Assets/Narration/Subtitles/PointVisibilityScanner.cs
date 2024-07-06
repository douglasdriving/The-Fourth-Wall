using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointVisibilityScanner : MonoBehaviour
{
    [SerializeField] LayerMask layerToCheckForVisionBlock;
    [SerializeField] float minDistanceFromCamera = 6f;
    [SerializeField] Transform pointsParent;

    List<Vector3> pointsInScene = new();
    Camera cam;

    void Awake()
    {
        SetupSceneReferences();
    }

    private void SetupSceneReferences()
    {
        cam = Camera.main;
        foreach (Transform child in pointsParent)
        {
            pointsInScene.Add(child.position);
        }
    }

    public Vector3? GetClosestVisiblePoint()
    {
        Vector3[] visiblePoints = pointsInScene.Where(point => IsPointVisible(point)).ToArray();
        Vector3? bestPoint = GetPointAtBestDistance(visiblePoints);
        return bestPoint;
    }

    bool IsPointVisible(Vector3 point)
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(point);
        bool isInView = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                        viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                        viewportPoint.z > 0;

        if (!isInView) return false;

        Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(point));
        bool rayHitSomething = Physics.Raycast(ray, layerToCheckForVisionBlock);
        bool pointIsObstructed = rayHitSomething;
        bool pointIsVisible = !pointIsObstructed;

        return pointIsVisible;
    }

    private Vector3? GetPointAtBestDistance(Vector3[] points)
    {
        Vector3? bestPoint = null;
        foreach (Vector3 point in points)
        {
            float distanceToCamera = Vector3.Distance(cam.transform.position, point);

            bool isTooCloseToCamera = distanceToCamera < minDistanceFromCamera;
            if (isTooCloseToCamera) continue;

            if (bestPoint == null)
            {
                bestPoint = point;
                continue;
            }

            float distanceBetweenCameraAndCurrentBestPoint = Vector3.Distance(cam.transform.position, (Vector3)bestPoint);
            bool isCloserThanCurrentBestPoint = distanceToCamera < distanceBetweenCameraAndCurrentBestPoint;
            if (isCloserThanCurrentBestPoint)
            {
                bestPoint = point;
            }
        }
        return bestPoint;
    }
}
