using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private PlayerBlaster playerBlaster;

    private int currentTileIndex = 1; // Start at center tile
    private Vector3 targetPosition;

    // Input System reference
    private InputAction moveAction;
    private InputAction gridAction;
    private InputAction blasterAction;

    void Awake()
    {
    
        moveAction = InputSystem.actions.FindAction("Player/Move");
        gridAction = InputSystem.actions.FindAction("Player/OpenCustomScreen");
    }

    void OnEnable()
    {
       moveAction.Enable();
        gridAction.Enable();

    }

    void OnDisable()
    {
      moveAction?.Disable();
        gridAction?.Disable();
       
    }

    void Start()
    {
        // Set initial position to Tile 1
        targetPosition = GridManager.Instance.gridTiles[currentTileIndex];
        transform.position = targetPosition;
    }

    void Update()
    {
        if(Time.timeScale == 1f)
        {
            if (gridAction.WasPressedThisFrame())
            {
                OpenCustomScreen();
            }

            if (moveAction.WasPressedThisFrame())
            {
                HandleMovement(moveAction.ReadValue<Vector2>());
            }

        }
       
    }

    // Handle left/right input
    private void HandleMovement(Vector2 moveInput)
    {

        //If move is left move to the tile left of the current tile, if move is right move to the tile right of the current tile
        if (moveInput.x < 0)
        {
            MoveToTile(currentTileIndex - 1);
        }
        else if (moveInput.x > 0)
        {
            MoveToTile(currentTileIndex + 1);
        }
      
    }

    private void OpenCustomScreen()
    {
        Debug.Log("Grid action triggered. Current battle state: " + BattleManager.Instance.BattleState);
        ManagerContainer.Instance.customScreenManager.buildCardGrid();
    }

    private void MoveToTile(int newIndex)
    {
        newIndex = Mathf.Clamp(newIndex, 0, 2); // Clamp to front row (0-2)
        if (newIndex != currentTileIndex)
        {
            currentTileIndex = newIndex;
            targetPosition = GridManager.Instance.gridTiles[currentTileIndex];
            transform.position = targetPosition; // Snap immediately to the new tile
        }
      
    }

   
}