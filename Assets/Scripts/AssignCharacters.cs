using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssignCharacters : MonoBehaviour
{
    [SerializeField] private List<CharacterSelection> selectedPlayerCharacters;

    [SerializeField] private List<CharacterSelection> selectedEnemyCharacters;

    [SerializeField] private NumberOfCharacterSelection numberOfCharacters;

    public void Assign()
    {
        for (int i = 0; i < selectedPlayerCharacters.Count; i++)
        {
            if(selectedPlayerCharacters[i].fighterList[selectedPlayerCharacters[i].selectionNumber] != null)
            {
                ChosenCharacters.playerFighters.Add(selectedPlayerCharacters[i].fighterList[selectedPlayerCharacters[i].selectionNumber]);
            }
        }

        for (int i = 0; i < selectedEnemyCharacters.Count; i++)
        {
            if (selectedEnemyCharacters[i].fighterList[selectedEnemyCharacters[i].selectionNumber] != null)
            {
                ChosenCharacters.enemyFighters.Add(selectedEnemyCharacters[i].fighterList[selectedEnemyCharacters[i].selectionNumber]);
            }
        }

        ChosenCharacters.numberOfEnemies = numberOfCharacters.selectionNumber;

        SceneManager.LoadScene("Battle");
    }
}
