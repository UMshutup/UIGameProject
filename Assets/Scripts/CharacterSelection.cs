using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public int characterSlot;

    public List<GameObject> fighterList;

    private int selectionNumber = 0;

    [SerializeField] private Image characterImage;

    [SerializeField] private Sprite NoCharacter;

    private void Update()
    {
        if (fighterList[selectionNumber] == null)
        {
            characterImage.sprite = NoCharacter;
        }
        else
        {
            characterImage.sprite = fighterList[selectionNumber].GetComponent<SpriteRenderer>().sprite;
        }
        characterImage.SetNativeSize();
    }

    public void ChangeChar()
    {
        if (selectionNumber >= fighterList.Count-1) 
        { 
            selectionNumber = 0;
        }
        else
        {
            selectionNumber++;
        }
    }
}
