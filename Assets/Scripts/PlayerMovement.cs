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
        moveAction = playerControls.Gameplay.MoveLeftRight;
    }

    void OnEnable()
    {
        playerControls.Gameplay.Enable();
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
        SmoothMove();
    }

    // Handle left/right input
    private void HandleMovement(InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();
        if (moveInput < 0) MoveToTile(currentTileIndex - 1); // Left
        else if (moveInput > 0) MoveToTile(currentTileIndex + 1); // Right
    }

    private void MoveToTile(int newIndex)
    {
        newIndex = Mathf.Clamp(newIndex, 0, 2); // Clamp to front row (0-2)
        if (newIndex != currentTileIndex)
        {
            currentTileIndex = newIndex;
            targetPosition = GridManager.Instance.gridTiles[currentTileIndex];
        }
    }

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}