using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{       
    [SerializeField] private float posX = 10.0f;
    [SerializeField] private float posY = 10.0f;
    [SerializeField] private float buffer = 100.0f;
    [SerializeField] private float width = 90.0f;
    [SerializeField] private float height = 90.0f;
    
    private List<PowerUpType>  _powerUps;       
    
    public void AddToList(PowerUpType power)
    {
        if (!_powerUps.Contains(power))
        {
            _powerUps.Add(power);
        }
    }

    public void RemoveFromList(PowerUpType power)
    {
        if (_powerUps.Contains(power))
        {
            _powerUps.Remove(power);
        }
    }

    private void Awake()
    {
        _powerUps = new List<PowerUpType>();
    }

    private void OnGUI() 
    {
        float curX = posX;
        
        foreach(PowerUpType power in _powerUps)
        {
            Texture2D image = Resources.Load<Texture2D>("Icons/" + power) as Texture2D;
            GUI.Label(new Rect(curX, posY, width, height), image);
            curX += buffer;
        }           
    }    
}