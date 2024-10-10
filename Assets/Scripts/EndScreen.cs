using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public Animator player;
    public static TextManager tm;

    void Start()
    {
        tm = TextManager.Instance;
        StartCoroutine(EndScreenAfterAnimation());
    }

     bool AnimatorIsNotPlaying(){
        return 1 <= player.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    IEnumerator EndScreenAfterAnimation(){
        yield return new WaitUntil(AnimatorIsNotPlaying);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        tm.StartTextBox(new string[] {"So... You're Done"});
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
}
