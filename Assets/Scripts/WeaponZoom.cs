using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Thêm biến để tham chiếu đến Virtual Camera
    [SerializeField] private float zoomedInFieldOfView = 20.0f;
    [SerializeField] private float sniperZoomedInFieldOfView = 10.0f; // FOV cho súng bắn tỉa
    [SerializeField] private float zoomedMouseSensitivityFactor = 0.1f;

    private WeaponAnimator weaponAnimator;
    private FirstPersonController fpsController;
    private float baseFieldOfView;
    private bool isZoomed = false;

    private void Start()
    {
        baseFieldOfView = virtualCamera.m_Lens.FieldOfView; // Lấy FOV ban đầu từ Cinemachine Virtual Camera
        fpsController = FindObjectOfType<FirstPersonController>();
        UpdateActiveWeapon();
    }

    private void UpdateActiveWeapon()
    {
        Transform weaponsTransform = transform.Find("CameraRot/CameraRecoil/MainCamera/Weapons");
        Debug.Log("Weapons Transform found: " + (weaponsTransform != null));

        if (weaponsTransform != null)
        {
            foreach (Transform weapon in weaponsTransform)
            {
                Debug.Log("Checking weapon: " + weapon.name + ", Active: " + weapon.gameObject.activeInHierarchy);
                if (weapon.gameObject.activeInHierarchy)
                {
                    weaponAnimator = weapon.GetComponent<WeaponAnimator>();
                    Debug.Log("WeaponAnimator found: " + (weaponAnimator != null));
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (weaponAnimator == null)
        {
            UpdateActiveWeapon();
            return;
        }
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
        weaponAnimator.ZoomIn(zoomIn);
        float animTime = weaponAnimator.ZoomAnimTime;
        float elapsed = 0f;

        float startFieldOfView = virtualCamera.m_Lens.FieldOfView; // Sử dụng FOV từ Virtual Camera
        float endFieldOfView = zoomIn ? GetZoomedInFieldOfView() : baseFieldOfView;

        while (elapsed < animTime)
        {
            float currentFieldOfView = Mathf.Lerp(startFieldOfView, endFieldOfView, elapsed / animTime);
            virtualCamera.m_Lens.FieldOfView = currentFieldOfView; // Thay đổi FOV của Virtual Camera
            elapsed += Time.deltaTime;
            yield return null;
        }
        virtualCamera.m_Lens.FieldOfView = endFieldOfView; // Đảm bảo FOV cuối cùng đúng
    }

    private float GetZoomedInFieldOfView()
    {
        // Kiểm tra nếu vũ khí hiện tại là súng bắn tỉa
        if (weaponAnimator is SniperAnimator)
        {
            return sniperZoomedInFieldOfView;
        }
        return zoomedInFieldOfView;
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