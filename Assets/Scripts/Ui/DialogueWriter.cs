using BrunoMikoski.AnimationSequencer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueWriter : MonoBehaviour
{
    public static DialogueWriter instance;
    public TMP_Animated sourceText;
    public AnimationSequencerController animator;

    List<string> sentencesToRead;
    bool isReading,sentenceFinished;
    int sentenceIndex;

    UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        instance=this;
    }

    void Start()
    {
        sourceText.OnDialogueFinished.Subscribe(eventHandler, () => sentenceFinished = true);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && sentenceFinished)
        {
            NextSentence();
        }
    }

    public void ReadSentences(string[] sentences)
    {

    }

    void PressedNext()
    {

    }
    void NextSentence()
    {
        sentenceFinished = false;
        sentenceIndex++;
        if (sentenceIndex > sentencesToRead.Count - 1)
        {
            HideDialogue();
            return;
        }
            sourceText.ReadText(sentencesToRead[sentenceIndex]);
    }

    void ShowDialogue()
    {
        animator.Play();
    }

    void HideDialogue()
    {
        animator.PlayBackwards();
    }

}
