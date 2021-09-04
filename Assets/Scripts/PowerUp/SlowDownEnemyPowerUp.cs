using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownEnemyPowerUp : MonoBehaviour, IPowerUp
{    
    [SerializeField] private float duration = 5.0f;        
    [SerializeField] private float speedModifier = 0.5f;        

    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Collider2D collider;        

    public void Implement(PlayerController player)
    {
        renderer.enabled = false;
        collider.enabled = false;
        if (speedModifier > 1)
        {
            GameManager.Powers.AddToList(PowerUpType.SpeedUpEnemy);
        }
        else
        {
            GameManager.Powers.AddToList(PowerUpType.SlowDownEnemy);
        }
        StartCoroutine(SlowDown(GameObject.Find("Enemy").GetComponent<EnemyBehaviour>()));
    }

    private IEnumerator SlowDown(EnemyBehaviour enemy)
    {        
        enemy.SpeedModifier = speedModifier;
        yield return new WaitForSeconds(duration);
        enemy.SpeedModifier = 1.0f;
        if (speedModifier > 1)
        {
            GameManager.Powers.RemoveFromList(PowerUpType.SpeedUpPlayer);
        }
        else
        {
            GameManager.Powers.RemoveFromList(PowerUpType.SlowDownEnemy);
        }
        Destroy(gameObject);
    }
}
