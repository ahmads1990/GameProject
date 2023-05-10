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
        int seconds = (int)KitchenGameManager.Instance.GetRemainingPlayingTime();
        int minutes = seconds / 60;
        seconds = seconds % 60;

        timerText.SetText(minutes.ToString() + ":" + seconds.ToString());
    }
}
