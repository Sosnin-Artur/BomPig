using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Bush"))
        {
            Destroy(other.gameObject);
            return;
        }        
        other.SendMessage("TakeDamage");
    }
}
