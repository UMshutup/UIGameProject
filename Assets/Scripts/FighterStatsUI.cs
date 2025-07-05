using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FighterStatsUI : MonoBehaviour
{
    public TextMeshProUGUI fighterName;
    public Image healthBar;
    public Image vitalityBar;
    public Image[] actionPoints;
    public Image spriteImage;

    public Sprite emptyActionPointsSprite;
    public Sprite fullActionPointsSprite;

    [Range(0, 10)] public int testMaxPoints;
    [Range(0, 10)] public int testPoints;

    public void UpdateStats(Fighter _fighter)
    {

        if (spriteImage != null)
        {
            spriteImage.sprite = _fighter.GetComponent<SpriteRenderer>().sprite;
            spriteImage.SetNativeSize();
        }

        fighterName.text = _fighter.fighterName;
        healthBar.fillAmount = _fighter.currentHP / _fighter.maxHP;
        vitalityBar.fillAmount = 1f - Mathf.Max( _fighter.currentHP / _fighter.minHP, 0);
        if (testMaxPoints != 0)
        {
            for (int i = 0; i < actionPoints.Length; i++)
            {
                if (i+1 <= _fighter.maxActionPoints )
                {
                    actionPoints[i].gameObject.SetActive(true);
                }
                else
                {
                    actionPoints[i].gameObject.SetActive(false);
                }

                if (i + 1 <= _fighter.currentActionPoints)
                {
                    actionPoints[i].sprite = fullActionPointsSprite;
                }
                else
                {
                    actionPoints[i].sprite = emptyActionPointsSprite;
                }
            }
        }
    }
}
