using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBonus : MonoBehaviour
{
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private float speedModifier = 1.5f;
    
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Collider2D collider;
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Realize(other));            
        }    
    }

    private IEnumerator Realize(Collider2D other)
    {
        renderer.enabled = false;
        collider.enabled = false;

        yield return StartCoroutine(other.GetComponent<PlayerController>().SpeedUp(duration, speedModifier));
        Destroy(gameObject);
    }
}
