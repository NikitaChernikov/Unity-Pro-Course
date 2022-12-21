using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : NavigationAI
{
    [SerializeField] [Range(0, 360)] private float ViewAngle = 90f; // угол обзора врага
    [SerializeField] private float ViewDistance = 15f; //дальность обзора врага      
    private Transform Target; // объект за которым будем следить
    [SerializeField] GameObject rifleStart;
    [SerializeField] ParticleSystem flash;

    private Enemy ragdoll;
    float cooldown = 1;
    float timer = 0;

    private int health = 100;

    private void Awake()
    {
        Target = FindObjectOfType<PlayerController>().transform;
        ragdoll = GetComponent<Enemy>();
        
    }

    public void ChangeHealth(int count)
    {
        health += count;
        anim.SetTrigger("damage");
        if (health <= 0)
        {
            ragdoll.Death(true);
            agent.enabled = false;
            GetComponent<NavigationAI>().enabled = false;
            GetComponent<EnemyAI>().enabled = false;
        }
    }

    override protected void Update() //переписываем функцию
    {
        DrawView();
        float distanceToPlayer = Vector3.Distance(Target.transform.position, agent.transform.position);
        //расстояние от врага до персонажа
        if (IsInView()) // если цель в зоне видимости
        {
        if (distanceToPlayer >= 5f) // если расстояние больше 2 единиц
        {
            MoveToTarget(); // враг двигается к персонажу
            anim.SetBool("idle", false);
                
        }
        else
        {
            agent.isStopped = true; // если враг подошел близко к персонажу, то останавливаем его
            anim.SetBool("idle", true);
            timer += Time.deltaTime;
            if (timer > cooldown)
            {
                timer = 0;
                if (Physics.Raycast(rifleStart.transform.position, -rifleStart.transform.forward, out RaycastHit hit, 11))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        flash.Play();
                        Target.GetComponent<PlayerController>().ChangeHealth(-20);
                    }
                }
            }
        }
        }
        else //если цель пропала из зоны видимости
        {
            agent.isStopped = false; //возвращаем возможность передвигаться           
            base.Update(); //добавляем строки из функции Update основного скрипта
        }

    }

    private bool IsInView()
    {
        //угол между врагом и персонажем
        float currentAngle = Vector3.Angle(transform.forward, Target.position - transform.position);
        RaycastHit hit;
        //Пускаем Raycast от врага в сторону текущего положения персонажа на расстояние переменной ViewDistance
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Target.position - transform.position, out hit, ViewDistance))
        {
            //если угол между врагом и персонажем меньше угла обнаружения/2 И расстояние до персонажа меньше ViewDistance  И луч врезался именно во врага, а не в препятствие между ними
            if (currentAngle < (ViewAngle / 2f) && Vector3.Distance(transform.position, Target.position) <= ViewDistance && hit.transform == Target.transform)
            {
                return true; // то bool IsInView = true
            }
        }
        return false; //иначе bool IsInView = false
    }

    private void MoveToTarget()
    {
        agent.isStopped = false;
        agent.speed = 3.5f;
        agent.SetDestination(Target.position);
    }

    private void DrawView()
    {
        Debug.DrawRay(rifleStart.transform.position, -rifleStart.transform.forward * 11, Color.red);
        Vector3 left = transform.position + Quaternion.Euler(new Vector3(0, ViewAngle / 2f, 0)) * (transform.forward * ViewDistance);
        Vector3 right = transform.position + Quaternion.Euler(-new Vector3(0, ViewAngle / 2f, 0)) * (transform.forward * ViewDistance);
        Debug.DrawLine(transform.position, left, Color.yellow);
        Debug.DrawLine(transform.position, right, Color.yellow);
    }

    
}
