using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowsPuzzle : MonoBehaviour
{
    public string leftArrowSpriteName;
    public string rightArrowSpriteName;
    public string upArrowSpriteName;
    public string downtArrowSpriteName;

    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip successSound;
    public AudioClip failureSound;

    public RectTransform parent;
    public RectTransform wrongEffect;
    public Slider slider;
    public TextMeshProUGUI text;

    public float wrongPenalty = 0.3f; 

    private InputManager input;
    private PlayerController playerController;
    private bool running;
    private Stack<string> commands;
    private new AudioSource audio;

    private System.Action successCallback;
    private System.Action failCallback;

    public static ArrowsPuzzle instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        input = FindObjectOfType<InputManager>();
        playerController = GameManager.instance.player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (running)
        {
            ProcessArrowsInput();

            if (commands.Count == 0)
            {
                Success();
            }

            slider.value -= Time.deltaTime;

            if (slider.value == 0)
            {
                Fail();
            }
        }
    }

    public bool isRunning()
    {
        return running;
    }

    public void StartPuzzle(int count, float time, System.Action callbackSuccess, System.Action callbackFailure)
    {
        commands = GenerateCommands(count);

        parent.gameObject.SetActive(true);

        slider.maxValue = time;
        slider.value = time;

        audio.Play();

        RenderCommands();

        running = true;

        successCallback = callbackSuccess;
        failCallback = callbackFailure;
    }

    public void StopPuzzle()
    {
        audio.Stop();

        Fail();
    }

    private void Success()
    {
        running = false;

        text.text = "";

        parent.gameObject.SetActive(false);

        audio.Stop();
        audio.PlayOneShot(successSound);
        
        successCallback();
    }

    private void Fail()
    {
        running = false;

        text.text = "";

        parent.gameObject.SetActive(false);

        audio.Stop();
        audio.PlayOneShot(failureSound);

        failCallback();
    }

    private IEnumerator ShowWrongEffect()
    {
        wrongEffect.gameObject.SetActive(true);

        slider.value -= wrongPenalty;

        audio.PlayOneShot(wrongSound);

        yield return new WaitForSeconds(0.05f);

        wrongEffect.gameObject.SetActive(false);
    }

    private void CorrectCommand()
    {
        commands.Pop();
        RenderCommands();
        audio.PlayOneShot(correctSound);
    }

    private void ProcessArrowsInput()
    {
        if (
            (input.leftArrow && commands.Peek() == "<sprite name=\"" + leftArrowSpriteName + "\">") ||
            (input.rightArrow && commands.Peek() == "<sprite name=\"" + rightArrowSpriteName + "\">") || 
            (input.upArrow && commands.Peek() == "<sprite name=\"" + upArrowSpriteName + "\">") ||
            (input.downArrow && commands.Peek() == "<sprite name=\"" + downtArrowSpriteName + "\">")
            )
        {
            CorrectCommand();
        }

        else if (input.leftArrow || input.rightArrow || input.upArrow || input.downArrow)
        {

            StartCoroutine("ShowWrongEffect");
        }
    }

    private Stack<string> GenerateCommands(int count)
    {
        Stack<string> commands = new Stack<string>();

        for (int i = 0; i < count; i++)
        {
            var arrowSpriteName = "";
            var rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    arrowSpriteName = leftArrowSpriteName;
                    break;
                case 1:
                    arrowSpriteName = rightArrowSpriteName;
                    break;
                case 2:
                    arrowSpriteName = upArrowSpriteName;
                    break;
                case 3:
                    arrowSpriteName = downtArrowSpriteName;
                    break;
            }

            commands.Push("<sprite name=\""+arrowSpriteName+"\">");
        }

        return commands;
    }

    private void RenderCommands()
    {
        text.text = "  " + string.Join("", commands);
    }
}
