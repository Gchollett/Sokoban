using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    private Animator animator;
    private bool lit = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate() {
        transform.GetChild(0).gameObject.SetActive(Physics2D.OverlapCircleAll(transform.position,.5f).Any(collider => collider.gameObject.tag == "Player") && !lit);
    }
    public void Light(){
        lit = true;
        StartCoroutine(StartLight());
    }
    private IEnumerator StartLight(){
        animator.SetInteger("Status",1);
        yield return new WaitForSeconds(1);
        animator.SetInteger("Status",2);
        yield return new WaitForSeconds(1);
        Explode();
    }

    public void Explode(){
            animator.SetInteger("Status",3);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(waitForAnimation());
    }

    bool AnimatorIsNotPlaying(){
        return 1 <= animator.GetCurrentAnimatorStateInfo(0).normalizedTime && animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion");
    }

    private IEnumerator waitForAnimation(){
        yield return new WaitForSeconds(.5f);
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position,1f)){
            Vector3 distance = math.abs(collider.gameObject.transform.position - transform.position);
            if((math.round(distance.x) == 1 || math.round(distance.y) == 1) &&  math.round(distance.y) != math.round(distance.x)){
                if(collider.gameObject.tag == "Breakable"){
                    collider.gameObject.GetComponent<Crates>().Break();
                }
                if(collider.gameObject.tag == "Interactable"){
                    collider.gameObject.GetComponent<Bombs>().Explode();
                }
                if(collider.gameObject.tag == "Player"){
                    collider.gameObject.GetComponent<PlayerBehavior>().Die();
                }
                if(collider.gameObject.layer == 9){
                    collider.gameObject.GetComponent<MetalBoxes>().MoveUntilStopped(collider.gameObject.transform.position - transform.position);
                }
            }
        }
        yield return new WaitUntil(() => AnimatorIsNotPlaying());
        Destroy(gameObject);
    }
}
