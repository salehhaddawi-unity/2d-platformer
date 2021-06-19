using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private TextMeshPro dialogueText;
    private int currentDialoguePriority = 0;
    private Coroutine showDialogueCorotine;

    private void Start() {
        dialogueText = GetComponentInChildren<TextMeshPro>();
    }

    private void Awake()
    {
        // When this component is first added or activated, setup the global reference
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ShowDialogue(DialogueData data)
    {
        if (this.currentDialoguePriority > data.priority)
        {
            return;
        }

        this.showDialogueCorotine = StartCoroutine(_ShowDialogue(data));
    }

    private IEnumerator _ShowDialogue(DialogueData data)
    {
        yield return new WaitForSeconds(data.delay);

        this.currentDialoguePriority = data.priority;

        if (this.currentDialoguePriority <= data.priority)
        {
            this.dialogueText.text = data.text;

            this.dialogueText.enabled = true;
        }

        yield return new WaitForSeconds(data.time);

        HideDialogue(data.priority);
    }

    public void HideDialogue(int priority) 
    {
        if (this.currentDialoguePriority > priority)
        {
            return;
        }

        dialogueText.enabled = false;

        dialogueText.text = "";

        this.currentDialoguePriority = 0;
    }
}

[System.Serializable]
public struct DialogueData
{
    public string name;
    public string text;
    public float time;
    public float delay;
    public int priority;
}
