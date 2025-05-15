// CameraCutsceneManager.cs
using System.Collections;
using UnityEngine;

public class CameraCutsceneManager : MonoBehaviour
{
    [Header("Riferimenti obbligatori")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform cameraTopView;
    [SerializeField] private GameObject playerRemy; // deve avere FPSInput.cs

    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [Header("Durata visuale dall'alto")]
    [SerializeField] private float durataVistaAlto = 2f;

    private bool inCutscene = false;

    /// <summary>
    /// Avvia la cutscene: la camera si sposta sulla vista dall'alto per qualche secondo.
    /// </summary>
    public void AvviaVistaDallAlto()
    {
        if (inCutscene || playerCamera == null || cameraTopView == null) return;
        StartCoroutine(EseguiVistaAlto());
    }

    private IEnumerator EseguiVistaAlto()
    {
        inCutscene = true;

        // Blocca follow della camera nella testa
        if (playerRemy.TryGetComponent(out FPSInput fpsInput))
            fpsInput.BloccaCamera(true);

        // Salva stato originale
        originalParent = playerCamera.parent;
        originalPosition = playerCamera.position;
        originalRotation = playerCamera.rotation;

        // Sposta la camera
        playerCamera.SetParent(null);
        playerCamera.position = cameraTopView.position;
        playerCamera.rotation = cameraTopView.rotation;

        yield return new WaitForSeconds(durataVistaAlto);

        // Ripristina
        playerCamera.SetParent(originalParent);
        playerCamera.position = originalPosition;
        playerCamera.rotation = originalRotation;

        // Riattiva camera follow
        if (fpsInput != null)
            fpsInput.BloccaCamera(false);

        inCutscene = false;
    }
}
