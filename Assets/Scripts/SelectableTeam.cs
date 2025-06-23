using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectableTeam : MonoBehaviour
{
    private List<SelectableFighter> selectableFighters;

    [HideInInspector] public bool canSelect = false;
    [HideInInspector] public bool hasSelectedTeam;

    private void Start()
    {
        selectableFighters = GetComponentsInChildren<SelectableFighter>().ToList();
    }

    private void OnMouseEnter()
    {
        if (canSelect)
        {
            foreach (SelectableFighter fighter in selectableFighters)
            {
                fighter.canSelect = true;
                fighter.isOnSprite = true;
            }
        }
    }

    private void OnMouseExit()
    {
        foreach (SelectableFighter fighter in selectableFighters)
        {
            fighter.canSelect = false;
            fighter.isOnSprite = false;
        }
    }
    private void OnMouseDown()
    {
        if (canSelect)
        {
            hasSelectedTeam = true;
        }
    }

    private Fighter GetFighterFromTeam(int _fighterNumber)
    {
        if (hasSelectedTeam)
        {
            for (int i = 0; i < selectableFighters.Count; i++)
            {
                if (i == _fighterNumber)
                {
                    return selectableFighters[i].GetComponent<Fighter>();
                }
            }
        }
        return null;
    }


}
