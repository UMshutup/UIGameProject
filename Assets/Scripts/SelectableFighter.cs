using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableFighter : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public bool isBackup = true;

    [HideInInspector] public bool canSelect = false;
    [HideInInspector] public bool hasSelectedTarget;
    private bool hasAppended = false;
    [HideInInspector] public bool isOnSprite = false;
    [HideInInspector] public bool canGlow = false;

    private void OnMouseEnter()
    {
        if (canSelect)
        {
            isOnSprite = true;
            canGlow = true;
        }
        else
        {
            isOnSprite = false;
            canGlow = false;
        }
    }

    private void OnMouseExit()
    {
        isOnSprite = false;
        canGlow = false;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (canSelect && isOnSprite)
        {
            hasSelectedTarget = true;
        }
    }

    private void Update()
    {
        var sequence = DOTween.Sequence();

        if (canGlow)
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
