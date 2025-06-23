using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;

<<<<<<< HEAD
    [HideInInspector] public List<GameObject> currentPlayers;
    [HideInInspector] public List<GameObject> currentEnemies;
    [HideInInspector] public List<Fighter> currentPlayerFighters;
    [HideInInspector] public List<Fighter> currentEnemyFighters;

    private List<Fighter> fighters;

    public GameObject playerTeam;
    public GameObject enemyTeam;


    private List<SelectableFighter> selectablePlayerFighters;
    private List<SelectableFighter> selectableEnemyFighters;
    private List<SelectableFighter> selectableFighters;
=======
    [HideInInspector] public GameObject currentPlayer;
    [HideInInspector] public GameObject currentEnemy;
    [HideInInspector] public Fighter currentPlayerFighter;
    [HideInInspector] public Fighter currentEnemyFighter;
>>>>>>> parent of 8a1b8ed (Added target selection and another player character)

    public BattleState state;

    public bool playerGoesFirst;

<<<<<<< HEAD
    public bool hasRandomized;

    public bool isPlayer1Choosing;

    public bool isSelectableFighterAEnemy;

    public bool hasEveryoneChosenAMove;

    public bool hasEveryoneChosenATarget;
=======
    public bool hasMoveBeenSelected;
>>>>>>> parent of 8a1b8ed (Added target selection and another player character)

    private void Start()
    {
        state = BattleState.START;
        SetupBattle();
        state = BattleState.ACTIONTURN;
<<<<<<< HEAD

        fighters = new List<Fighter>(FindObjectsByType<Fighter>(FindObjectsSortMode.None));

        selectablePlayerFighters = new List<SelectableFighter>(playerTeam.GetComponentsInChildren<SelectableFighter>());
        selectableEnemyFighters = new List<SelectableFighter>(enemyTeam.GetComponentsInChildren<SelectableFighter>());

        foreach (SelectableFighter selectableEnemy in selectableEnemyFighters)
        {
            selectableEnemy.isEnemy = true;
        }

        selectableFighters = new List<SelectableFighter>(FindObjectsByType<SelectableFighter>(FindObjectsSortMode.None).ToList());

=======
        currentPlayerFighter.target = currentEnemyFighter;
        currentEnemyFighter.target = currentPlayerFighter;
>>>>>>> parent of 8a1b8ed (Added target selection and another player character)
    }

    private void SetupBattle()
    {
<<<<<<< HEAD

        for (int i = 0; i < playerPrefabs.Count; i++)
        {
            currentPlayers.Add(Instantiate(playerPrefabs[i], playerPositions[i]));
            currentPlayerFighters.Add(currentPlayers[i].GetComponent<Fighter>());
        }

        for (int i = 0; i< enemyPrefabs.Count; i++)
        {
            currentEnemies.Add(Instantiate(enemyPrefabs[i], enemyPositions[i]));
            currentEnemyFighters.Add(currentEnemies[i].GetComponent<Fighter>());
        }

        foreach (Fighter fighter in currentPlayerFighters)
        {
            fighter.targets = new List<Fighter> { currentEnemyFighters[0] };
        }

        foreach (Fighter fighter in currentEnemyFighters)
        {
            fighter.targets = new List<Fighter> { currentPlayerFighters[0] };
        }
=======
        currentPlayer = Instantiate(playerPrefab, playerPosition);
        currentPlayerFighter = currentPlayer.GetComponent<Fighter>();
        currentEnemy = Instantiate(enemyPrefab, enemyPosition);
        currentEnemyFighter = currentEnemy.GetComponent<Fighter>();
>>>>>>> parent of 8a1b8ed (Added target selection and another player character)
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
<<<<<<< HEAD
        hasEveryoneChosenAMove = true;
        foreach (Fighter currentFighter in fighters)
        {
            if (!currentFighter.hasChosenMove)
            {
                hasEveryoneChosenAMove = false;
            }
        }

        hasEveryoneChosenATarget = true;
        foreach (Fighter currentFighter in fighters)
        {
            if (!currentFighter.hasChosenTarget)
            {
                hasEveryoneChosenATarget = false;
            }
        }


        TargetSelection();

        if (hasEveryoneChosenAMove && hasEveryoneChosenATarget)
        {
=======
        if (hasMoveBeenSelected) {
>>>>>>> parent of 8a1b8ed (Added target selection and another player character)
            TurnOrder();
        }
        else
        {
<<<<<<< HEAD
            foreach (Fighter fighter in currentPlayerFighters)
            {
                fighter.ResetValues();
            }

            foreach (Fighter fighter in currentEnemyFighters)
            {
                fighter.ResetValues();
            }

        }
    }

    private void TargetSelection()
    {
        foreach (SelectableFighter selectable in selectableFighters)
        {
            if ((currentPlayerFighters[0].hasChosenMove && isPlayer1Choosing && !currentPlayerFighters[0].hasChosenTarget) || 
                (currentPlayerFighters[1].hasChosenMove && !isPlayer1Choosing && !currentPlayerFighters[1].hasChosenTarget))
            {
                selectable.canSelect = true;
                if (currentPlayerFighters[0].chosenMove.GetHitsAWholeSquad() || currentPlayerFighters[1].chosenMove.GetHitsAWholeSquad())
                {
                    if (selectable.isEnemy)
                    {
                        isSelectableFighterAEnemy = true;
                    }
                    else
                    {
                        isSelectableFighterAEnemy = false;
                    }

                    if (isSelectableFighterAEnemy)
                    {
                        if (selectable.isEnemy)
                        {
                            selectable.isOnSprite = true;
                        }
                        else
                        {
                            selectable.isOnSprite = false;
                        }
                    }
                    else
                    {
                        if (!selectable.isEnemy)
                        {
                            selectable.isOnSprite = true;
                        }
                        else
                        {
                            selectable.isOnSprite = false;
                        }
                    }
                }
            }
            else 
            {
                selectable.canSelect = false;
                selectable.hasSelectedTarget = false;
            }

            

            if (selectable.hasSelectedTarget && isPlayer1Choosing)
            {
                currentPlayerFighters[0].ChooseTarget(new List<Fighter> { selectable.gameObject.GetComponent<Fighter>() });

                foreach (Fighter fighter in currentEnemyFighters)
                {
                    fighter.ChooseTarget(new List<Fighter> { currentPlayerFighters[Random.Range(0, currentPlayerFighters.Count - 1)] });
                    fighter.hasChosenTarget = true;
                }

                isPlayer1Choosing = false;

                selectable.canSelect = false;
            }

            if(selectable.hasSelectedTarget && !isPlayer1Choosing)
            {
                currentPlayerFighters[1].ChooseTarget(new List<Fighter> { selectable.gameObject.GetComponent<Fighter>() });

                foreach (Fighter fighter in currentEnemyFighters)
                {
                    fighter.ChooseTarget(new List<Fighter> { currentPlayerFighters[Random.Range(0, currentPlayerFighters.Count - 1)] });
                    fighter.hasChosenTarget = true;
                }

                selectable.canSelect = false;
            }
=======
            currentPlayerFighter.ResetValues();
            currentEnemyFighter.ResetValues();
>>>>>>> parent of 8a1b8ed (Added target selection and another player character)
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
