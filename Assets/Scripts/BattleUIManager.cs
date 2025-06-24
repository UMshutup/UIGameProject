using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;

    [Header("Player UI")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image playerHealthBar;

    [Header("Enemy UI")]
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private Image enemyHealthBar;

    [Header("Game UI")]
    [SerializeField] private GameObject actionsPlayer1;
    [SerializeField] private GameObject actionsPlayer2;
    [SerializeField] private GameObject moveListPlayer1;
    [SerializeField] private GameObject moveListPlayer2;



    private bool isMoveList1OnScreen;
    private bool isMoveList2OnScreen;

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
    }

    private void Update()
    {
        var sequence = DOTween.Sequence();

        // show player2 menu
        if (actionsPlayer1.activeSelf && battleManager.currentPlayerFighters[0].hasChosenTarget)
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
        if (!battleManager.currentPlayerFighters[0].hasChosenTarget && !battleManager.currentPlayerFighters[1].hasChosenTarget)
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

        AssignUIInformation();

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

    private void AssignUIInformation()
    {
        playerNameText.text = battleManager.currentPlayerFighters[0].fighterName;
        playerHealthBar.fillAmount = battleManager.currentPlayerFighters[0].currentHP / battleManager.currentPlayerFighters[0].maxHP;

        enemyNameText.text = battleManager.currentEnemyFighters[0].fighterName;
        enemyHealthBar.fillAmount = battleManager.currentEnemyFighters[0].currentHP / battleManager.currentEnemyFighters[0].maxHP;
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
}
