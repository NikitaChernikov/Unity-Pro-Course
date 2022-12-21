using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : NavigationAI
{
    [SerializeField] [Range(0, 360)] private float ViewAngle = 90f; // ���� ������ �����
    [SerializeField] private float ViewDistance = 15f; //��������� ������ �����      
    private Transform Target; // ������ �� ������� ����� �������
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

    override protected void Update() //������������ �������
    {
        DrawView();
        float distanceToPlayer = Vector3.Distance(Target.transform.position, agent.transform.position);
        //���������� �� ����� �� ���������
        if (IsInView()) // ���� ���� � ���� ���������
        {
        if (distanceToPlayer >= 5f) // ���� ���������� ������ 2 ������
        {
            MoveToTarget(); // ���� ��������� � ���������
            anim.SetBool("idle", false);
                
        }
        else
        {
            agent.isStopped = true; // ���� ���� ������� ������ � ���������, �� ������������� ���
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
        else //���� ���� ������� �� ���� ���������
        {
            agent.isStopped = false; //���������� ����������� �������������           
            base.Update(); //��������� ������ �� ������� Update ��������� �������
        }

    }

    private bool IsInView()
    {
        //���� ����� ������ � ����������
        float currentAngle = Vector3.Angle(transform.forward, Target.position - transform.position);
        RaycastHit hit;
        //������� Raycast �� ����� � ������� �������� ��������� ��������� �� ���������� ���������� ViewDistance
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Target.position - transform.position, out hit, ViewDistance))
        {
            //���� ���� ����� ������ � ���������� ������ ���� �����������/2 � ���������� �� ��������� ������ ViewDistance  � ��� �������� ������ �� �����, � �� � ����������� ����� ����
            if (currentAngle < (ViewAngle / 2f) && Vector3.Distance(transform.position, Target.position) <= ViewDistance && hit.transform == Target.transform)
            {
                return true; // �� bool IsInView = true
            }
        }
        return false; //����� bool IsInView = false
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
