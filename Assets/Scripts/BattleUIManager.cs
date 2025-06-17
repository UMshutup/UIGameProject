using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;

    [Header("Player UI")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image playerHealthBar;

    [Header("Enemy UI")]
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private Image enemyHealthBar;

    private void Update()
    {
        AssignUIInformation();
    }

    private void AssignUIInformation()
    {
        playerNameText.text = battleManager.currentPlayerFighter.fighterName;
        playerHealthBar.fillAmount = battleManager.currentPlayerFighter.currentHP / battleManager.currentPlayerFighter.maxHP;

        enemyNameText.text = battleManager.currentEnemyFighter.fighterName;
        enemyHealthBar.fillAmount = battleManager.currentEnemyFighter.currentHP / battleManager.currentEnemyFighter.maxHP;
    }
}
