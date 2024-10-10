using UnityEngine;

public class Crates : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // animator = GetComponent<Animator>();
    }
    public void Break(){
        // animator.SetBool("Break",true);
        // GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}
