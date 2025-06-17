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

    public bool playerGoesFirst;

    public bool hasMoveBeenSelected;

    private void Start()
    {
        state = BattleState.START;
        SetupBattle();
        state = BattleState.ACTIONTURN;
        currentPlayerFighter.target = currentEnemyFighter;
        currentEnemyFighter.target = currentPlayerFighter;
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
        currentPlayerFighter.ChooseMove(_moveNumber);
        currentEnemyFighter.ChooseMoveRandom();

        hasMoveBeenSelected = true;
        DecideWhosFirst();
    }

    private void Update()
    {
        if (hasMoveBeenSelected) {
            TurnOrder();
        }
        else
        {
            currentPlayerFighter.hasFinishedAnimation =false;
            currentPlayerFighter.hasAppendedAnimation = false;
            currentEnemyFighter.hasFinishedAnimation = false;
            currentEnemyFighter.hasAppendedAnimation = false;
        }
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
            hasMoveBeenSelected = false;
        }
    }

    private void PlayerTurn()
    {
        currentPlayerFighter.UseMove();
        state = BattleState.PLAYERTURN;
    }

    private void EnemyTurn()
    {
        currentEnemyFighter.UseMove();
        state = BattleState.ENEMYTURN;
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
