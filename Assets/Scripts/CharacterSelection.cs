using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public int characterSlot;

    public List<GameObject> fighterList;

    [HideInInspector] public int selectionNumber = 0;

    [SerializeField] private Image characterImage;

    [SerializeField] private Sprite NoCharacter;
    [SerializeField] private TextMeshProUGUI characterText;
    [SerializeField] private TextMeshProUGUI characterDescription;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (fighterList[selectionNumber] == null)
        {
            characterImage.sprite = NoCharacter;
            characterText.text = "Empty";
            characterDescription.text = "Just an empty slot";
        }
        else
        {
            characterImage.sprite = fighterList[selectionNumber].GetComponent<SpriteRenderer>().sprite;
            characterText.text = fighterList[selectionNumber].GetComponent<Fighter>().fighterName;
            characterDescription.text = fighterList[selectionNumber].GetComponent<Fighter>().fighterDescription;
        }
        characterImage.SetNativeSize();
    }

    public void ChangeChar()
    {
        audioManager.PlayUISound();

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
