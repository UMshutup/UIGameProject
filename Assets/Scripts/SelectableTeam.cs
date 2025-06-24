using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectableTeam : MonoBehaviour
{
    private List<SelectableFighter> selectableFighters;
    private List<Fighter> fighters;
    public BattleManager battleManager;

    private bool selectWholeTeam;
    private bool isOnSpriteWholeTeam;
    private SelectableFighter selectedFighter;

    private void Start()
    {
        selectableFighters = gameObject.GetComponentsInChildren<SelectableFighter>().ToList();

        fighters = new List<Fighter>();

        for (int i = 0; i < selectableFighters.Count; i++)
        {
            fighters.Add(selectableFighters[i].gameObject.GetComponent<Fighter>());
        }
    }

    public void SelectTeamOrTarget(List<Fighter> _playerFighters, bool _isPlayer1Choosing)
    {

        ChooseTarget(_playerFighters[0], _isPlayer1Choosing);
        ChooseTarget(_playerFighters[1], !_isPlayer1Choosing);

    }

    private void ChooseTarget(Fighter _fighter, bool _isPlayer1Choosing)
    {
        if (_fighter.hasChosenMove && _isPlayer1Choosing)
        {
            if (_fighter.chosenMove.GetHitsAWholeSquad())
            {
                foreach (SelectableFighter selectable in selectableFighters)
                {
                    if (_fighter.hasChosenMove && _isPlayer1Choosing && !_fighter.hasChosenTarget)
                    {
                        selectable.canSelect = true;
                    }
                    else
                    {
                        selectable.canSelect = false;
                        selectable.hasSelectedTarget = false;
                    }

                    if (selectable.isOnSprite)
                    {
                        selectedFighter = selectable;
                    }

                    if (selectable.Equals(selectedFighter) && selectedFighter.isOnSprite)
                    {
                        isOnSpriteWholeTeam = true;
                    }
                    else if (selectable.Equals(selectedFighter) && !selectedFighter.isOnSprite)
                    {
                        isOnSpriteWholeTeam = false;
                        selectedFighter.canGlow = false;
                        selectedFighter.isOnSprite = false;
                    }


                    if (isOnSpriteWholeTeam)
                    {
                        selectable.canGlow = true;
                    }
                    else
                    {
                        selectable.canGlow = false;
                    }

                    if (selectable.hasSelectedTarget)
                    {

                        selectWholeTeam = true;
                    }

                    if (selectWholeTeam && _isPlayer1Choosing)
                    {
                        selectedFighter.canSelect = false;
                        selectedFighter.canGlow = false;
                        selectedFighter.isOnSprite = false;

                        _fighter.ChooseTarget(fighters);

                        selectable.canSelect = false;
                        selectable.hasSelectedTarget = false;

                        isOnSpriteWholeTeam = false;
                        selectWholeTeam = false;
                        _isPlayer1Choosing = false;
                    }

                }
            }
            else
            {
                foreach (SelectableFighter selectable in selectableFighters)
                {
                    if (_fighter.hasChosenMove && _isPlayer1Choosing && !_fighter.hasChosenTarget)
                    {
                        selectable.canSelect = true;
                    }
                    else
                    {
                        selectable.canSelect = false;
                        selectable.hasSelectedTarget = false;
                    }

                    if (selectable.hasSelectedTarget && _isPlayer1Choosing)
                    {
                        _fighter.ChooseTarget(new List<Fighter> { selectable.gameObject.GetComponent<Fighter>() });

                        selectable.hasSelectedTarget = false;

                        selectable.canSelect = false;

                        _isPlayer1Choosing = false;
                    }
                }
            }
        }
    }

    

}
