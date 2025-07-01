using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveListUI : MonoBehaviour
{
    [SerializeField] private RectTransform choiceList;
    [SerializeField] private Scrollbar scrollBar;

    [SerializeField] private GameObject choiceButtonPrefab;

    [SerializeField] private BattleManager battleManager;

    [SerializeField] private int currentPlayerNumber;

    private MoveButtonLogic[] buttons;

    private bool hasScrolled = false;

    private void Update()
    {
        buttons = choiceList.transform.GetComponentsInChildren<MoveButtonLogic>(true);

        for (int i = 0; i < battleManager.currentPlayerFighters[currentPlayerNumber].currentMoves.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].SetMoveToButton(i, battleManager.currentPlayerFighters[currentPlayerNumber].currentMoves[i].GetMoveApCost());
            if (battleManager.currentPlayerFighters[currentPlayerNumber].currentMoves[i].GetMoveApCost() > battleManager.currentPlayerFighters[currentPlayerNumber].currentActionPoints)
            {
                buttons[i].gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                buttons[i].gameObject.GetComponent<Button>().interactable = true;
            }
        }

        for(int i = battleManager.currentPlayerFighters[currentPlayerNumber].currentMoves.Count; i < 8; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }

        choiceList.sizeDelta = new Vector2(choiceList.rect.width, choiceList.childCount * 125);
        
        if (!hasScrolled)
        {
            scrollBar.value = 2;
            hasScrolled = true;
        }
    }

}
