using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public GameObject player;
    public Vector3 targetPosition;
    public List<EnemyAI> enemies = new();
   
    public BattleState BattleState { get; private set; }
    public static event Action OnBattleStart;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

       
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyHealth.Death += UnregisterEnemies;
        if (player == null)
        {
            Debug.LogWarning("Player Missing");
            Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
            targetPosition = GridManager.Instance.gridTiles[1]; // Start at center tile
            player.transform.position = targetPosition;
        }
        else
        {
            targetPosition = GridManager.Instance.gridTiles[1]; // Start at center tile
            player.transform.position = targetPosition;
        }
        StartCoroutine((StartBattle()));
      
    }

    // Update is called once per frame
    void Update()
    {
       
      
        
    }

    public IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(0.1f); // Optional delay before starting the battle
        OnBattleStart?.Invoke();
    }

    public void CheckWin()
    {
       
        if (enemies.Count <= 0)
        {
            Debug.Log("All enemies defeated! Victory!");
            BattleState = BattleState.Victory;
            AudioManager.Instance.PlaySongSequence("VictorySongStart", "VictorySongLoop");
        }
        else
        {
            Debug.LogWarning("Enemies still present, cannot declare victory.");
            return;
        }
    }

    public void RegisterEnemies(EnemyAI enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemies(EnemyAI enemy)
    {

        enemies.Remove(enemy);
        CheckWin();
    }

    public void SetBattleState(BattleState state)
    {
                BattleState = state;

    }
}

public enum BattleState
{
    PlayerTurn,
    CardSelect,
    Victory,
    Defeat
}
