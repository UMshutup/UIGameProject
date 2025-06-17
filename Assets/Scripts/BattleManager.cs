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

    private bool playerGoesFirst;

    private void Start()
    {
        state = BattleState.START;
        SetupBattle();
        state = BattleState.ACTIONTURN;
    }

    private void SetupBattle()
    {
        currentPlayer = Instantiate(playerPrefab, playerPosition);
        currentPlayerFighter = currentPlayer.GetComponent<Fighter>();
        currentEnemy = Instantiate(enemyPrefab, enemyPosition);
        currentEnemyFighter = currentEnemy.GetComponent<Fighter>();
    }

    public void MoveSelected(int _moveNumber)
    {
        if (state != BattleState.ACTIONTURN)
        {
            return;
        }
        currentEnemyFighter.ChooseMoveRandom();
        currentPlayerFighter.ChooseMove(_moveNumber);
        DecideWhosFirst();
        TurnOrder();
    }

    private void TurnOrder()
    {
        if (playerGoesFirst)
        {
            PlayerTurn();
            if (currentPlayerFighter.hasFinishedAnimation)
            {
                EnemyTurn();
            }
        }
        else
        {
            EnemyTurn();
            if (currentEnemyFighter.hasFinishedAnimation)
            {
                PlayerTurn();
            }
        }
        if (currentPlayerFighter.hasFinishedAnimation && currentEnemyFighter.hasFinishedAnimation)
        {
            state = BattleState.ACTIONTURN;
        }
    }

    private void PlayerTurn()
    {
        if (currentPlayerFighter.dealDamage)
        {
            currentEnemyFighter.currentHP -= currentPlayerFighter.chosenMove.CalculateDamage(currentPlayerFighter, currentEnemyFighter);
        }
        currentPlayerFighter.target = currentEnemyFighter;
    }

    private void EnemyTurn()
    {
        if (currentEnemyFighter.dealDamage)
        {
            currentPlayerFighter.currentHP -= currentEnemyFighter.chosenMove.CalculateDamage(currentEnemyFighter, currentPlayerFighter);
        }
        currentEnemyFighter.target = currentPlayerFighter;
    }

    private void DecideWhosFirst()
    {
        if (currentPlayerFighter.speed > currentEnemyFighter.speed)
        {
            playerGoesFirst = true;
        }
        else if (currentPlayerFighter.speed < currentEnemyFighter.speed)
        {
            playerGoesFirst = false;
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                playerGoesFirst = true;
            }
            else
            {
                playerGoesFirst = false;
            }
        }


    }
}
