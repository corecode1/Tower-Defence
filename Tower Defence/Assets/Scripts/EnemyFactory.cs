using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField] private Enemy _prefab;

    [SerializeField] [FloatRangeSlider(0.5f, 2f)]
    private FloatRange _scale = new FloatRange(1f);
    
    [SerializeField] [FloatRangeSlider(-0.4f, 0.4f)]
    private FloatRange _pathOffset = new FloatRange(0f);

    [SerializeField] [FloatRangeSlider(0.2f, 5f)]
    private FloatRange _speed = new FloatRange(0f);

    [SerializeField] private EnemyConfig _small;
    [SerializeField] private EnemyConfig _medium;
    [SerializeField] private EnemyConfig _large;

    public Enemy Get(EnemyType type)
    {
        EnemyConfig config = GetConfig(type);
        Enemy instance = CreateGameObjectInstance(config.Prefab);
        instance.OriginFactory = this;
        instance.Initialize(config.Scale.RandomValueInRange,
            config.PathOffset.RandomValueInRange,
            config.Speed.RandomValueInRange,
            config.Health.RandomValueInRange);

        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    private EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Large:
                return _large;
            case EnemyType.Medium:
                return _medium;
            case EnemyType.Small:
                return _small;
        }
        
        Debug.LogError($"Couldn't find config for enemy type: {type}");
        return _medium;
    }
}