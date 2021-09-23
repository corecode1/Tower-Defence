using UnityEngine;

[CreateAssetMenu]
public class WarFactory : GameObjectFactory
{
    [SerializeField] private Shell _shellPrefab;

    public Shell Shell => Get(_shellPrefab);

    private T Get<T>(T prefab) where T : WarEntity
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
    
    public void Reclaim(WarEntity entity)
    {
        Destroy(entity.gameObject);
    }
}
