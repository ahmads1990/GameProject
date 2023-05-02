using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countDownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 10f;
    private void Awake()
    {
        state = State.WaitingToStart;
        Instance = this;
    }
    private void Update()
    {
        switch (state)
        {

            case State.WaitingToStart:
            waitingToStartTimer -= Time.deltaTime;
            if (waitingToStartTimer < 0f)
            {
                state = State.CountDownToStart;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
            }
            break;

            case State.CountDownToStart:
            countDownToStartTimer -= Time.deltaTime;
            if (countDownToStartTimer < 0f)
            {
                state = State.GamePlaying;
                gamePlayingTimer = gamePlayingTimerMax;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
            }
            break;

            case State.GamePlaying:
            gamePlayingTimer -= Time.deltaTime;
            if (gamePlayingTimer < 0f)
            {
                state = State.GameOver;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
            }
            break;

            case State.GameOver:
            break;

            default:
            break;
        }
        Debug.Log(state);
    }
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
    public bool IsCountDownToStartActive()
    {
        return state == State.CountDownToStart;
    }

    public float GetCountDownToStartTimer()
    {
        return countDownToStartTimer;
    }
    public float GetPlayingTimer()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
}