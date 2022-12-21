using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameObject blood;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject mainCamera;



    [SerializeField] Text HpText;
    [SerializeField] GameObject rifleStart;

    [SerializeField] Text ammoText;
    private int ammo;
    private int capacity;
    private int capacityMax = 30;

    [SerializeField] ButtonChanger[] buttonChanger;

    private int health;

    [SerializeField] GameObject pauseUI;
    bool isPause = false;

    bool shoot;
    float shootTimer;

    [SerializeField] float radius;
    Animator anim;

    float range = 100f;

    [SerializeField] ParticleSystem flash;
    [SerializeField] GameObject impact;
    private float impactForce = 1000f;

    [SerializeField] Text moneyText;
    public int money;

    [SerializeField] GameObject questTarget;
    [SerializeField] Dialogue dialogue;

    bool isDead = false;

    public bool GetPause()
    {
        return isPause;
    }

    public void ChangeHealth(int count)
    {
        health = health + count;
        HpText.text = health.ToString();

        if (health <= 0 && !isDead)
        {
            isDead = true;
            GameOver();
        }

    }

    void Start()
    {
        Time.timeScale = 1;
        anim = GetComponent<Animator>();
        AddAmmo(320);
        Reload();
        ChangeHealth(100);
        PlayerPrefs.SetInt("FindGrenade", 1);
        GetMoney(0);
    }

    private void Update()
    {
        Debug.DrawRay(rifleStart.transform.position, rifleStart.transform.forward * 100, Color.red);
        shootTimer += Time.deltaTime;
        if (shoot && shootTimer > 0.1f)
        {
            shootTimer = 0;
            if (capacity <= 0)
            {
                return;
            }
            capacity -= 1;
            ammoText.text = capacity + " / " + ammo;
            flash.Play();
            RaycastHit shootObj;

            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out shootObj, range))
            {
                if (!shootObj.collider.CompareTag("enemy_ragdoll") && !shootObj.collider.CompareTag("ragdoll_head") && !shootObj.collider.CompareTag("NPC"))
                {
                    GameObject inst = Instantiate(impact, shootObj.point, Quaternion.LookRotation(shootObj.normal));
                    Destroy(inst, 0.5f);
                }
                else
                {
                    GameObject inst = Instantiate(blood, shootObj.point, Quaternion.LookRotation(shootObj.normal));
                    Destroy(inst, 1f);
                }
                if(shootObj.collider.CompareTag("enemy_ragdoll"))
                {
                    shootObj.transform.GetComponentInParent<EnemyAI>().ChangeHealth(-40);
                    shootObj.rigidbody.AddForce(-shootObj.normal * impactForce);
                }
                if (shootObj.collider.CompareTag("ragdoll_head"))
                {
                    shootObj.transform.GetComponentInParent<EnemyAI>().ChangeHealth(-100);
                    shootObj.rigidbody.AddForce(-shootObj.normal * impactForce);
                }
                if (shootObj.rigidbody != null)
                {
                    shootObj.rigidbody.AddForce(-shootObj.normal * impactForce);
                }
                if (shootObj.collider.CompareTag("RedBarrel"))
                {
                    shootObj.collider.GetComponent<RedBarrel>().Boom();
                }
            }
        }
        //RaycastHit hit;
        //Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);
        //Debug.DrawRay(transform.position + new Vector3(0,1,0), transform.forward, Color.red);
        //if (Physics.Raycast(ray, out hit, 2f))
        //{
        //    //if (hit.collider.CompareTag("NPC"))
        //    //{
        //    //    hit.transform.GetComponent<NavigationAI>().Fear();
        //    //}
        //    if (hit.collider.CompareTag("HP"))
        //    {
        //        if (health >= 50)
        //        {
        //            ChangeHealth(100 - health);
        //        }
        //        else
        //        {
        //            ChangeHealth(50);
        //        }
        //        Destroy(hit.collider.gameObject);
        //    }
        //    if (hit.collider.CompareTag("Ammo"))
        //    {
        //        if (ammo >= 10)
        //        {
        //            AddAmmo(30 - ammo);
        //        }
        //        else
        //        {
        //            AddAmmo(20);
        //        }
        //        ammoText.text = capacity + " / " + ammo;
        //        Destroy(hit.collider.gameObject);
        //    }
        //}
    }

    public void AddAmmo(int count)
    {
        ammo += count;
        ammoText.text = capacity + " / " + ammo;
    }

    public void Reload()
    {
        int need = capacityMax - capacity;
        if (need <= ammo)
        {
            ammo -= need;
            capacity += need;
        }
        else
        {
            capacity += ammo;
            ammo = 0;
        }
        ammoText.text = capacity + " / " + ammo;
    }

    public void Pause()
    {
        if (!isPause)
        {
            isPause = true;
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            isPause = false;
            pauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void OnPointerDownShoot()
    {
        shoot = true;
    }
    public void OnPointerUpShoot()
    {
        shoot = false;
    }

    public void SayHello()
    {
        anim.SetTrigger("Hi");
    }

    public void HelloGuys(string say)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var people in colliders)
        {
            if (people.tag == "NPCTalk")
            {
                people.GetComponent<Animator>().SetTrigger("Hi");
                
                print(say);
            }
        }
    }

    public void GetMoney(int count)
    {
        money += count;
        moneyText.text = "Money: " + money.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Grenade"))
        {
            if (PlayerPrefs.GetInt("FindGrenade") == 2)
            {
                dialogue.target.transform.position = questTarget.transform.position;
                Destroy(collision.gameObject);
                PlayerPrefs.SetInt("FindGrenade", 3);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            GameOver();
        }
        if (other.CompareTag("NPCTalk"))
        {
            foreach (ButtonChanger button in buttonChanger)
            {
                button.SetNpc(other.gameObject);
            }
        }
    }

    public void GameOver()
    {
        anim.SetTrigger("death");
        gun.SetActive(false);
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(2);
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }

}