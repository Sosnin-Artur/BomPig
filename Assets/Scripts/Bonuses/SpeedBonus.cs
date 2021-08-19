using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBonus : MonoBehaviour
{
    [SerializeField] float duration = 1.5f;
    [SerializeField] float speedModifier = 1.5f;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Realize(other));            
        }    
    }

    private IEnumerator Realize(Collider2D other)
    {
        gameObject.SetActive(false);
        yield return StartCoroutine(other.GetComponent<PlayerController>().SpeedUp(duration, speedModifier));
        Destroy(gameObject);
    }
}
