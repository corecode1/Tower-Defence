using UnityEngine;

public class LaserTower : Tower
{
    [SerializeField] [Range(1f, 100f)] private float _damagePerSecond = 10f;
    [SerializeField] private Transform _turret;
    [SerializeField] private Transform _laserBeam;
    
    private Vector3 _laserBeamScale;
    
    private TargetPoint _target;

    public override TowerType Type => TowerType.Laser;

    public override void GameUpdate()
    {
        if (IsTargetTracked(ref _target) || IsAcquireTarget(out _target))
        {
            Shoot();
        }
        else
        {
            _laserBeam.localScale = Vector3.zero;
        }
    }

    private void Awake()
    {
        _laserBeamScale = _laserBeam.localScale;
    }
    
    private void Shoot()
    {
        Vector3 point = _target.Position;
        _turret.LookAt(point);
        _laserBeam.localRotation = _turret.localRotation;

        float distance = Vector3.Distance(_turret.position, point);
        _laserBeamScale.z = distance;
        _laserBeam.localScale = _laserBeamScale;
        _laserBeam.localPosition = _turret.localPosition + 0.5f * distance * _laserBeam.forward;
        
        _target.Enemy.TakeDamage(_damagePerSecond * Time.deltaTime);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        if (_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(DebugGraphicsPosition, _target.Position);
        }
    }
}