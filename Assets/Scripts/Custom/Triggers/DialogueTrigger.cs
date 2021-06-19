using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogueTrigger : MonoBehaviour
{
    public int priority = 0;
    public bool hideOnExit = true;
    public bool once = true;
    public DialogueData[] onEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (this.onEnter != null)
            {
                foreach (var dialogue in onEnter)
                {
                    DialogueManager.instance.ShowDialogue(dialogue);
                }
            }

            if (this.once)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && this.hideOnExit)
        {
            DialogueManager.instance.HideDialogue(this.priority);
        }
    }
}
