using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{    
    [SerializeField] private float explosionDuration = 2.0f;
    [SerializeField] private int explosionRange = 2;

    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private GameObject explosionSize;    
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private Vector2 verticalOffset;    

    // Create explosions in directions.
    public IEnumerator Explode()
    {        
        yield return new WaitForSeconds(explosionDuration);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject(transform.position);                
        GameManager.Audio.PlaySound(explosionSound);
        CreateExplosions(Vector2.left);
        CreateExplosions(verticalOffset);
        CreateExplosions(Vector2.right);
        CreateExplosions(-verticalOffset);
        yield return StartCoroutine(SetUnActive(explosion, explosionDuration));
        Destroy(gameObject);
    }
    
    private void Start()
    {
        StartCoroutine(Explode());
    }

    private void OnTriggerExit2D(Collider2D other)
    {        
        collider.isTrigger = false;
    }    

    // Create explosions in the direction.
    private void CreateExplosions(Vector2 direction)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();

        Vector2 explosionDimensions = explosionSize.GetComponent<SpriteRenderer>().bounds.size;
        Vector2 explosionPosition = (Vector2)transform.position + (explosionDimensions.x * direction);
        

        for (int i = 1; i < explosionRange; ++i)
        {
            // Checking for collisions.
            Collider2D[] colliders = new Collider2D[4];
            Physics2D.OverlapBox(explosionPosition, explosionDimensions, 0.0f, contactFilter, colliders);
            
            bool foundObstacle = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider)
                {   
                    if (collider.CompareTag("Player") || collider.CompareTag("Bomb"))
                    {
                        continue;
                    }
                    foundObstacle = true;                                                     
                }
            }            

            GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject(explosionPosition);
            StartCoroutine(SetUnActive(explosion, explosionDuration));
            if (foundObstacle)
            {
                break;
            }
            explosionPosition += explosionDimensions.x * direction;
            
        }
    }    
    
    private IEnumerator SetUnActive(GameObject obj, float time)
    {                                   
        yield return new WaitForSeconds(time);                                                          
        obj.SetActive(false);
    }
}
