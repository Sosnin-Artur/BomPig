using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LocationManager))]
[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(PowerUpManager))]
public class GameManager : MonoBehaviour
{
    public static LocationManager Location;
    public static AudioManager Audio;        
    public static PowerUpManager Powers;

    static public bool IsPaused = false;

    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject levelComplitedMenu;            

    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyBehaviour enemy;            

    [SerializeField] private AudioClip gameOverClip;         
    [SerializeField] private AudioClip winClip;             
    
    public void GameOver()
    {
        Audio.PlaySound(gameOverClip);
        Time.timeScale = 0;
        StopAllCoroutines();
        gameOverMenu.SetActive(true);        
    }

    public void CompliteLevel()
    {        
        Audio.PlaySound(winClip);
        Time.timeScale = 0;
        StopAllCoroutines();
        levelComplitedMenu.SetActive(true);        
    }

    public void StartGame()
    {        
        IsPaused = false;
        Time.timeScale = 1;                       
        player.Restart();
        enemy.Restart();        
    }

    public void Restart()
    {        
        gameOverMenu.SetActive(false);        
        SceneManager.LoadScene("Game");
    }    
    
    public void Pause()
    {                        
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        IsPaused = pauseMenu.activeSelf;
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }

    private void Awake()
    {        
        Audio = GetComponent<AudioManager>();
        Location = GetComponent<LocationManager>();
        Powers = GetComponent<PowerUpManager>();
    }    

    private void Start()
    {
        StartGame();
    }    
}
