using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;    
    
    // Creation bounces.
    [SerializeField] private int row = 5;
    [SerializeField] private int col = 0;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Vector3 vertDirection; // Diection to move up the level.
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Joystick joystick;
    
    private float _speedModifier = 1.0f;
    
    public IEnumerator SpeedUp(float duration, float speedModifier)
    {        
        _speedModifier = speedModifier;
        yield return new WaitForSeconds(duration);
        _speedModifier = 1;     
    }

    public void TakeDamage()
    {
        gameManager.GameOver();
    }
        
    public void DropBomb()
    {
        Vector2 pos = transform.position;
        gameManager.FromPositionToGrid(pos, out row, out col);
        gameManager.FromGridToPosition(row, col, ref pos);
        Instantiate(bombPrefab, pos, Quaternion.identity);    
    }
    
    public void Start()
    {
        // Seelect random position in bounds
        // while position is not correct.
        Vector2 pos = new Vector2();
        while (!gameManager.FromGridToPosition(Random.Range(0, row), Random.Range(0, col), ref pos))
        {

        }
        transform.position = pos;   
    }    

    private void Update()
    {
        Move();
        
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     DropBomb();
        // }
    }
    // Code in comments for testing on PC.
    private void Move()
    {   
        // float hor = Input.GetAxis("Horizontal");        
        // float vert = Input.GetAxis("Vertical");        
        float hor = joystick.Horizontal;        
        float vert = joystick.Vertical;

        transform.Translate(Vector2.right * hor * speed * Time.deltaTime * _speedModifier);
        if (hor > 0)
        {
            animator.SetInteger("DirectionX", 1);
        }
        else if (hor < 0)
        {
            animator.SetInteger("DirectionX", -1);
        }
        else
        {
            animator.SetInteger("DirectionX", 0);            
        }

        transform.Translate(vertDirection * vert * speed * Time.deltaTime * _speedModifier);
        if (vert > 0)
        {
            animator.SetInteger("DirectionY", 1);
        }
        else if (vert < 0)
        {
            animator.SetInteger("DirectionY", -1);
        }
        else
        {
            animator.SetInteger("DirectionY", 0);            
        }        
    }
        
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameManager.GameOver();
        }    
    }    
}

