using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionPowerUp : MonoBehaviour, IPowerUp
{    
    [SerializeField] private float duration = 3.0f;        
    [SerializeField] private bool canProtectPlayer = true;        

    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Collider2D collider;         

    public void Implement(PlayerController player)
    {
        renderer.enabled = false;
        collider.enabled = false;        
        if (canProtectPlayer)
        {
            GameManager.Powers.AddToList(PowerUpType.ProtectPlayer);                
            StartCoroutine(Protect(player));
        }
        else
        {
            GameManager.Powers.AddToList(PowerUpType.ProtectEnemy);                
            StartCoroutine(Protect(GameObject.Find("Farmer").GetComponent<EnemyBehaviour>()));
        }
        
    }

    private IEnumerator Protect(PlayerController player)
    {        
        player.IsInvulnerable = true;
        yield return new WaitForSeconds(duration);
        player.IsInvulnerable = false;
        GameManager.Powers.RemoveFromList(PowerUpType.ProtectPlayer);        
        Destroy(gameObject);
    }

    private IEnumerator Protect(EnemyBehaviour enemy)
    {        
        enemy.IsInvulnerable = true;
        yield return new WaitForSeconds(duration);
        enemy.IsInvulnerable = false;
        GameManager.Powers.RemoveFromList(PowerUpType.ProtectEnemy);        
        Destroy(gameObject);
    }
}
