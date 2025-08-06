using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssignCharacters : MonoBehaviour
{
    [SerializeField] private List<CharacterSelection> selectedPlayerCharacters;

    public void Assign()
    {
        for (int i = 0; i < selectedPlayerCharacters.Count; i++)
        {
            if(selectedPlayerCharacters[i].fighterList[selectedPlayerCharacters[i].selectionNumber] != null)
            {
                ChosenCharacters.playerFighters.Add(selectedPlayerCharacters[i].fighterList[selectedPlayerCharacters[i].selectionNumber]);
            }
        }

        SceneManager.LoadScene("Battle");
    }
}
