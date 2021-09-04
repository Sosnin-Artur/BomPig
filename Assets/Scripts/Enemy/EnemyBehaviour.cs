using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{    
    [SerializeField] private int lives = 2;
    [SerializeField] private float invulnerabilityDuration = 2.0f;
    [SerializeField] private float intelligenceChance = 0.3f;
    [SerializeField] private float speed = 3.0f;    
    [SerializeField] private float angrySpeed = 3.0f;  
    [SerializeField] private float timeToStart = 1.0f;    
    
    // Position on grid.
    [SerializeField] private int row = 5;
    [SerializeField] private int col = 16;
    
    // Number of steps to mive in a random direction.
    [SerializeField] private int minStepCount = 1;
    [SerializeField] private int maxStepCount = 5;

    [SerializeField] private Animator animator;
    [SerializeField] private GameManager gameManager;    
    [SerializeField] private Vector3 vertDirection;        
    [SerializeField] private GameObject target;        

    private float _curSpeed;
    private bool _isInvulnerable;         
    private bool _isAngry;      
    private Vector2 _moveDirectiron;        
    private Vector2[] _directions = {Vector2.up, Vector2.left, Vector2.down, Vector2.right};
    
    private int _rowSize;
    private int _colSize;
    private int[,] _map;
    
    public float SpeedModifier {get; set;}
    
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
            lives--;
            animator.SetBool("IsDirty", true);
            if (lives == 0)
            {
                gameManager.CompliteLevel();
                Destroy(gameObject);
            }        
            _isInvulnerable = true;
            
            StartCoroutine(UseInvulnerability());
        }                
    }    
    
    public void Restart()
    {
        Vector2 pos = new Vector2();        
        
        row = UnityEngine.Random.Range(0, _rowSize - 1);
        while (!GameManager.Location.FromGridToPosition(row, _colSize - 1, ref pos))
        {            
           row = UnityEngine.Random.Range(0, _rowSize - 1);
        }
        col = _colSize - 1;
        transform.position = pos; 
        StartCoroutine(ChangeDiretion());        
    }

    private void Start()
    {
        _curSpeed = speed;
        _rowSize = GameManager.Location.Data.GetUpperBound(0) + 1;
        _colSize = GameManager.Location.Data.GetUpperBound(1) + 1; 
            
        _map = new int[_rowSize, _colSize];        

        SpeedModifier = 1.0f;
    }         
    
    private IEnumerator Move(Vector2 direction, int stepCount)
    {        
        Vector2 pos = Vector2.zero;        
        for (int i = 0; i < stepCount; ++i)
        {
            if (GameManager.Location.FromGridToPosition(row - (int)direction.y, col + (int)direction.x, ref pos))
            {                                    
                _moveDirectiron = direction;                         
                yield return StartCoroutine(MoveTo(pos));                                                  
                row -= (int)direction.y;
                col += (int)direction.x;                                  
            }
        }        
    }

    private IEnumerator MoveTo(Vector2 target)
    {
        float hor = _moveDirectiron.x;
        float vert = _moveDirectiron.y;        
        
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
                
        while ((Vector2)transform.position != target)
        {                                             
            transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * _curSpeed * SpeedModifier);            
            yield return null;
        }        
    }

    private IEnumerator ChangeDiretion()
    {
        yield return new WaitForSeconds(timeToStart);
        while (true)
        {                        
            if (UnityEngine.Random.value < intelligenceChance)
            {                
                yield return StartCoroutine(FollowPlayer());                                   
            }
            else
            {
                int stepCount = UnityEngine.Random.Range(minStepCount, maxStepCount);
                yield return StartCoroutine(Move(RandomDirection(), stepCount));                                      
            }            
        }        
    }      
    
    private Vector2 RandomDirection()
    {              
        int direction = UnityEngine.Random.Range(0, 4);
        Vector2 res;
        if (direction == 0)
        {
            res = Vector2.right;
        }
        else if (direction == 1)
        {
            res = Vector2.up;
        }
        else if (direction == 2)
        {
            res = Vector2.left;
        }
        else 
        {
            res = Vector2.down;
        }
                
        return res;        
    }        
    
    // Lee algorithm for coroutine.    
    private IEnumerator FollowPlayer()
    {                      
        // Inizialization.        
        for (int i = 0; i < _rowSize; ++i)
        {
            for (int j = 0; j < _colSize; ++j)
            {
                if (GameManager.Location.Data[i, j] != 0)
                {
                    _map[i, j] = int.MaxValue;
                }
            }
        }

        int playerRow;
        int playerCol;
                
        if (!GameManager.Location.FromPositionToGrid(target.transform.position, out playerRow, out playerCol))
        {                     
            yield break;
        }
        
        // Select player position as start point
        // and enemy position as target point.
        int counter = 1;
        _map[playerRow, playerCol] = counter;        
        Vector2 enemyPos = new Vector2(row, col);
        Vector2 playerPos = new Vector2(playerRow, playerCol);
        List<Vector2> prevWave = new List<Vector2>();
        List<Vector2> wave = new List<Vector2>();        
        prevWave.Add(playerPos);        
        
        // Wave expansion.        
        while (prevWave.Count > 0)
        {
            counter++;
            wave.Clear();
            bool flag = true;
            foreach (Vector2 point in prevWave)
            {
                if (point == enemyPos)
                { 
                    flag = false;
                    wave.Clear();
                    break;
                }
                for (int i = 0, length = _directions.Length; i < length; ++i)
                {
                    Vector2 temp = point + _directions[i];
                    if (GameManager.Location.GetCell((int)temp.x, (int)temp.y) == Identifier.None && _map[(int)temp.x, (int)temp.y] == 0)
                    {
                        wave.Add(temp);
                        _map[(int)temp.x, (int)temp.y] = counter;
                        flag = false;
                    }                    
                }                
            }
            if (flag)
            {                         
                yield break;
            }
            prevWave = new List<Vector2>(wave);
        }          
       
        ToggleAngryMode();
        // Backtrace.
        while (playerPos != enemyPos)
        {
            for (int i = 0, length = _directions.Length; i < length; ++i)
            {
                Vector2 temp = enemyPos + _directions[i];
                if (GameManager.Location.GetCell((int)temp.x, (int)temp.y) == 0 && 
                    _map[(int)temp.x, (int)temp.y] == _map[(int)enemyPos.x, (int)enemyPos.y] - 1)
                {                                                              
                    enemyPos = temp;                                           
                    yield return StartCoroutine(Move(new Vector2(_directions[i].y, -_directions[i].x), 1));
                    break;
                }
            }                          
        }      
        ToggleAngryMode();                
    }
    
    private void ToggleAngryMode()
    {
        _isAngry = !_isAngry;
        animator.SetBool("IsAngry", _isAngry);
        _curSpeed = _isAngry ? angrySpeed : speed;
    }
    
    private IEnumerator UseInvulnerability()
    {        
        yield return new WaitForSeconds(invulnerabilityDuration);
        _isInvulnerable = false;
    }
}
