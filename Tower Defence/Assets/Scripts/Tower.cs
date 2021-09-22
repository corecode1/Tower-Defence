using System;
using UnityEngine;

public class Tower : GameTileContent
{
    [SerializeField] [Range(1.5f, 10.5f)] private float _targetingRange = 1.5f;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Transform _turret;
    [SerializeField] private Transform _laserBeam;
    [SerializeField] [Range(1f, 100f)] private float _damagePerSecond = 10f;

    private Vector3 _laserBeamScale;
    
    private TargetPoint _target;

    public override void GameUpdate()
    {
        if (IsTargetTracked() || IsAcquireTarget())
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

    private bool IsAcquireTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.localPosition, _targetingRange, _enemyLayerMask);

        if (targets.Length > 0)
        {
            _target = targets[0].GetComponent<TargetPoint>();
            return true;
        }

        _target = null;
        return false;
    }

    private bool IsTargetTracked()
    {
        if (_target == null)
        {
            return false;
        }

        Vector3 ownPosition = transform.localPosition;
        Vector3 targetPosition = _target.Position;

        if ((targetPosition - ownPosition).sqrMagnitude > _targetingRange * _targetingRange + _target.ColliderSize * _target.Enemy.Scale)
        {
            _target = null;
            return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition + Vector3.up * 0.01f;
        Gizmos.DrawWireSphere(position, _targetingRange);

        if (_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, _target.Position);
        }
    }
}