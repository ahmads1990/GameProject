using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        // Reset time for the game time to work after press main menu button
        Time.timeScale = 1f; 
    }

    
}
