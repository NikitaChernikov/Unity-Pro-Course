using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpticalSight : MonoBehaviour
{
    [SerializeField] Camera cameraMain;
    [SerializeField] Camera opticalCamera;
    [SerializeField] GameObject optic;
    [SerializeField] Slider slider;
    PlayerLook playerLook;
    [SerializeField] float mouse;

    float mouseMax = 0.2f;
    float maxFOV = 60;

    bool isOptic;
    // Start is called before the first frame update
    void Start()
    {
        mouse = mouseMax;
        isOptic = false;
        playerLook = GetComponent<PlayerLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOptic)
        {
            mouse = slider.value / maxFOV * mouseMax;
            opticalCamera.fieldOfView = Mathf.Lerp(opticalCamera.fieldOfView, slider.value, 10 * Time.deltaTime);
            playerLook.ChangeMouseSensitivity(mouse);
            cameraMain.enabled = false;
            opticalCamera.enabled = true;
            optic.SetActive(true);
        }
        else
        {
            slider.value = maxFOV;
            mouse = mouseMax;
            cameraMain.enabled = true;
            opticalCamera.enabled = false;
            optic.SetActive(false);
        }
    }

    public void OpticOnOff()
    {
        if (!isOptic)
        {
            GetComponent<CameraPositionChanger>().crosshairUI.SetActive(false);
            isOptic = true;
        }
        else
        {
            GetComponent<CameraPositionChanger>().crosshairUI.SetActive(true);
            isOptic = false;
        }
    }

}
