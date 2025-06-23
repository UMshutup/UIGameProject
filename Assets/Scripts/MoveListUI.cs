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
    private MoveButtonLogic[] buttons;

    private void Start()
    {
        buttons = choiceList.transform.GetComponentsInChildren<MoveButtonLogic>(true);

        for (int i = 0; i < battleManager.currentPlayerFighter.moves.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].SetMoveToButton(i);
        }

        choiceList.sizeDelta = new Vector2(choiceList.rect.width, choiceList.childCount * 125);
        scrollBar.value = 2;
    }

    private void Update()
    {
        
    }

}
