using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] private WeaponAnimator ak47Animator;
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Thêm biến để tham chiếu đến Virtual Camera
    [SerializeField] private float zoomedInFieldOfView = 20.0f;
    [SerializeField] private float zoomedMouseSensitivityFactor = 0.1f;

    private FirstPersonController fpsController;
    private float baseFieldOfView;
    private bool isZoomed = false;

    private void Start()
    {
        baseFieldOfView = virtualCamera.m_Lens.FieldOfView; // Lấy FOV ban đầu từ Cinemachine Virtual Camera
        fpsController = FindObjectOfType<FirstPersonController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            StartCoroutine(ZoomAnim(true));
            isZoomed = true;
            AdjustMouseSensitivity();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            StopAllCoroutines();
            StartCoroutine(ZoomAnim(false));
            isZoomed = false;
            AdjustMouseSensitivity();
        }
    }

    private IEnumerator ZoomAnim(bool zoomIn)
    {
        ak47Animator.ZoomIn(zoomIn);
        float animTime = ak47Animator.ZoomAnimTime;
        float elapsed = 0f;

        float startFieldOfView = virtualCamera.m_Lens.FieldOfView; // Sử dụng FOV từ Virtual Camera
        float endFieldOfView = zoomIn ? zoomedInFieldOfView : baseFieldOfView;

        while (elapsed < animTime)
        {
            float currentFieldOfView = Mathf.Lerp(startFieldOfView, endFieldOfView, elapsed / animTime);
            virtualCamera.m_Lens.FieldOfView = currentFieldOfView; // Thay đổi FOV của Virtual Camera
            elapsed += Time.deltaTime;
            yield return null;
        }
        virtualCamera.m_Lens.FieldOfView = endFieldOfView; // Đảm bảo FOV cuối cùng đúng
    }
    private void AdjustMouseSensitivity()
    {
        if (fpsController != null)
        {
            float currentSensitivityFactor = isZoomed ? zoomedMouseSensitivityFactor : 1.0f;
            fpsController.SetMouseSensitivityFactor(currentSensitivityFactor);
        }
    }
}
