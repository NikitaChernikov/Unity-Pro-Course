using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject head;
    Animator anim;
    Rigidbody[] childrenRb;
    

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        childrenRb = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in childrenRb)
        {
            rb.isKinematic = true;
            rb.tag = "enemy_ragdoll";
        }
        head.tag = "ragdoll_head";
    }

    public void Death(bool gravity)
    {
        foreach(Rigidbody rb in childrenRb)
        {
            rb.isKinematic = false;
            rb.useGravity = gravity;
        }
        anim.enabled = false;
    }

    public void OffTelekinesis()
    {
        foreach (Rigidbody rb in childrenRb)
        {
            rb.useGravity = true;
        }
    }
}
