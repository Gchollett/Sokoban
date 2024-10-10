using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance;
    public TextMeshProUGUI text;
    private Queue<string> sentences;
    public bool ended{get;set;}
    void Awake()
    {
        Instance = this;
        sentences = new Queue<string>();
    }

    public void StartTextBox(string[] strings){
        ended = false;
        foreach (string sentence in strings){
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence(){
        if(sentences.Count == 0){
            EndTextBox();
            return;
        }
        string next = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(WriteDialogue(next));
    }

    void EndTextBox()
    {
        ended = true;
        text.text = "";
    }

    IEnumerator WriteDialogue(string sentence){
        text.text = "";
        foreach (char letter in sentence.ToCharArray()){
            text.text += letter;
            yield return null;
        }
    }
}
