using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;


public class CannonInstance : BattleCardInstance
{
   protected CannonCardData CardData;

    public CannonInstance(CannonCardData card) : base(card)
    {
        cardData = card;
    }

    public override void OnUse()
    {
       int damage = cardData.damage;

        int playerColumn = GridManager.Instance.GetPlayerColumn(BattleManager.Instance.player.transform.position);

        List <EnemyAI> enemies = new List<EnemyAI>(BattleManager.Instance.enemies);
        foreach (EnemyAI enemy in enemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            int enemyColumn;

            if (enemy != null)
            {
                Vector2Int enemyGridPos = enemy.GetGridPosition();
                enemyColumn = enemyGridPos.x;
            }
            else
            {
                enemyColumn = GridManager.Instance.GetColumnIndex(enemy.transform.position);
            }

            if (enemyColumn == playerColumn)
            {
                Debug.Log("Cannon card hits enemy in column " + enemyColumn + " for " + damage + " damage.");
                Debug.Log(playerColumn + " vs " + enemyColumn);
                enemyHealth.TakeDamage((int)damage);
                //noiseManager.NoiseGain(damage);
            }
        }

        // Implement the specific behavior for the Cannon card when used.
        // For example, you might want to apply damage to a target or trigger an effect.
        Debug.Log("Using Cannon card: " + cardData.cardName + " for " + cardData.damage + " damage.");
        // Here you would add the logic to determine targets and apply damage/effects based on cardData.
    }

}
