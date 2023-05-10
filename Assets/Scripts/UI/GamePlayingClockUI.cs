using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI timerText;
    private void Update()
    {
        timerImage.fillAmount = KitchenGameManager.Instance.GetPlayingTimerNorm();
        float timeRemaining = KitchenGameManager.Instance.GetRemainingPlayingTime();

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.SetText($"{minutes:0}:{seconds:00}");
    }
}
