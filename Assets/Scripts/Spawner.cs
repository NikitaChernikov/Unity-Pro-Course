using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject ammoBox;
    [SerializeField] private GameObject hpBox;
    [SerializeField] float radius = 50;
    [SerializeField] float cooldown = 0.5f;

    private float timer = 0;

    void Start()
    {
        StartCoroutine(WaitAndSpawn(cooldown));
    }

    IEnumerator WaitAndSpawn(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Spawn(ammoBox);
            Spawn(hpBox);
        }
    }

    public void Spawn(GameObject box)
    {
        GameObject buf = Instantiate(box);
        float x = Random.Range(-radius, radius);
        float z = Random.Range(-radius, radius);
        buf.transform.position = transform.position + new Vector3(x, 0, z);
    }
}