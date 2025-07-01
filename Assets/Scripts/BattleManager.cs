using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Range(1, 2)] public int limitOfActivePlayers;
    [Range(1,3)] public int limitOfActiveEnemies;
    
    [Space]
    public List<GameObject> playerPrefabs;
    public List<GameObject> enemyPrefabs;

    [Space]
    public List<Transform> playerPositions;
    public List<Transform> enemyPositions;

    [Space]
    public Transform backupPlayerPosition;

    [Space]
    public SelectableTeam playerTeam;
    public SelectableTeam enemyTeam;

    [HideInInspector] public List<GameObject> currentPlayers;
    [HideInInspector] public List<GameObject> currentEnemies;

    [HideInInspector] public List<GameObject> currentPlayerBackups;
    [HideInInspector] public List<GameObject> currentEnemyBackups;

    [HideInInspector] public List<Fighter> currentPlayerFighters;
    [HideInInspector] public List<Fighter> currentEnemyFighters;

    private List<Fighter> fighters;


    public BattleState state;

    // booleans

    public bool hasRandomized;

    public bool isPlayer1Choosing;

    public bool hasEveryoneChosenAMove;

    public bool hasEveryoneChosenATarget;

    private bool hasAddedAP = false;

    private bool hasAppendedAnimation;

    private void Start()
    {
        currentPlayers = new List<GameObject>();
        currentPlayerFighters = new List<Fighter>();

        currentEnemies = new List<GameObject>();
        currentEnemyFighters = new List<Fighter>();

        currentPlayerBackups = new List<GameObject>();
        currentEnemyBackups = new List<GameObject>();

        state = BattleState.START;
        
        SetupBattle();
        
        state = BattleState.DECISIONTURNPLAYER1;

        playerTeam.RebuildTeam();
        enemyTeam.RebuildTeam();

    }

    private void SetupBattle()
    {

        for (int i = 0; i < limitOfActivePlayers; i++)
        {
            currentPlayers.Add(Instantiate(playerPrefabs[i], playerPositions[i]));
            currentPlayers[i].GetComponent<SelectableFighter>().isBackup = false;
        }

        for (int i = limitOfActivePlayers; i < playerPrefabs.Count; i++)
        {
            currentPlayerBackups.Add(Instantiate(playerPrefabs[i], backupPlayerPosition));
        }

        for (int i = 0; i < currentPlayers.Count; i++)
        {
            currentPlayerFighters.Add(currentPlayers[i].GetComponent<Fighter>());
        }


        //enemies
        for (int i = 0; i< limitOfActiveEnemies; i++)
        {
            currentEnemies.Add(Instantiate(enemyPrefabs[i], enemyPositions[i]));
            currentEnemies[i].GetComponent<SelectableFighter>().isBackup = false;
        }

        for (int i = limitOfActiveEnemies; i< enemyPrefabs.Count; i++)
        {
            currentEnemyBackups.Add(enemyPrefabs[i]);
        }

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            currentEnemyFighters.Add(currentEnemies[i].GetComponent<Fighter>());
        }

        foreach (Fighter fighter in currentPlayerFighters)
        {
            fighter.targets = new List<Fighter> { currentEnemyFighters[0] };
            fighter.ResetAP();
        }

        foreach (Fighter fighter in currentEnemyFighters)
        {
            fighter.targets = new List<Fighter> { currentPlayerFighters[0] };
            fighter.isEnemy = true;
            fighter.ResetAP();
        }

        fighters = new List<Fighter>();

        foreach (Fighter fighter in currentPlayerFighters)
        {
            fighters.Add(fighter);
        }
        foreach (Fighter fighter in currentEnemyFighters)
        {
            fighters.Add(fighter);
        }
    }

    public void MoveSelectionPlayer1(int _moveNumber)
    {

        currentPlayerFighters[0].ChooseMove(_moveNumber);

        MoveSelectionEnemies();
    }

    public void MoveSelectionPlayer2(int _moveNumber)
    {

        currentPlayerFighters[1].ChooseMove(_moveNumber);
    }

    public void MoveSelectionEnemies()
    {
        foreach (Fighter fighter in currentEnemyFighters)
        {
            fighter.ChooseMoveRandom();
        }
    }

    private void Update()
    {
        

        if (state == BattleState.DECISIONTURNPLAYER1)
        {
            ReplaceEnemy();
        }

        if (state == BattleState.DECISIONTURNPLAYER1)
        {
            if (!hasAddedAP)
            {
                foreach (Fighter fighter in currentPlayerFighters)
                {
                    fighter.RegenerateAP();
                    hasAddedAP = true;
                }

                foreach (Fighter fighter in currentEnemyFighters)
                {
                    fighter.RegenerateAP();
                    hasAddedAP = true;
                }

                
            }
        }
        else
        {
            hasAddedAP = false;
        }


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

    public void SwapPlayer(int _fighterNumber)
    {
        var sequence = DOTween.Sequence();

        if (state == BattleState.DECISIONTURNPLAYER1)
        {
            sequence.Append(currentPlayerFighters[0].transform.DOMove(backupPlayerPosition.position, 0.5f)).
                Append(currentPlayerBackups[_fighterNumber].transform.DOMove(playerPositions[0].position, 0.5f)).
                AppendCallback(() => Swap(0, _fighterNumber));
        }
    }

    private void Swap(int _playerNumber, int _backupNumber)
    {
        currentPlayers[_playerNumber].GetComponent<SelectableFighter>().isBackup = true;
        currentPlayerBackups[_backupNumber].GetComponent<SelectableFighter>().isBackup = false;

        GameObject temp = currentPlayers[_playerNumber];
        currentPlayers[_playerNumber] = currentPlayerBackups[_backupNumber];

        currentPlayerFighters[_playerNumber] = currentPlayerBackups[_backupNumber].GetComponent<Fighter>();

        currentPlayerBackups[_backupNumber] = temp;

        fighters = new List<Fighter>();

        foreach (Fighter fighter in currentPlayerFighters)
        {
            fighters.Add(fighter);
        }
        foreach (Fighter fighter in currentEnemyFighters)
        {
            fighters.Add(fighter);
        }

        playerTeam.RebuildTeam();
    }

    private void ReplaceEnemy()
    {
        var sequence = DOTween.Sequence();

        if (currentEnemyBackups.Count > 0)
        {
            if (!hasAppendedAnimation)
            {
                for (int i = 0; i < currentEnemyFighters.Count; i++)
                {
                    if (currentEnemyFighters[i].fighterState != FighterState.NORMAL)
                    {
                        sequence.Append(currentEnemyFighters[i].transform.DOMove(enemyPositions[i].position + new Vector3(5, 0, 0), 0.5f)).
                            AppendCallback(() => ReplaceWithNextEnemy(i)).
                            AppendInterval(0.5f).
                            AppendCallback(() => RebuildListsEnemies(i)).
                            AppendCallback(() => hasAppendedAnimation = false);
                        hasAppendedAnimation = true;
                        break;

                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < currentEnemyFighters.Count; i++)
            {
                if (currentEnemyFighters[i].fighterState != FighterState.NORMAL)
                {
                    currentEnemyFighters.RemoveAt(i);
                    currentEnemies.RemoveAt(i);
                    enemyPositions.RemoveAt(i);

                    fighters = new List<Fighter>();

                    foreach (Fighter fighter in currentPlayerFighters)
                    {
                        fighters.Add(fighter);
                    }
                    foreach (Fighter fighter in currentEnemyFighters)
                    {
                        fighters.Add(fighter);
                    }

                    enemyTeam.RebuildTeam();
                    break;

                }
            }
        }

        
    }

    public void ReplaceWithNextEnemy(int i)
    {
        var sequence = DOTween.Sequence();

        Destroy(currentEnemies[i]);
        currentEnemies[i] = Instantiate(currentEnemyBackups[0], enemyPositions[i]);
        currentEnemies[i].GetComponent<SelectableFighter>().isBackup = false;
        currentEnemies[i].transform.position += new Vector3(5, 0, 0);

        sequence.Append(currentEnemies[i].transform.DOMove(enemyPositions[i].position, 0.5f));
    }


    private void RebuildListsEnemies(int i)
    {
        currentEnemyFighters[i] = currentEnemies[i].GetComponent<Fighter>();
        currentEnemyBackups.RemoveAt(0);

        fighters = new List<Fighter>();

        foreach (Fighter fighter in currentPlayerFighters)
        {
            fighters.Add(fighter);
        }
        foreach (Fighter fighter in currentEnemyFighters)
        {
            fighters.Add(fighter);
        }

        enemyTeam.RebuildTeam();
    }

    private void TargetSelection()
    {
        playerTeam.SelectTeamOrTarget(currentPlayerFighters[0], state, BattleState.DECISIONTURNPLAYER1);
        playerTeam.SelectTeamOrTarget(currentPlayerFighters[1], state, BattleState.DECISIONTURNPLAYER2);

        enemyTeam.SelectTeamOrTarget(currentPlayerFighters[0], state, BattleState.DECISIONTURNPLAYER1);
        enemyTeam.SelectTeamOrTarget(currentPlayerFighters[1], state, BattleState.DECISIONTURNPLAYER2);

        if (currentPlayerFighters[0].hasChosenMove && currentPlayerFighters[0].hasChosenTarget)
        {
            state = BattleState.DECISIONTURNPLAYER2;
        }

        foreach (Fighter enemyFighter in currentEnemyFighters)
        {
            enemyFighter.ChooseTargetAI(currentEnemyFighters, currentPlayerFighters);
        }
    }

    private void TurnOrder()
    {

        state = BattleState.ACTIONTURN;

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
            state = BattleState.DECISIONTURNPLAYER1;
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
