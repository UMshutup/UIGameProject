using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableFighter : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [HideInInspector] public bool canSelect = false;
    [HideInInspector] public bool hasSelectedTarget;
    [HideInInspector] public bool hasAppended = false;
    [HideInInspector] public bool isOnSprite = false;
    [HideInInspector] public bool isEnemy;

    private void OnMouseEnter()
    {
        if (canSelect)
        {
            isOnSprite = true;
        }
    }

    private void OnMouseExit()
    {
        isOnSprite = false;
    }

    private void OnMouseDown()
    {
        if (canSelect)
        {
            hasSelectedTarget = true;
        }
    }

    private void Update()
    {
        var sequence = DOTween.Sequence();

        if (isOnSprite)
        {
            if (!hasAppended)
            {
                sequence.Append(spriteRenderer.DOColor(new Color(3, 3, 3), 0.2f)).
                    Append(spriteRenderer.DOColor(Color.white, 0.2f)).
                    AppendCallback(() => hasAppended = false);
                hasAppended = true;
            }
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
