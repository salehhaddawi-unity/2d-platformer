using UnityEngine;

public class DoubleJumpTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerController = GameManager.instance.player.GetComponent<PlayerController>();

        playerController.allowedJumps = 2;
    }
}
