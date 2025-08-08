using DG.Tweening;
using UnityEngine;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private GameObject creditsMenu;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        audioManager.PlayMusic(audioManager.menuMusic, 1f);
    }

    public void ShowMenu()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(selectionMenu.transform.DOLocalMoveY(0, 0.8f));
    }

    public void HideMenu()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(selectionMenu.transform.DOLocalMoveY(1079.7f, 0.8f));
    }

    public void ShowCreditsMenu()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(creditsMenu.transform.DOLocalMoveY(0, 0.8f));
    }

    public void HideCreditsMenu()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(creditsMenu.transform.DOLocalMoveY(1079.7f, 0.8f));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
