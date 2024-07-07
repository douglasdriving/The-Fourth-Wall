using UnityEngine;

namespace Narration
{
    public class WordPointAdder : MonoBehaviour
    {
        void Awake()
        {
            FindAnyObjectByType<PointVisibilityScanner>().AddPoint(transform.position);
        }
    }
}

