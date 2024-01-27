using System;
using System.Collections;
using UnityEngine;

public class PhotoFrame : MonoBehaviour
{
    [SerializeField] private PhotoPiece[] photoPieces;

    [Space]

    [SerializeField] private Transform restoredTransform;
    [SerializeField] private float restorationAnimationDuration = 1.0f;
    [SerializeField] private AnimationCurve animationSpeed;

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

        Vector3 initialPosition = transform.position;
        Vector3 finalPosition = restoredTransform.position;

        while (elapsed < restorationAnimationDuration)
        {
            float interpolant = animationSpeed.Evaluate(elapsed / restorationAnimationDuration);
            Vector3 currentPosition = Vector3.Lerp(initialPosition, finalPosition, interpolant);
            transform.position = currentPosition;

            yield return null;

            elapsed += Time.deltaTime;
        }

        transform.position = finalPosition;
    }
}