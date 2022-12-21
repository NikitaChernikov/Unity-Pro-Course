using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionChanger : MonoBehaviour
{
    [SerializeField] GameObject scopePos;
    [SerializeField] GameObject mainPos;
    [SerializeField] GameObject camera;
    [SerializeField] GameObject shootUI;
    [SerializeField] public GameObject crosshairUI;
    bool isScope;

    private void Update()
    {
        if (!isScope)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, mainPos.transform.position, 0.05f);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, mainPos.transform.rotation, 0.05f);
        }
        else
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, scopePos.transform.position, 0.05f);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, scopePos.transform.rotation, 0.05f);
        }
    }

    public void ScopeOnOff()
    {
        if (isScope)
        {
            crosshairUI.SetActive(false);
            shootUI.SetActive(false);
            isScope = false;
        }
        else
        {
            crosshairUI.SetActive(true);
            shootUI.SetActive(true);
            isScope = true;
        }
    }

}
