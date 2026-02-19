using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    private int currentTileIndex = 1; // Start at center tile
    private Vector3 targetPosition;

    // Input System reference
    private PlayerControls playerControls;
    private InputAction moveAction;

    void Awake()
    {
        // Initialize Input System
        playerControls = new PlayerControls();
        moveAction = InputSystem.actions.FindAction("Player/Move");
    }

    void OnEnable()
    {
       
        moveAction.performed += HandleMovement;
    }

    void OnDisable()
    {
        playerControls.Gameplay.Disable();
        moveAction.performed -= HandleMovement;
    }

    void Start()
    {
        // Set initial position to Tile 1
        targetPosition = GridManager.Instance.gridTiles[currentTileIndex];
        transform.position = targetPosition;
    }

    void Update()
    {
     
    }

    // Handle left/right input
    private void HandleMovement(InputAction.CallbackContext context)
    {
        Debug.Log("Move input received: " + context.ReadValue<Vector2>());
        Vector2 moveInput = context.ReadValue<Vector2>();

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