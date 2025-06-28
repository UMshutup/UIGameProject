using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectableTeam : MonoBehaviour
{
    private List<SelectableFighter> selectableFighters;
    private List<Fighter> fighters;

    private bool selectWholeTeam;
    private bool isOnSpriteWholeTeam;
    private SelectableFighter selectedFighter;

    public MoveTarget teamTarget;

    private void Start()
    {
        selectableFighters = gameObject.GetComponentsInChildren<SelectableFighter>().ToList();

        fighters = new List<Fighter>();

        for (int i = 0; i < selectableFighters.Count; i++)
        {
            fighters.Add(selectableFighters[i].gameObject.GetComponent<Fighter>());
        }
    }

    public void SelectTeamOrTarget(Fighter _fighter, BattleState _currentBattleState, BattleState _choosingStateOfPlayer)
    {
        if ((teamTarget == _fighter.chosenMove.GetMoveTarget() || (_fighter.chosenMove.GetMoveTarget() == MoveTarget.BOTH) || (_fighter.chosenMove.GetMoveTarget() == MoveTarget.SELF)) 
            && _fighter.hasChosenMove && _currentBattleState == _choosingStateOfPlayer)
        {
            if (_fighter.chosenMove.GetHitsAWholeSquad())
            {
                foreach (SelectableFighter selectable in selectableFighters)
                {
                    if (_fighter.hasChosenMove && _currentBattleState == _choosingStateOfPlayer && !_fighter.hasChosenTarget)
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

                    if (selectWholeTeam && _currentBattleState == _choosingStateOfPlayer)
                    {
                        selectedFighter.canSelect = false;
                        selectedFighter.canGlow = false;
                        selectedFighter.isOnSprite = false;

                        _fighter.ChooseTarget(fighters);

                        selectable.canSelect = false;
                        selectable.hasSelectedTarget = false;

                        isOnSpriteWholeTeam = false;
                        selectWholeTeam = false;
                    }

                }
            }
            else
            {
                foreach (SelectableFighter selectable in selectableFighters)
                {
                    if (_fighter.hasChosenMove && _currentBattleState == _choosingStateOfPlayer && !_fighter.hasChosenTarget)
                    {
                        if (_fighter.chosenMove.GetMoveTarget() != MoveTarget.SELF)
                        {
                            selectable.canSelect = true;
                        }
                        else
                        {
                            if (selectable.GetComponent<Fighter>().Equals(_fighter))
                            {
                                selectable.canSelect = true;
                            }
                        }

                        
                    }
                    else
                    {
                        selectable.canSelect = false;
                        selectable.hasSelectedTarget = false;
                    }

                    if (selectable.hasSelectedTarget && _currentBattleState == _choosingStateOfPlayer)
                    {
                        _fighter.ChooseTarget(new List<Fighter> { selectable.gameObject.GetComponent<Fighter>() });

                        selectable.hasSelectedTarget = false;

                        selectable.canSelect = false;

                    }
                }
            }
        }


    }





}
