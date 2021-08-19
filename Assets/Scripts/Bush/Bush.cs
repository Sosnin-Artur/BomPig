using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{        
    private int _row;
    private int _col;
    
    private GameManager _gameManager;    
    
    public void SetCoord(Vector2 coords)
    {
        _row = (int)coords.x;
        _col = (int)coords.y;
    }

    private void Start()
    {
        _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();        
    }
        
    private void OnDestroy()
    {
        _gameManager.ClearCell(_row, _col);
        Destroy(gameObject);
    }
}
