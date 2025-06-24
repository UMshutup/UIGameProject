using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> playerPrefabs;
    public List<GameObject> enemyPrefabs;

    public List<Transform> playerPositions;
    public List<Transform> enemyPositions;

    public SelectableTeam playerTeam;
    public SelectableTeam enemyTeam;

    [HideInInspector] public List<GameObject> currentPlayers;
    [HideInInspector] public List<GameObject> currentEnemies;
    [HideInInspector] public List<Fighter> currentPlayerFighters;
    [HideInInspector] public List<Fighter> currentEnemyFighters;

    private List<Fighter> fighters;


    public BattleState state;

    // booleans

    public bool hasRandomized;

    public bool isPlayer1Choosing;

    public bool hasEveryoneChosenAMove;

    public bool hasEveryoneChosenATarget;

    private void Start()
    {
        currentPlayers = new List<GameObject>();
        currentPlayerFighters = new List<Fighter>();

        currentEnemies = new List<GameObject>();
        currentEnemyFighters = new List<Fighter>();

        
        state = BattleState.START;
        
        SetupBattle();
        
        state = BattleState.ACTIONTURN;

        fighters = new List<Fighter>(FindObjectsByType<Fighter>(FindObjectsSortMode.None));

    }

    private void SetupBattle()
    {

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
    }

    public void MoveSelectionPlayer1(int _moveNumber)
    {

        currentPlayerFighters[0].ChooseMove(_moveNumber);
        currentPlayerFighters[0].hasChosenMove = true;

        foreach (Fighter fighter in currentEnemyFighters)
        {
            fighter.ChooseMoveRandom();
            fighter.hasChosenMove = true;
        }

        isPlayer1Choosing = true;
    }

    public void MoveSelectionPlayer2(int _moveNumber)
    {

        currentPlayerFighters[1].ChooseMove(_moveNumber);

        currentPlayerFighters[1].hasChosenMove = true;

        isPlayer1Choosing = false;
    }

    private void Update()
    {
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
            TurnOrder();
        }
        else
        {
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
        playerTeam.SelectTeamOrTarget(currentPlayerFighters, isPlayer1Choosing);
        enemyTeam.SelectTeamOrTarget(currentPlayerFighters, isPlayer1Choosing);

        foreach (Fighter enemyFighter in currentEnemyFighters)
        {
            enemyFighter.ChooseTarget(new List<Fighter> { currentPlayerFighters[Random.Range(0, currentPlayerFighters.Count-1)] });
        }
    }

    private void TurnOrder()
    {
        if (!hasRandomized)
        {
            fighters = fighters.OrderByDescending((val) => val.speed).ToList();
            ShuffleSameSpeedFighters(fighters);
            hasRandomized = true;
        }
        

        for (int i = 0; i < fighters.Count; i++)
        {
            if (i != 0)
            {
                if (fighters[i - 1].hasFinishedAnimation && !fighters[i].hasAppendedAnimation)
                {
                    Debug.Log("Turn: " + i + "Attacker: " + fighters[i].fighterName);
                    fighters[i].UseMove();
                }
                
            }
            else
            {
                if (!fighters[i].hasAppendedAnimation)
                {
                    Debug.Log("Turn: " + i + "Attacker: " + fighters[i].fighterName);
                    fighters[i].UseMove();
                }
            }
        }

        if (fighters[fighters.Count-1].hasFinishedAnimation)
        {
            state = BattleState.ACTIONTURN;
            hasRandomized = false;

            foreach (Fighter currentFighter in fighters)
            {
                currentFighter.hasChosenMove = false;
            }

            foreach (Fighter currentFighter in fighters)
            {
                currentFighter.hasChosenTarget = false;
            }
        }
    }

    void ShuffleSameSpeedFighters(List<Fighter> fighterList)
    {
        Dictionary<int, List<int>> speedGroups = new Dictionary<int, List<int>>();

        for (int i = 0; i < fighterList.Count; i++)
        {
            int speed = (int)fighterList[i].speed;
            if (!speedGroups.ContainsKey(speed))
            {
                speedGroups[speed] = new List<int>();
            }
            speedGroups[speed].Add(i);
        }

        foreach (var kvp in speedGroups)
        {
            List<int> indices = kvp.Value;
            if (indices.Count <= 1)
                continue; 

            List<Fighter> fightersToShuffle = indices.Select(i => fighterList[i]).ToList();

            ShuffleList(fightersToShuffle);

            for (int i = 0; i < indices.Count; i++)
            {
                fighterList[indices[i]] = fightersToShuffle[i];
            }
        }
    }
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}
