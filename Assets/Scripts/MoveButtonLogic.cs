using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtonLogic : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private Button button;
    public void SetMoveToButton(int _moveNumber)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = battleManager.currentPlayerFighter.moves[_moveNumber].GetMoveName();
        int test = _moveNumber;
        button.onClick.AddListener(() => battleManager.MoveSelected(test));
    }
}
