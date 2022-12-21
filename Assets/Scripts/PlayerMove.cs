using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] float speed = 5;
    [SerializeField] GameObject sitInCarButton;
    Rigidbody rb;
    Vector3 direction;

    float cooldown = 0.4f;
    float timer = 0.4f;

    bool isGrounded;

    Animator anim;

    [SerializeField] GameObject car;
    [SerializeField] Transform point;
    [SerializeField] Camera carCamera;
    [SerializeField] float radius;
    CarController carController;
    bool isDriver;
    NavMeshAgent agent;
    [SerializeField] public GameObject buttonSit;
    [SerializeField] public GameObject buttonExit;
    [SerializeField] public GameObject[] ui;

    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        carController = car.GetComponent<CarController>();
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector3(0, 15, 0), ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, car.transform.position) <= radius)
        {
            sitInCarButton.SetActive(true);
        }
        else
        {
            sitInCarButton.SetActive(false);
        }
        if (!isDriver)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;

            if (horizontal != 0 || vertical != 0)
            {
                timer += Time.deltaTime;
                if (timer > cooldown)
                {
                    timer = 0;
                    GetComponent<AudioSource>().Play();
                }
            }
            direction = transform.TransformDirection(horizontal, 0, vertical);
            anim.SetFloat("move", Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical)));
        }
        if (isDriver && agent.remainingDistance < .25f)
        {
            Invoke("SwitchCamera", 1f);
            isDriver = false;
            agent.enabled = false;
            transform.LookAt(car.transform);
            carController.enabled = true;
        }
    }

    private void SwitchCamera()
    {
        carCamera.enabled = true;
        gameObject.SetActive(false);
        gameObject.transform.SetParent(car.transform);
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            rb.MovePosition(transform.position + speed * direction * Time.deltaTime);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other != null)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }

    public void SitInCar()
    {
        StartCoroutine("IInCar");
    }

    IEnumerator IInCar()
    {
        if (!isDriver)
        {
            agent.enabled = true;
            agent.SetDestination(point.position);
            yield return new WaitForSeconds(1);
            ui[1].SetActive(true);
            ui[0].SetActive(false);
            isDriver = true;
            anim.SetFloat("move", 1f);
        }
    }

}

