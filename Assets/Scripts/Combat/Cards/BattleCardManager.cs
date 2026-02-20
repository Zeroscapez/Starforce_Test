using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class BattleCardManager : MonoBehaviour
{
    [Header("UI Setup")]
    public RectTransform gridParent;    // The parent panel (e.g., your CardGrid) on the Custom Screen.
    public GameObject cardPrefab;       // Your CardUI prefab.
    public GameObject customScreen;

    [Header("Card Data")]
    // Lists to keep track of cards currently equipped
    [SerializeField] public List<CardUI> equippedCards = new List<CardUI>();

    private bool selectionFinalized = false;
    private bool combatPaused = false;
    private DeckViewControl deckViewControl;
    InputAction useCardAction;



    public void Awake()
    {
        useCardAction = InputSystem.actions.FindAction("Player/UseCard");
    }
    private void Start()
    {
     
        deckViewControl = FindObjectOfType<DeckViewControl>();

      


    }

    void Update()
    {
        equippedCards = ManagerContainer.Instance.customScreenManager.GetChosenCards();

        if(useCardAction.WasPressedThisFrame())
        {
            Debug.Log("Use Card action triggered. Equipped cards count: " + equippedCards.Count);
            UseNextCard();
        }
    }

    



    void UseNextCard()
    {
        if (equippedCards.Count > 0)
        {
            BattleCard card = equippedCards[0].battleCard;
            equippedCards.RemoveAt(0);
            deckViewControl.UpdateDeckView();
            int damage = card.damage;
            card.CreateInstance().OnUse();


        }
        else
        {
            Debug.Log("No more cards to use!");
        }
    }

    //List<EnemyHealth> GetTargetsForCard(BattleCard card)
    //{
    //    List<EnemyHealth> targets = new List<EnemyHealth>();

    //    switch (card.targetPattern)
    //    {
    //        case BattleCard.TargetPattern.Single:
    //            {
    //                 //Get the player's current column
    //                int playerColumn = GridManager.Instance.GetColumnIndex(playerTransform.position);
    //                Debug.Log(playerColumn);

    //                 //Loop through registered enemies (using a copy in case the list is modified)
    //                List<EnemyHealth> enemies = new List<EnemyHealth>(GridManager.Instance.registeredEnemies);
    //                EnemyHealth closestEnemy = null;
    //                float closestDistance = float.MaxValue;

    //                foreach (EnemyHealth enemy in enemies)
    //                {
    //                    EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
    //                    int enemyColumn = 0;

    //                    if (enemyAI != null)
    //                    {
    //                        Vector2Int enemyGridPos = enemyAI.GetGridPosition();
    //                        enemyColumn = enemyGridPos.x;
    //                    }
    //                    else
    //                    {
    //                        enemyColumn = GridManager.Instance.GetColumnIndex(enemy.transform.position);
    //                    }

    //                    Debug.Log(enemyColumn);
    //                    // Check if the enemy is in the same column
    //                    if (enemyColumn == playerColumn)
    //                    {
    //                         //Find the closest enemy in the column
    //                        float distance = Vector3.Distance(transform.position, enemy.transform.position);
    //                        if (distance < closestDistance)
    //                        {
    //                            closestDistance = distance;
    //                            closestEnemy = enemy;
    //                        }
    //                    }
    //                }

    //                 //Apply damage to the closest enemy in the column
    //                if (closestEnemy != null)
    //                {
    //                    targets.Add(closestEnemy);
    //                }
    //            }
    //            break;

    //        case BattleCard.TargetPattern.Column:
    //            {
    //                // For a column-target card, hit all enemies in a specific column (here, column 1 as an example).
    //                int targetColumn = 1;
    //                foreach (EnemyHealth enemy in GridManager.Instance.registeredEnemies)
    //                {
    //                    EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
    //                    if (enemyAI != null)
    //                    {
    //                        Vector2Int enemyPos = enemyAI.GetGridPosition();
    //                        if (enemyPos.x == targetColumn)
    //                        {
    //                            targets.Add(enemy);
    //                        }
    //                    }
    //                }
    //            }
    //            break;

    //        case BattleCard.TargetPattern.Row:
    //            {
    //                // For a row-target card, hit all enemies on a specific row (here, row 1 as an example).
    //                int targetRow = 1;
    //                foreach (EnemyHealth enemy in GridManager.Instance.registeredEnemies)
    //                {
    //                    EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
    //                    if (enemyAI != null)
    //                    {
    //                        Vector2Int enemyPos = enemyAI.GetGridPosition();
    //                        if (enemyPos.y == targetRow)
    //                        {
    //                            targets.Add(enemy);
    //                        }
    //                    }
    //                }
    //            }
    //            break;

    //        case BattleCard.TargetPattern.All:
    //            {
    //                // Hit every enemy.
    //                targets.AddRange(GridManager.Instance.registeredEnemies);
    //            }
    //            break;

    //        case BattleCard.TargetPattern.Custom:
    //            {
    //                // Implement any custom targeting logic here.
    //            }
    //            break;
    //    }

    //    return targets;
    //}

}
