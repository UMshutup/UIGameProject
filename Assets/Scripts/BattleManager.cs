using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;

    [HideInInspector] public GameObject currentPlayer;
    [HideInInspector] public GameObject currentEnemy;
    [HideInInspector] public Fighter currentPlayerFighter;
    [HideInInspector] public Fighter currentEnemyFighter;

    public BattleState state;

    private void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    private void SetupBattle()
    {
        currentPlayer = Instantiate(playerPrefab, playerPosition);
        currentPlayerFighter = currentPlayerFighter.GetComponent<Fighter>();
        currentEnemy = Instantiate(enemyPrefab, enemyPosition);
        currentEnemyFighter = currentEnemyFighter.GetComponent<Fighter>();
    }
}
