using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{    
   // Map size.
    [SerializeField] private int rows = 9;  
    [SerializeField] private int cols = 17;   

    [SerializeField] private float bushMakingChance = 0.1f; // Coefficient for arranging bushes.
        
    [SerializeField] private GameObject startStone; // Stone for aarnging objects on the grid.
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private GameObject bushPrefab;
        
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private float powerupCreationDuration = 5.0f;
    
    private Vector2 _dimensions; // Cell size according to startStone

    public Identifier[,] Data {get; private set;} // Level map

    // Convert from position on grid to word position of centre of cell
    // return true if position on grid.
    public bool FromGridToPosition(int row, int col, ref Vector2 pos)
    {                
        if (row >= rows || row < 0 || col >= cols || col < 0)
        {            
            return false;
        }

        if (Data[row, col] == Identifier.None)
        {
            pos = (Vector2)startStone.transform.position +                     
                    new Vector2(_dimensions.x * (col) / 1.05f - 0.1f * (row), -_dimensions.y * (row) / 1.35f);            
            return true;
        }        
                        
        return false;
    }

    // Convert from word position to position on grid.
    // return true if position on grid.
    public bool FromPositionToGrid(Vector2 pos, out int row, out int col)
    {   
        Vector2 temp = new Vector2();
        FromGridToPosition(0, 0, ref temp);
        pos -= temp;
        
        row = (int)Mathf.Round((Mathf.Abs(pos.y)) / _dimensions.y);
        col = (int)Mathf.Round((Mathf.Abs(pos.x)) / _dimensions.x);
        
        row = (int)(row + _dimensions.y * (row) / 2.70f);        

        if (row >= rows || row < 0 || col >= cols || col < 0 || Data[row, col] != Identifier.None)
        {
            row = -1;
            col = -1;
            return false;
        }        
        return true; 
    }    

    public Identifier GetCell(int row, int col)
    {
        if (row >= rows || row < 0 || col >=cols || col < 0)
        {
            return Identifier.StoneOrWal;
        }

        return Data[row, col];
    }
    
    public void ClearCell(int row, int col)
    {                
        Data[row, col] = Identifier.None;
    }

    private void Awake()
    {
        _dimensions = startStone.GetComponent<SpriteRenderer>().bounds.size;         
        GenerateStones();              
    }    
     
    private void Start()
    {        
        GenerateLevel();              
    }    

    private void GenerateStones()
    {
        Data = new Identifier[rows, cols];
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                if (i % 2 == 1 && j % 2 == 1)
                {                    
                    Vector2 pos = new Vector2();
                    FromGridToPosition(i, j, ref pos);
                    Instantiate(stonePrefab, pos, Quaternion.identity);
                    Data[i, j] = Identifier.StoneOrWal;
                }
            }
        }        
    }

    private void GenerateLevel()
    {                        
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {                                 
                if ((i % 2 == 0 || j % 2 == 0) && Random.value < bushMakingChance && Data[i, j] != Identifier.Bush)
                {
                    Vector2 pos = new Vector2();
                    FromGridToPosition(i, j, ref pos);
                    Instantiate(bushPrefab, pos, Quaternion.identity).SendMessage("SetCoord", new Vector2(i, j));                        
                    Data[i, j] = Identifier.Bush;
                }                                    
            }
        }
            
        StartCoroutine(CreateBonus());
    }    

    private IEnumerator CreateBonus()
    {
        while (true)
        {
            yield return new WaitForSeconds(powerupCreationDuration);
            int index = Random.Range(0, powerUps.Length);            
            int i, j;

            do
            {
                i = Random.Range(0, rows);
                j = Random.Range(0, cols);
            }
            while (Data[i, j] != Identifier.None);

            Vector2 pos = new Vector2();
            FromGridToPosition(i, j, ref pos);
            Instantiate(powerUps[index], pos, Quaternion.identity);
        }        
    }    
}