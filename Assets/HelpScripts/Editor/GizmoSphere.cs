using UnityEngine;
public class GizmoSphere : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] float size = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, size);
    }
}
