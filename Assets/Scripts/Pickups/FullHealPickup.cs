using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHealPickup : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other) 
   {
        if (other.CompareTag("Player")) {
            var playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.ReceiveHealing(playerHealth.maximumHealth);
            Destroy(this.gameObject);
        }
   }
}
