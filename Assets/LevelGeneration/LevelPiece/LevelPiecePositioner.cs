using System;
using System.Collections;
using UnityEngine;

namespace LevelGeneration
{
    /// <summary>
    /// moves a position to a set position, rotation, and scale over a given time.
    /// </summary>
    public class LevelPiecePositioner : MonoBehaviour
    {

        //animation times
        [SerializeField] float hoverTime = 0.5f;

        //gemoetry reference
        [SerializeField] Transform walkOffPoint;

        //target position, rotation, and scale
        public Vector3 targetPieceScale = Vector3.one;
        public Vector3 targetPos { get; private set; }
        public Quaternion targetRot { get; private set; }
        public bool reachedFinalPosition { get; private set; }

        //materials
        [SerializeField] Material transparentMaterial;
        [SerializeField] Material defaultMaterial;

        //components
        Collider[] collidersToDisable;
        Renderer pieceRenderer;


        private void Awake()
        {
            pieceRenderer = GetComponentInChildren<Renderer>();
            collidersToDisable = GetComponentsInChildren<Collider>();
            if (!walkOffPoint) Debug.LogWarning("No walk off point set for " + name);
        }

        public void SetPosition(Vector3 targetPosition, Quaternion targetRotation)
        {
            this.targetPos = targetPosition;
            this.targetRot = targetRotation;
            SetFinalTransform(targetPosition, targetRotation);
        }

        public void MoveWithAnimation(Vector3 targetPosition, Quaternion targetRotation)
        {
            this.targetPos = targetPosition;
            this.targetRot = targetRotation;
            StartCoroutine(MoveAnimation());
        }

        IEnumerator MoveAnimation()
        {
            SetCollidersEnabled(false);
            yield return HoverInFrontOfCamera(hoverTime);
            Vector3 posWithSameZAsTarget = new Vector3(transform.position.x, transform.position.y, targetPos.z);
            yield return MoveToPosition(posWithSameZAsTarget);
            yield return new WaitForSeconds(0.1f);
            yield return ScaleToScale(targetPieceScale, 0.2f);
            yield return new WaitForSeconds(0.1f);
            yield return RotateToRotation(targetRot, 0.2f);
            yield return new WaitForSeconds(0.1f);
            yield return MoveToPosition(targetPos);
            SetFinalTransform(targetPos, targetRot);
            SetCollidersEnabled(true);
        }

        public void MoveWithSimpleAnimation(Vector3 _targetPos, Quaternion _targetRot)
        {
            targetPos = _targetPos;
            targetRot = _targetRot;
            StartCoroutine(SimpleMoveAnimation());
        }

        IEnumerator SimpleMoveAnimation()
        {
            SetCollidersEnabled(false);
            yield return new WaitForSeconds(0.2f);
            yield return RotateToRotation(targetRot, 0.2f);
            yield return new WaitForSeconds(0.1f);
            yield return MoveToPosition(targetPos);
            SetFinalTransform(targetPos, targetRot);
            SetCollidersEnabled(true);
        }

        IEnumerator HoverInFrontOfCamera(float hoverTime)
        {
            transform.parent = Camera.main.transform; //might get fucked by functions above
            yield return new WaitForSeconds(hoverTime);
            transform.parent = null; //SOULD BE WALKWAY
        }

        IEnumerator MoveToPosition(Vector3 targetPos)
        {
            float distanceToTarget = Vector3.Distance(targetPos, transform.position);
            Vector3 directionToTarget = (targetPos - transform.position).normalized;
            float moveSpeed = 15f; //magic!

            while (distanceToTarget > 0.1f)
            {
                transform.position += directionToTarget * moveSpeed * Time.deltaTime;
                distanceToTarget = Vector3.Distance(targetPos, transform.position);
                yield return null;
            }

            transform.position = targetPos;
        }

        IEnumerator RotateToRotation(Quaternion targetRot, float rotationTime)
        {
            float elapsedTime = 0;
            Quaternion startRotation = transform.rotation;
            while (elapsedTime < rotationTime)
            {
                float percentageOfTimePassed = elapsedTime / rotationTime;
                transform.rotation = Quaternion.Lerp(startRotation, targetRot, percentageOfTimePassed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRot;
        }

        IEnumerator ScaleToScale(Vector3 targetScale, float scaleTime)
        {
            float elapsedTime = 0;
            Vector3 startScale = transform.localScale;
            while (elapsedTime < scaleTime)
            {
                float percentageOfTimePassed = elapsedTime / scaleTime;
                transform.localScale = Vector3.Lerp(startScale, targetScale, percentageOfTimePassed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;
        }

        private void SetFinalTransform(Vector3 targetPosition, Quaternion targetRotation)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            transform.localScale = targetPieceScale;
            reachedFinalPosition = true;
        }

        void SetCollidersEnabled(bool enabled)
        {
            foreach (Collider collider in collidersToDisable)
            {
                if (collider)
                {
                    collider.enabled = enabled;
                }
            }

            if (enabled)
            {
                pieceRenderer.material = defaultMaterial;
            }
            else
            {
                pieceRenderer.material = transparentMaterial;
            }
        }

        public Vector3 GetFinalWalkOffPoint()
        {
            Vector3 localWalkOffPoint = transform.InverseTransformPoint(walkOffPoint.position);
            Vector3 scaledLocalWalkOffPoint = Vector3.Scale(localWalkOffPoint, targetPieceScale);
            Vector3 finalWalkOffPoint = targetPos + targetRot * scaledLocalWalkOffPoint;
            return finalWalkOffPoint;
        }
    }
}

