using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public void SpawEnemy()
    {
        var enemy = Instantiate(enemyPrefab, transform.position, transform.rotation, transform).GetComponentInChildren<Enemy>();
        enemy.spawner = this;
    }

    private void Start()
    {
        SpawEnemy();
    }

}
