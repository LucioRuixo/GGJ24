using System;
using System.Collections;
using UnityEngine;

public class PhotoFrame : MonoBehaviour
{
    [SerializeField] private PhotoPiece[] photoPieces;
    [SerializeField] private Transform restoredTransform;

    [Space]

    [SerializeField] private float animationDuration = 1.0f;
    [SerializeField] private AnimationCurve animationSpeed;
    
    [Space]

    [SerializeField] private Vector3 slerpCenterOffsetDirection;
    [SerializeField] private float slerpCenterOffsetFactor;

    private int photoPiecesAdded = 0;
    private bool photoRestored = false;

    static public event Action OnPhotoRestoredEvent;

    private void OnEnable()
    {
        foreach (PhotoPiece photoPiece in photoPieces) photoPiece.OnLockedEvent += OnPhotoPieceAdded;
    }

    private void OnDisable()
    {
        foreach (PhotoPiece photoPiece in photoPieces) photoPiece.OnLockedEvent -= OnPhotoPieceAdded;
    }

    private void OnPhotoPieceAdded()
    {
        if (photoRestored) return;

        photoPiecesAdded++;
        if (photoPiecesAdded == photoPieces.Length) OnPhotoRestored();
    }

    private void OnPhotoRestored()
    {
        photoRestored = true;
        OnPhotoRestoredEvent?.Invoke();

        StartCoroutine(RestorationAnimation());
    }

    private IEnumerator RestorationAnimation()
    {
        float elapsed = 0.0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = restoredTransform.position;

        Vector3 relativeCenterOffset = slerpCenterOffsetDirection.normalized * ((endPosition - startPosition).magnitude * slerpCenterOffsetFactor);
        Vector3 relativeCenter = (endPosition - startPosition) * 0.5f + relativeCenterOffset;

        while (elapsed < animationDuration)
        {
            float interpolant = animationSpeed.Evaluate(elapsed / animationDuration);

            Vector3 startRelativeCenter = startPosition - relativeCenter;
            Vector3 endRelativeCenter = endPosition - relativeCenter;
            Vector3 currentPosition = Vector3.Slerp(startRelativeCenter, endRelativeCenter, interpolant) + relativeCenter;

            transform.position = currentPosition;

            yield return null;

            elapsed += Time.deltaTime;
        }

        transform.position = endPosition;
    }
}