using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScript : MonoBehaviour
{
    public static TextManager tm;
    public GameObject hood;
    private string[] statements = {
        "Get Out!",
        "Take of the Hood",
        "Leave",
        "Escape",
        "Return Home",
        "Break Free",
        "Get to Freedom",
        "Find the way out"
    };
    private bool ready;
    void Start()
    {
        tm = TextManager.Instance;
    }
    void FixedUpdate()
    {
        if(ready)hood.transform.position = Vector3.MoveTowards(hood.transform.position,new Vector3(0,15,0),100*Time.deltaTime);
    }
    public void Hover()
    {
        tm.StartTextBox(new string[] {statements[Random.Range(0,statements.Length)]});
    }

    public void EndHover()
    {
        tm.StartTextBox(new string[] {"Start"});
    }

    public void OnClick()
    {
        ready = true;
        StartCoroutine(StartEnd());
    }

    IEnumerator StartEnd()
    {
        yield return new WaitForSeconds(1);
        tm.StartTextBox(new string[] {"So... You're Gone"});
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}
