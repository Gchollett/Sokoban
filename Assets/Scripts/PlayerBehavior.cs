using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    public Transform target;
    public LayerMask WallLayer;
    public LayerMask MovableLayer;
    public LayerMask UnmovableLayer;
    public LayerMask DoorLayer;
    public LayerMask LockedLayer;
    public int speed;
    private Animator animator;
    private GameObject movable;
    private Vector3 movableTarget;
    private int sceneIndex = 1;
    void Start()
    {
        target.position = math.round(transform.position*10)*.1f;
        movableTarget = target.position;
        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if(movable){
            movable.transform.position = Vector3.MoveTowards(movable.transform.position,movableTarget,speed * Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position,target.position,speed * Time.deltaTime);
        if(target.position == transform.position){
            animator.SetInteger("Direction",0);
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 moveVec = value.Get<Vector2>();
        if(Physics2D.OverlapCircle(transform.position + new Vector3(moveVec.x,moveVec.y,0),.4f,DoorLayer) && !Physics2D.OverlapCircle(transform.position + new Vector3(moveVec.x,moveVec.y,0),.4f,LockedLayer)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        if(target.position != transform.position || 
        Physics2D.OverlapCircle(transform.position + new Vector3(moveVec.x,moveVec.y,0),.4f,WallLayer) ||
        Physics2D.OverlapCircle(transform.position + new Vector3(moveVec.x,moveVec.y,0),.4f,UnmovableLayer)) return;
        if(Physics2D.OverlapCircle(transform.position + new Vector3(moveVec.x,moveVec.y,0),.4f,MovableLayer)){
            if(Physics2D.OverlapCircle(transform.position + 2*new Vector3(moveVec.x,moveVec.y,0),.4f,MovableLayer)||
            Physics2D.OverlapCircle(transform.position + 2*new Vector3(moveVec.x,moveVec.y,0),.4f,WallLayer)||
            Physics2D.OverlapCircle(transform.position + 2*new Vector3(moveVec.x,moveVec.y,0),.4f,UnmovableLayer)
            )return;
            else{
               movable = Physics2D.OverlapCircle(transform.position + new Vector3(moveVec.x,moveVec.y,0),.4f,MovableLayer).gameObject;
            }
        }else{
            movable = null;
        }
        if(math.abs(moveVec.x) >= math.abs(moveVec.y)) animator.SetInteger("Direction",2*(int)moveVec.x);
        else animator.SetInteger("Direction",(int)moveVec.y);
        target.position = transform.position + new Vector3(moveVec.x,moveVec.y,0);
        if(movable) movableTarget = transform.position + 2 * new Vector3(moveVec.x,moveVec.y,0);
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        StartCoroutine(waitForAnimationToRestart());
    }

    bool AnimatorIsNotPlaying(){
        return animator.GetCurrentAnimatorStateInfo(0).length <= animator.GetCurrentAnimatorStateInfo(0).normalizedTime && animator.GetCurrentAnimatorStateInfo(0).IsName("Death");
    }

    private IEnumerator waitForAnimationToRestart()
    {
        yield return new WaitUntil(() => AnimatorIsNotPlaying());
        Restart();
    }

    void OnInteract()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position,1f)){
            if(collider.gameObject.tag == "Interactable"){
                collider.gameObject.GetComponent<Bombs>().Light();
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnRestart()
    {
        Restart();
    }
}
