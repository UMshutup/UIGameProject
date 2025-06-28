using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtonLogic : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private BattleUIManager battleUIManager;
    [SerializeField] private Button button;
    [SerializeField] private int currentPlayerNumber;

    public Image[] actionPointsCost;

    public Sprite emptyActionPointsSprite;
    public Sprite fullActionPointsSprite;

    private void Awake()
    {
        battleUIManager = FindAnyObjectByType<BattleUIManager>();
    }

    public void SetMoveToButton(int _moveNumber, int _moveCost)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = battleManager.currentPlayerFighters[currentPlayerNumber].currentMoves[_moveNumber].GetMoveName();

        for (int i = 0; i < actionPointsCost.Length; i++)
        {
            if (i + 1 <= _moveCost)
            {
                actionPointsCost[i].sprite = fullActionPointsSprite;
            }
            else
            {
                actionPointsCost[i].sprite = emptyActionPointsSprite;
            }
        }

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
