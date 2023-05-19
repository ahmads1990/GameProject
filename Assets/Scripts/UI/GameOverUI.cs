using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private TextMeshProUGUI LevelScoreText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failColor;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();

            bool levelSuccess = DeliveryManager.Instance.PassedLevel();
            if (levelSuccess)
            {
                LevelScoreText.text = "Congratulations!!";
                LevelScoreText.color = successColor;
            }
            else
            {
                LevelScoreText.text = "Better Luck next time";
                LevelScoreText.color = failColor;
            }
        }
        else
            Hide();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
