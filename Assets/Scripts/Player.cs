using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    // Singleton
    public static Player Instance { get; private set; }

    public event EventHandler OnPickSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
        }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        // if game didn't start or already finished don't interact
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        // Else Interact with the counter
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        // if game didn't start or already finished don't interact
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        // Else Interact with the counter
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() { return isWalking; }

    private void HandleInteractions()
    {
        // get input from input manager
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // if there is input by user change lastInteractDir
        if (moveDir != Vector3.zero)
            lastInteractDir = moveDir;

        float interactDistance = 2f;
        // cast a ray from player position with interact direction, with max distance and layer mask = counter then store output
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            // if ray cast hit any counter
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has Counter if its different than the currently selected counter change it
                if (baseCounter != selectedCounter)
                {
                    // set current counter and invoke selected counter changed
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                // didn't hit a counter just set currently selected counter to null
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }


    }
    private void HandleMovement()
    {
        // get input from input manager
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        /* 
            Cast a capsule around the player capsule dimen 
            height from transform position (0,0,0) to head (up * height)
            with capsule radius = player radius
            with input direction and the distance that will be moved
        */
        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDir,
            moveDistance
            );

        if (!canMove)
        {
            // Cannot move toward moveDir
            // test if (hug the wall)

            // attempt move on x
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;

            // do capsule move again with updated direction and condition for controller playable
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) &&
                !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            
            if (canMove)
                // can move only on the x
                moveDir = moveDirX;
            else
            {
                // cannot move only on the x
                // attempt move on z
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;

                // do capsule move again with updated direction and condition for controller playable
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) &&
                    !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                    // can move only on the x
                    moveDir = moveDirZ;
                else
                {
                    // just cannot move in any dir
                }
            }
        }

        if (canMove)
            transform.position += moveDir * moveDistance;

        isWalking = moveDir != Vector3.zero;

        // for rotation 
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter,
        });
    }

    public Transform GetKitchenObjectFollowTransform() { return kitchenObjectHoldPoint; }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public void ClearKitchenObject() { kitchenObject = null; }
    public bool HasKitchenObject() { return kitchenObject != null; }
}
