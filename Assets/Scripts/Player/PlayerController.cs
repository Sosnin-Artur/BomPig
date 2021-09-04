using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;    
    
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Vector3 vertDirection; // Diection to move up the level.
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Joystick joystick;
    [SerializeField] private AudioClip pickUpSound;    

    private float _speedModifier = 1.0f;            
    public float SpeedModifier
    {
        get
        {
            return _speedModifier;
        }
        set
        {
            if (value > 0)
            {
                _speedModifier = value;
            }
        }    
    }
     
    private bool _isInvulnerable;
    public bool IsInvulnerable
    {
        get
        {
            return _isInvulnerable;
        }
        set
        {
            _isInvulnerable = value;
        }    
    }

    public void TakeDamage()
    {
        if (!_isInvulnerable)
        {
            gameManager.GameOver();
        }        
    }
        
    public void DropBomb()
    {        
        if (!GameManager.IsPaused)
        {
            Instantiate(bombPrefab, transform.position, Quaternion.identity);       
        }        
    }
    
    public void Restart()
    {
        Vector2 pos = new Vector2();        
        int rowSize = GameManager.Location.Data.GetUpperBound(0);
        
        while (!GameManager.Location.FromGridToPosition(UnityEngine.Random.Range(0, rowSize), 0, ref pos))
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
            TakeDamage();
        }    
    }    

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("PowerUp"))
        {            
            GameManager.Audio.PlaySound(pickUpSound);
            other.GetComponent<IPowerUp>().Implement(this);
        }    
    }        
}

