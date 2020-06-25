﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    public GameObject continueButton;
    public GameObject player;
    private Queue<string> sentences;
    
    
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, bool freezePlayer)
    {
        if (freezePlayer) {
            if (player.GetComponent<PlayerMovement>())
                player.GetComponent<PlayerMovement>().dialogueFreeze = true;
            else
                player.GetComponent<TutorialMovement>().dialogueFreeze = true;
        }
            

        animator.SetBool("IsOpen", true);
        
        nameText.text = dialogue.name;
        
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        continueButton.SetActive(false);

        if (sentences.Count == 0)
        {
            if (player.GetComponent<PlayerMovement>())
                player.GetComponent<PlayerMovement>().enabled = true;
            else
                player.GetComponent<TutorialMovement>().enabled = true;

            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        continueButton.SetActive(true);
    }
    
    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        // player.GetComponent<PlayerMovement>().dialogueFreeze = false;
        
        if (player.GetComponent<PlayerMovement>())
            player.GetComponent<PlayerMovement>().dialogueFreeze = false;
        else
            player.GetComponent<TutorialMovement>().dialogueFreeze = false;
    }

   
 
}
