using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBarrel : MonoBehaviour
{
    [SerializeField] float radius = 50;
    [SerializeField] GameObject particle;

    

    public void Boom()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        var enemies = FindObjectsOfType<EnemyAI>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(transform.position, enemies[i].transform.position) < radius)
            {
                enemies[i].ChangeHealth(-100);
            }
        }

        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            player.ChangeHealth(-100);
        }
        GameObject boom = Instantiate(particle);
        boom.transform.position = transform.position;
        Boom2();
        Destroy(gameObject);
    }

    public void Boom2()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rb = colliders[i].attachedRigidbody;
            if (rb && rb.tag != "Player")
            {
                rb.AddExplosionForce(1000, transform.position, radius);
            }
        }
    }
}
