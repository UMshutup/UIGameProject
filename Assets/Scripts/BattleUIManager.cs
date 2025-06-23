using DG.Tweening;
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

    [Header("Player UI")]
    [SerializeField] private GameObject moveList;

    private bool isMoveListOnScreen;

    private bool hasHiddenMoveList = false;

    private void Update()
    {
        AssignUIInformation();

        if (battleManager.hasMoveBeenSelected && !hasHiddenMoveList)
        {
            isMoveListOnScreen = true;
            ShowHideMoveList();
            hasHiddenMoveList=true;
        }

    }

    private void AssignUIInformation()
    {
        playerNameText.text = battleManager.currentPlayerFighter.fighterName;
        playerHealthBar.fillAmount = battleManager.currentPlayerFighter.currentHP / battleManager.currentPlayerFighter.maxHP;

        enemyNameText.text = battleManager.currentEnemyFighter.fighterName;
        enemyHealthBar.fillAmount = battleManager.currentEnemyFighter.currentHP / battleManager.currentEnemyFighter.maxHP;
    }

    public void ShowHideMoveList()
    {

        if (!isMoveListOnScreen)
        {
            moveList.transform.DOLocalMoveY(227, 0.5f);
            isMoveListOnScreen = true;
            hasHiddenMoveList = false ;
        }
        else
        {
            moveList.transform.DOLocalMoveY(-498, 0.5f);
            isMoveListOnScreen = false;
        }
    }
}
