using UnityEngine;

namespace Helpers
{
    /// <summary>
    /// shows a gizmo sphere at the object position in the editor
    /// </summary>
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
}

