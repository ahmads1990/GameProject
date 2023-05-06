using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;

    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    [SerializeField] private float spawnTimerMax = 4f;

    private int platesSpawnAmount;
    private int platesSpawnAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnTimerMax)
        {
            spawnPlateTimer = 0f;
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnAmount < platesSpawnAmountMax)
            {
                platesSpawnAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // player is empty handed
            if(platesSpawnAmount > 0)
            {
                // there is at least one plate spawned
                platesSpawnAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
