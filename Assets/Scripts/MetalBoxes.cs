using UnityEngine;

public class MetalBoxes : MonoBehaviour
{
    public LayerMask WallLayer;
    public LayerMask MovableLayer;
    public LayerMask UnmovableLayer;
    public int speed;
    private Vector3 target;
    void Start()
    {
        target = transform.position;
    }
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position,target,speed * Time.deltaTime);
    }

    public void MoveUntilStopped(Vector3 direction)
    {
        Vector3 temp = transform.position;
        while(!Physics2D.OverlapCircle(temp+direction,.4f,WallLayer) &&
        !Physics2D.OverlapCircle(temp+direction,.4f,MovableLayer) &&
        !Physics2D.OverlapCircle(temp+direction,.4f,UnmovableLayer) &&
        Physics2D.OverlapCircle(temp+direction,.4f)?.gameObject.tag != "Player"){
            temp += direction;
        }
        if(Physics2D.OverlapCircle(temp+direction,.4f)?.gameObject.tag == "Player"){
            Physics2D.OverlapCircle(temp+direction,.4f).gameObject.GetComponent<PlayerBehavior>().Die();
        }
        Collider2D in_front_movable = Physics2D.OverlapCircle(temp+direction,.4f,MovableLayer);
        if(in_front_movable){
            if(in_front_movable.tag == "Interactable"){
                in_front_movable.GetComponent<Bombs>().Explode();
            }else{
                temp += direction;
                in_front_movable.gameObject.GetComponent<Crates>().Break();
            }
        }
        target = temp;
    }
}
