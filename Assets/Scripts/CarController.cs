using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    //[SerializeField] GameObject lightLeft;
    //[SerializeField] GameObject lightRight;

    [SerializeField] List<AxleInfo> axleInfos;
    [SerializeField] float maxMotorTorque;
    [SerializeField] float maxSteeringAngle;
    [SerializeField] Joystick joystick;
    bool isBreak;
    [SerializeField] GameObject player;
    [SerializeField] TrailRenderer leftWheel;
    [SerializeField] TrailRenderer rightWheel;
    [SerializeField] GameObject raceUI;
    float pi;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(rb.centerOfMass.x, 0.3f, rb.centerOfMass.z);
    }

    private void FixedUpdate()
    {
        pi = Mathf.Lerp(0.6f, 0.1f, joystick.Vertical);
        GetComponent<AudioSource>().pitch = Mathf.Lerp(GetComponent<AudioSource>().pitch, pi, 0.01f);
        float motor = maxMotorTorque * joystick.Vertical; //ускорение
        float steering = maxSteeringAngle * joystick.Horizontal; //угол поворота
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (!isBreak)
            {
                //lightLeft.SetActive(false);
                //lightRight.SetActive(false);

                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
                leftWheel.emitting = false;
                rightWheel.emitting = false;
            }
            else
            {
                //lightLeft.SetActive(true);
                //lightRight.SetActive(true);

                axleInfo.leftWheel.brakeTorque = 500;
                axleInfo.rightWheel.brakeTorque = 500;
                leftWheel.emitting = true;
                rightWheel.emitting = true;
            }

            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = -motor; //здесь в зависимости от локальных координат выбираем - или +
                axleInfo.rightWheel.motorTorque = -motor;  //здесь в зависимости от локальных координат выбираем - или +              
            }
            VisualWheels(axleInfo.leftWheel);
            VisualWheels(axleInfo.rightWheel);
        }
    }

    public void StopOn()
    {
        isBreak = true;
    }
    public void StopOff()
    {
        isBreak = false;
    }

    public void StartRace()
    {
        SceneManager.LoadScene("Drift Track");
    }
    public void ExitRace()
    {
        raceUI.SetActive(false);
    }


    public void VisualWheels(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }
        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }


    public void ExitCar()
    {
        player.SetActive(true);
        player.transform.parent = null;
        player.GetComponent<PlayerMove>().buttonExit.SetActive(false);
        player.GetComponent<PlayerMove>().buttonSit.SetActive(true);
        player.GetComponent<PlayerMove>().ui[0].SetActive(true);
        player.GetComponent<PlayerMove>().ui[1].SetActive(false);
        player.transform.rotation = Quaternion.identity;
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartRace"))
        {
            raceUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StartRace"))
        {
            ExitRace();
        }
    }


}


[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // присоединено ли колесо к мотору?
    public bool steering; // поворачивает ли это колесо?
}
