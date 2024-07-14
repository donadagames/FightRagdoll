using UnityEngine;

public class DepositCollider : MonoBehaviour
{
    Player player;
    [SerializeField] UIController UIController;
    [SerializeField] PileController pileController;
    [SerializeField] DepositParent depositParent;
    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            player.money += 20;
            UIController.UpdateMoney(player.money);
            enemy.spawner.SpawEnemy();
            Destroy(enemy.m_Enemy);
            player.DepositSound();

            if (enemy.isLastEnemy)
            {
                depositParent.ResetDepositParent();
            }
        }
    }
}
