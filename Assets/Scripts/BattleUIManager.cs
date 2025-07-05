using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;

    [Header("Game UI")]
    [SerializeField] private GameObject actionsPlayer1;
    [SerializeField] private GameObject actionsPlayer2;
    [SerializeField] private GameObject moveListPlayer1;
    [SerializeField] private GameObject moveListPlayer2;

    [Header("StatsUI")]
    [SerializeField] private GameObject playerStats;
    [SerializeField] private GameObject enemyStats;

    [Header("TeamUI")]
    [SerializeField] private GameObject teamBackgroundUI;
    [SerializeField] private List<FighterStatsUI> mainPlayersUI;
    [SerializeField] private List<FighterStatsUI> backupPlayersUI;

    private List<FighterStatsUI> playerUI;
    private List<FighterStatsUI> enemyUI;



    private bool isMoveList1OnScreen;
    private bool isMoveList2OnScreen;

    private bool isTeamBackgroundUIOnscreen;

    private bool hasAppendedAnimation1 = false;
    private bool hasAppendedAnimation2 = false;
    private bool hasAppendedAnimation3 = false;
    private bool hasAppendedAnimation4 = false;

    private void Start()
    {
        if (battleManager.currentPlayerFighters.Count == 1)
        {
            moveListPlayer2.SetActive(false);
        }

        playerUI = new List<FighterStatsUI>();

        enemyUI = new List<FighterStatsUI>();

        List<FighterStatsUI> playerUITotal = playerStats.GetComponentsInChildren<FighterStatsUI>(true).ToList();
        for (int i = 0; i< battleManager.currentPlayerFighters.Count; i++)
        {
            playerUI.Add(playerUITotal[i]);
            playerUI[i].gameObject.SetActive(true);
        }

        List<FighterStatsUI> enemyUITotal = enemyStats.GetComponentsInChildren<FighterStatsUI>(true).ToList();
        for (int i = 0; i < battleManager.currentEnemyFighters.Count; i++)
        {
            enemyUI.Add(enemyUITotal[i]);
            enemyUI[i].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        MenuAnimations();

        for (int i = 0; i < battleManager.currentPlayerFighters.Count; i++)
        {
            playerUI[i].UpdateStats(battleManager.currentPlayerFighters[i]);
        }

        for (int i = 0; i < battleManager.currentEnemyFighters.Count; i++)
        {
            enemyUI[i].UpdateStats(battleManager.currentEnemyFighters[i]);
        }

        for (int i = battleManager.currentEnemyFighters.Count; i < enemyUI.Count; i++)
        {
            enemyUI[i].gameObject.SetActive(false);
        }



        mainPlayersUI[0].UpdateStats(battleManager.currentPlayerFighters[0]);
        mainPlayersUI[1].UpdateStats(battleManager.currentPlayerFighters[1]);

        for (int i = 0; i < battleManager.currentPlayerBackups.Count; i++)
        {
            if (battleManager.currentPlayerBackups[i].GetComponent<Fighter>().fighterState == FighterState.DEAD)
            {
                backupPlayersUI[i].GetComponent<Button>().interactable = false;
            }
            backupPlayersUI[i].gameObject.SetActive(true) ;
            backupPlayersUI[i].UpdateStats(battleManager.currentPlayerBackups[i].GetComponent<Fighter>());
        }

    }

    private void MenuAnimations()
    {
        var sequence = DOTween.Sequence();

        // show player2 menu
        if (battleManager.state == BattleState.DECISIONTURNPLAYER2 && !actionsPlayer2.activeSelf)
        {
            if (!hasAppendedAnimation1)
            {
                sequence.AppendCallback(() => actionsPlayer2.SetActive(true)).
                    Append(actionsPlayer1.transform.DOLocalMoveY(-667, 0.5f)).
                    Join(actionsPlayer2.transform.DOLocalMoveY(-410, 0.5f)).
                    AppendCallback(() => actionsPlayer1.SetActive(false));

                hasAppendedAnimation1 = true;
            }
        }
        else
        {
            hasAppendedAnimation1 = false;
        }

        // show player1 menu
        if (battleManager.state == BattleState.DECISIONTURNPLAYER1 && !actionsPlayer1.activeSelf)
        {
            if (!hasAppendedAnimation2)
            {
                sequence.AppendCallback(() => actionsPlayer1.SetActive(true)).
                    Append(actionsPlayer2.transform.DOLocalMoveY(-667, 0.5f)).
                    Join(actionsPlayer1.transform.DOLocalMoveY(-410, 0.5f)).
                    AppendCallback(() => actionsPlayer2.SetActive(false));

                hasAppendedAnimation2 = true;
            }
        }
        else
        {
            hasAppendedAnimation2 = false;
        }

        if (battleManager.currentPlayerFighters[0].hasChosenMove)
        {
            if (!hasAppendedAnimation3)
            {
                isMoveList1OnScreen = true;
                ShowHideMoveList1();
                hasAppendedAnimation3 = true;
            }

        }
        else
        {
            hasAppendedAnimation3 = false;
        }

        if (battleManager.currentPlayerFighters[1].hasChosenMove)
        {
            if (!hasAppendedAnimation4)
            {
                isMoveList2OnScreen = true;
                ShowHideMoveList2();
                hasAppendedAnimation4 = true;
            }

        }
        else
        {
            hasAppendedAnimation4 = false;
        }
    }

    public void ShowHideMoveList1()
    {
        if (!isMoveList1OnScreen)
        {
            moveListPlayer1.transform.DOLocalMoveY(227, 0.5f);
            isMoveList1OnScreen = true;
        }
        else
        {
            moveListPlayer1.transform.DOLocalMoveY(-498, 0.5f);
            isMoveList1OnScreen = false;
        }
    }

    public void ShowHideMoveList2()
    {
        if (!isMoveList2OnScreen)
        {
            moveListPlayer2.transform.DOLocalMoveY(227, 0.5f);
            isMoveList2OnScreen = true;
        }
        else
        {
            moveListPlayer2.transform.DOLocalMoveY(-498, 0.5f);
            isMoveList2OnScreen = false;
        }
    }

    public void ShowHideTeamBackground()
    {
        if (!isTeamBackgroundUIOnscreen)
        {
            teamBackgroundUI.transform.DOLocalMoveX(0, 0.5f);
            isTeamBackgroundUIOnscreen = true;
        }
        else
        {
            teamBackgroundUI.transform.DOLocalMoveX(1923, 0.5f);
            isTeamBackgroundUIOnscreen = false;
        }
    }
}
