using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtonLogic : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private BattleUIManager battleUIManager;
    [SerializeField] private Button button;
    [SerializeField] private int currentPlayerNumber;

    private void Awake()
    {
        battleUIManager = FindAnyObjectByType<BattleUIManager>();
    }

    public void SetMoveToButton(int _moveNumber)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = battleManager.currentPlayerFighters[currentPlayerNumber].moves[_moveNumber].GetMoveName();
        int moveNumber = _moveNumber;

        if (currentPlayerNumber == 0)
        {
            button.onClick.AddListener(() => battleManager.MoveSelectionPlayer1(moveNumber));
        }
        else if(currentPlayerNumber == 1)
        {
            button.onClick.AddListener(() => battleManager.MoveSelectionPlayer2(moveNumber));
        }
        
    }
    public void SetShowHide()
    {
        if (currentPlayerNumber == 0)
        {
            battleUIManager.ShowHideMoveList1();
        }
        else if (currentPlayerNumber == 1)
        {
            battleUIManager.ShowHideMoveList2();
        }

        
    }
}
