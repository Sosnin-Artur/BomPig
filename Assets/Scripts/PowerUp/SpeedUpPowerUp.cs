using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpPowerUp : MonoBehaviour, IPowerUp
{    
    [SerializeField] private float duration = 1.5f;    
    [SerializeField] private float speedModifier = 1.5f;     

    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Collider2D collider;           

    public void Implement(PlayerController player)
    {
        renderer.enabled = false;
        collider.enabled = false;
        if (speedModifier > 1)
        {
            GameManager.Powers.AddToList(PowerUpType.SpeedUpPlayer);
        }
        else
        {
            GameManager.Powers.AddToList(PowerUpType.SlowDownPlayer);
        }
        StartCoroutine(SpeedUp(player));
    }

    private IEnumerator SpeedUp(PlayerController player)
    {        
        player.SpeedModifier = speedModifier;
        yield return new WaitForSeconds(duration);
        player.SpeedModifier = 1;
        if (speedModifier > 1)
        {
            GameManager.Powers.RemoveFromList(PowerUpType.SpeedUpPlayer);
        }
        else
        {
            GameManager.Powers.RemoveFromList(PowerUpType.SlowDownPlayer);
        }
        Destroy(gameObject);
    }
}
