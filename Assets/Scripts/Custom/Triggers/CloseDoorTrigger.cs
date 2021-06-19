using UnityEngine;

public class CloseDoorTrigger : MonoBehaviour
{
    private Door door;

    private void Start()
    {
        door = GetComponentInParent<Door>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && door.isOpen)
        {
            door.Close();
        }
    }
}
