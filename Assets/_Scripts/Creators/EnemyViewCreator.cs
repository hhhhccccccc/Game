using UnityEngine;

public class EnemyViewCreator : Singleton<EnemyViewCreator>
{
    [SerializeField] private EnemyView enemyViewPrefab;

    public EnemyView CreateEnemyView(EnemyConfig enemyConfig, Vector3 pos, Quaternion rot)
    {
        EnemyView enemyView = Instantiate(enemyViewPrefab, pos, rot);
        enemyView.Setup(enemyConfig);
        return enemyView;
    }
}