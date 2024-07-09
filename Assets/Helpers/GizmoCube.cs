using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoCube : MonoBehaviour
{
    [SerializeField] Color color;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }
}
