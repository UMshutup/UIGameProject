using System.Collections.Generic;
using System.Linq;
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

    public GameObject statusEffectZone;

    [Range(0, 10)] public int testMaxPoints;
    [Range(0, 10)] public int testPoints;

    private bool hasMadeList = false;

    private List<StatusEffectInstance> list;

    private void Start()
    {
        list = new List<StatusEffectInstance>();
    }

    public void UpdateStats(Fighter _fighter, List<StatusEffectSO> _statusEffects)
    {

        if (spriteImage != null)
        {
            spriteImage.sprite = _fighter.GetComponent<SpriteRenderer>().sprite;
            spriteImage.SetNativeSize();
        }

        fighterName.text = _fighter.fighterName;
        healthBar.fillAmount = _fighter.currentHP / _fighter.maxHP;
        vitalityBar.fillAmount = 1f - Mathf.Max( _fighter.currentHP / _fighter.minHP, 0);

        //convert to SO to instance
        if (!hasMadeList)
        {
            for (int i = 0; i < _statusEffects.Count; i++)
            {
                list.Add(new StatusEffectInstance(_statusEffects[i]));
            }
            hasMadeList = true;
        }

        UpdateStatusEffects(_fighter, list);

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

    public void UpdateStatusEffects(Fighter _fighter, List<StatusEffectInstance> _statusEffectInstances)
    {
        if (statusEffectZone != null)
        {
            List<Image> icons = statusEffectZone.GetComponentsInChildren<Image>(true).ToList();

            for (int i = 0; i < _statusEffectInstances.Count; i++)
            {
                if (_fighter.statusEffectInstances.Count > 0)
                {
                    for (int j = 0; j < _fighter.statusEffectInstances.Count; j++)
                    {
                        if (_fighter.statusEffectInstances[j].Equals(_statusEffectInstances[i]))
                        {
                            icons[i].gameObject.SetActive(true);
                            icons[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _fighter.statusEffectInstances[j].statusEffectDuration.ToString();
                        }
                        
                    }
                }
                else
                {
                    icons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
