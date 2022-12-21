using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    [SerializeField] public List<Transform> points = new List<Transform>();
    //[SerializeField] Transform player;
    protected Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetDestination();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if(agent.remainingDistance < 0.25f)
        {
            StartCoroutine("Idle");
        }
    }

    public void SetDestination()
    {
        Vector3 newTarget = points[Random.Range(0, points.Count)].position;
        agent.SetDestination(newTarget);
    }

    public void Fear()
    {
        StartCoroutine("FearCor");
    }

    IEnumerator Idle()
    {
        agent.speed = 0;
        SetDestination();
        anim.SetBool("idle", true);
        yield return new WaitForSeconds(5);
        agent.speed = 3.5f;
        anim.SetBool("idle", false);
    }

    IEnumerator FearCor()
    {
        agent.speed = 10;
        anim.SetBool("fear", true);
        SetDestination();
        yield return new WaitForSeconds(3);
        agent.speed = 3.5f;
        anim.SetBool("fear", false);
    }
}
