using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopUp";
    [SerializeField] private TextMeshProUGUI countDownText;

    private Animator animator;
    private int previousCountDownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start() {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (KitchenGameManager.Instance.IsCountDownToStartActive())
            Show();
        else
            Hide();
    }
    private void Update()
    {
        int countDownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountDownToStartTimer());
        countDownText.text = countDownNumber.ToString();

        if(previousCountDownNumber != countDownNumber)
        {
            previousCountDownNumber = countDownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountDownSound(); 
        }
    }
    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }

}
