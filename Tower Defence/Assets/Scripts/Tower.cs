using System;
using UnityEngine;

public abstract class Tower : GameTileContent
{
    [SerializeField] [Range(1.5f, 10.5f)] protected float _targetingRange = 1.5f;

    public abstract TowerType Type { get; }
    protected Vector3 DebugGraphicsPosition => transform.localPosition + Vector3.up * 0.01f;
    
    protected bool IsAcquireTarget(out TargetPoint target)
    {
        if (TargetPoint.FillBuffer(transform.localPosition, _targetingRange))
        {
            target = TargetPoint.GetBuffered(0);
            return true;
        }

        target = null;
        return false;
    }

    protected bool IsTargetTracked(ref TargetPoint target)
    {
        if (target == null)
        {
            return false;
        }

        Vector3 ownPosition = transform.localPosition;
        Vector3 targetPosition = target.Position;

        if ((targetPosition - ownPosition).sqrMagnitude > _targetingRange * _targetingRange + target.ColliderSize * target.Enemy.Scale)
        {
            target = null;
            return false;
        }

        return true;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(DebugGraphicsPosition, _targetingRange);
    }
}