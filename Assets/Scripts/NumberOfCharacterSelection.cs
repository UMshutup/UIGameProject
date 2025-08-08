using TMPro;
using UnityEngine;

public class NumberOfCharacterSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;

    public int selectionNumber = 1;

    private void Update()
    {
        switch (selectionNumber)
        {
            case 1:
                numberText.text = "2 vs 1";
                break;
            case 2:
                numberText.text = "2 vs 2";
                break;
            case 3:
                numberText.text = "2 vs 3";
                break;
        }

        if (selectionNumber >= 4)
        {
            selectionNumber = 1;
        }
    }

    public void ChangeNumber()
    {
        selectionNumber++;
    }
}
