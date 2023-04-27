using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying KitchenObject
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            // There is KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
            }
            else
            {
                // Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
