using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelPiece
{
    /// <summary>
    /// moves a position to a set position, rotation, and scale over a given time.
    /// </summary>
    [RequireComponent(typeof(ColorSetter))]
    public class Positioner : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 18f; //speed of the movement

        //animation times
        [SerializeField] float hoverTime = 0.5f;

        //gemoetry reference
        [SerializeField] Transform walkOffPoint;

        //target position, rotation, and scale
        public Vector3 targetPieceScale = Vector3.one;
        public Vector3 targetPos { get; private set; }
        public Quaternion targetRot { get; private set; }
        public bool reachedFinalPosition { get; private set; }

        //freezing
        public bool isFrozen = false;

        //components
        Collider[] colliders;
        ColorSetter colorSetter;
        Freezer freezer;

        //object refs
        Transform playerCamera;

        private void Awake()
        {
            if (!walkOffPoint) Debug.LogWarning("No walk off point set for " + name);
            colliders = GetComponentsInChildren<Collider>();
            colorSetter = GetComponent<ColorSetter>();
            freezer = GetComponentInChildren<Freezer>();
            playerCamera = Camera.main.transform;
        }

        public void SetPosition(Vector3 targetPosition, Quaternion targetRotation)
        {
            this.targetPos = targetPosition;
            this.targetRot = targetRotation;
            SetFinalTransform(targetPosition, targetRotation);
        }

        public void MoveWithAnimation(Vector3 targetPosition, Quaternion targetRotation)
        {

            IEnumerator MoveAnimation()
            {

                IEnumerator HoverInFrontOfCamera(float hoverTime)
                {
                    transform.parent = Camera.main.transform;
                    yield return new WaitForSeconds(hoverTime);
                    transform.parent = null;
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

                SetCollidersEnabled(false);
                yield return HoverInFrontOfCamera(hoverTime);
                Vector3 posWithSameZAsTarget = new Vector3(transform.position.x, transform.position.y, targetPos.z);
                yield return MoveOverTime(posWithSameZAsTarget, 0.3f);
                yield return new WaitForSeconds(0.1f);
                yield return ScaleToScale(targetPieceScale, 0.2f);
                yield return new WaitForSeconds(0.1f);
                yield return RotateOverTime(targetRot, 0.2f);
                yield return new WaitForSeconds(0.1f);
                yield return MoveOverTime(targetPos, 0.3f);
                SetFinalTransform(targetPos, targetRot);
                SetCollidersEnabled(true);
            }

            this.targetPos = targetPosition;
            this.targetRot = targetRotation;
            StartCoroutine(MoveAnimation());
        }

        public void MoveWithSimpleAnimation(Vector3 _targetPos, Quaternion _targetRot)
        {
            IEnumerator SimpleMoveAnimation()
            {
                if (!isFrozen)
                {
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    while (isFrozen)
                    {
                        yield return null;
                    }
                }
                SetCollidersEnabled(false);
                yield return RotateOverTime(targetRot, 0.2f);
                yield return new WaitForSeconds(0.1f);
                yield return MoveOverTime(targetPos, 0.3f);
                SetFinalTransform(targetPos, targetRot);
                SetCollidersEnabled(true);
            }

            targetPos = _targetPos;
            targetRot = _targetRot;
            StartCoroutine(SimpleMoveAnimation());
        }

        public void MoveFromTalkingHead(Vector3 posAboveTarget, Vector3 _targetPos, Quaternion _targetRot)
        {
            IEnumerator MovePattern()
            {

                IEnumerator MoveFromMouthToAir(float time, float scaleInMouth)
                {
                    Vector3 finalScale = transform.localScale;
                    transform.localScale *= scaleInMouth;
                    float distanceToPlayerCam = Vector3.Distance(playerCamera.position, posAboveTarget);
                    Vector3 scaleInAir = finalScale * distanceToPlayerCam / 10;

                    Vector3 directionFromPosOverTargetToPlayer = (playerCamera.position - posAboveTarget).normalized;
                    Quaternion rotInAir = Quaternion.LookRotation(Vector3.up, directionFromPosOverTargetToPlayer);

                    yield return TransformOverTime(posAboveTarget, rotInAir, scaleInAir, time);
                }

                SetCollidersEnabled(false);
                colorSetter.SetBaseMaterial();
                yield return MoveFromMouthToAir(time: 0.5f, scaleInMouth: 0.2f);
                freezer.Freeze();
                SetCollidersEnabled(true);
                while (isFrozen) yield return null;
                SetCollidersEnabled(false);
                yield return TransformOverTime(targetPos, targetRot, targetPieceScale, time: 0.5f);
                SetFinalTransform(targetPos, targetRot);
                SetCollidersEnabled(true);
            }
            targetPos = _targetPos;
            targetRot = _targetRot;
            StartCoroutine(MovePattern());
        }

        IEnumerator MoveOverTime(Vector3 targetPos, float time)
        {
            yield return TransformOverTime(targetPos, transform.rotation, transform.localScale, time);
        }

        IEnumerator RotateOverTime(Quaternion targetRot, float time)
        {
            yield return TransformOverTime(transform.position, targetRot, transform.localScale, time);
        }

        IEnumerator ScaleOverTime(Vector3 targetScale, float time)
        {
            yield return TransformOverTime(transform.position, transform.rotation, targetScale, time);
        }

        IEnumerator TransformOverTime(Vector3 targetPosition, Quaternion targetRotation, Vector3 targetScale, float time)
        {
            float elapsedTime = 0;
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;
            Vector3 startScale = transform.localScale;
            bool isMoving = targetPosition != startPosition;
            bool isRotating = targetRotation != startRotation;
            bool isScaling = targetScale != startScale;
            while (elapsedTime < time)
            {
                float percentageOfTimePassed = elapsedTime / time;
                if (isMoving) transform.position = Vector3.Lerp(startPosition, targetPosition, percentageOfTimePassed);
                if (isRotating) transform.rotation = Quaternion.Lerp(startRotation, targetRotation, percentageOfTimePassed);
                if (isScaling) transform.localScale = Vector3.Lerp(startScale, targetScale, percentageOfTimePassed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
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
            foreach (Collider collider in colliders)
            {
                if (collider)
                {
                    collider.enabled = enabled;
                }
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

