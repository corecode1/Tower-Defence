using UnityEngine;

public class Shell : WarEntity
{
    public Vector3 _launchPoint;
    public Vector3 _targetPoint;
    public Vector3 _launchVelocity;
    
    private float _blastRadius = 1f;
    private float _damage;
    private float _timeSinceLaunch;

    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity, float damage, float blastRadius)
    {
        _launchPoint = launchPoint;
        _targetPoint = targetPoint;
        _launchVelocity = launchVelocity;
        _damage = damage;
        _blastRadius = blastRadius;
    }

    public override bool GameUpdate()
    {
        _timeSinceLaunch += Time.deltaTime;
        Vector3 point = _launchPoint + _launchVelocity * _timeSinceLaunch;
        point.y -= 0.5f * 9.81f * _timeSinceLaunch * _timeSinceLaunch;

        if (point.y <= 0)
        {
            Game.SpawnExplosion().Initialize(_targetPoint, _blastRadius, _damage);
            OriginFactory.Reclaim(this);
            return false;
        }
        
        transform.localPosition = point;

        Vector3 d = _launchVelocity;
        d.y -= 9.81f * _timeSinceLaunch;
        transform.localRotation = Quaternion.LookRotation(d);

        Game.SpawnExplosion().Initialize(point, 0.1f);

        return true;
    }
}