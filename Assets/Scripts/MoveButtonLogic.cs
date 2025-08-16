using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtonLogic : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private BattleUIManager battleUIManager;
    [SerializeField] private Button button;
    [SerializeField] private int currentPlayerNumber;

    [SerializeField] private TextMeshProUGUI moveName;
    [SerializeField] private TextMeshProUGUI moveDescription;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI moveCategory;

    public Image[] actionPointsCost;

    public Sprite emptyActionPointsSprite;
    public Sprite fullActionPointsSprite;

    private void Awake()
    {
        battleUIManager = FindAnyObjectByType<BattleUIManager>();
    }

    public void SetMoveToButton(int _moveNumber, int _moveCost)
    {
        moveName.text = battleManager.currentPlayerFighters[currentPlayerNumber].currentMoves[_moveNumber].GetMoveName();
        damage.text = string.Format("DMG: {0}", battleManager.currentPlayerFighters[currentPlayerNumber].currentMoves[_moveNumber].GetMoveDamage().ToString());
        moveCategory.text = "(" + battleManager.currentPlayerFighters[currentPlayerNumber].currentMoves[_moveNumber].GetMoveCategory().ToString() + ")";
        moveDescription.text = battleManager.currentPlayerFighters[currentPlayerNumber].currentMoves[_moveNumber].GetMoveDescription();

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
