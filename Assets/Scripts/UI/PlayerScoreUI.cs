using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerScoreText;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateScore();
    }
    public void UpdateScore()
    {
        int playerScore = DeliveryManager.Instance.GetPlayerCurrentScore();
        int levelTarget = DeliveryManager.Instance.GetLevelTargetScore();
        playerScoreText.text = "Score " + playerScore + "/" + levelTarget;
    }
}
