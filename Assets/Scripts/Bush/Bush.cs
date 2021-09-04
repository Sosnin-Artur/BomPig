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
        
    private void OnDestroy()
    {
        GameManager.Location.ClearCell(_row, _col);
        Destroy(gameObject);
    }
}
