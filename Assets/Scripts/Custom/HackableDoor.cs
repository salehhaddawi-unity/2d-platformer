using UnityEngine;

public class HackableDoor : MonoBehaviour
{
    [Min(1)] public int arrowsCount = 6;
    [Min(1)] public float time = 4;

    private Door door;
    private bool isFinished = false;
    private GameObject doorHackDevice;

    private void Start()
    {
        door = GetComponent<Door>();
        doorHackDevice = GameObject.FindGameObjectWithTag("HackingDevice");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkCanHack() && !door.isOpen && !isFinished)
        {
            ArrowsPuzzle.instance.StartPuzzle(arrowsCount, time, Success, Fail);
        }

        if (!door.isOpen && isFinished)
        {
            door.Open();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (checkCanHack() && !door.isOpen && !isFinished)
        {
            if (ArrowsPuzzle.instance.isRunning())
            {
                ArrowsPuzzle.instance.StopPuzzle();
            }
        }
    }

    private void Success()
    {
        door.Open();

        GetComponent<Animator>().SetBool("hacked", true);

        isFinished = true;
    }

    private void Fail()
    {
        print("FAIL");
    }

    private bool checkCanHack()
    {
        return !doorHackDevice.activeInHierarchy;
    }
}
