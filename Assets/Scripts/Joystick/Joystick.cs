using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    [SerializeField] private GameObject handler;

    private Vector2 _direction;    
    private Vector2 _handlerStartPos;
        
    public float Horizontal {get; private set;}
    public float Vertical {get; private set;}

    private void Start()
    {
        _handlerStartPos = handler.transform.position;
    }
    
    
    private void FixedUpdate()
    {
        // Code in comments for testing on PC
        // if (Input.GetMouseButton(0))
        if (Input.touchCount > 0)
        {
            // Vector3 target = Input.mousePosition;
            Vector3 target = Input.GetTouch(0).position;
            _direction = target - transform.position;                                    
            
            if (_direction.magnitude < 300)            
            {
                _direction.Normalize();  
                if (Mathf.Abs(_direction.x) > Mathf.Abs(_direction.y))
                {
                    Horizontal = _direction.x;
                    Vertical = 0;
                }
                else
                {
                    Vertical = _direction.y;
                    Horizontal = 0;                    
                }                            
                handler.transform.localPosition = new Vector2(Horizontal, Vertical) * 100;
            }
        }
        else
        {
            _direction = new Vector2(0, 0);
            Horizontal = 0;      
            Vertical = 0;      
            handler.transform.position = _handlerStartPos;
        }
    }
}
