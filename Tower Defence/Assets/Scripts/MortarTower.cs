using System;
using UnityEngine;

public class MortarTower : Tower
{
    [SerializeField] [Range(0.5f, 2f)] private float _shotsPerSecond = 1f;
    [SerializeField] private Transform _mortar;

    public override TowerType Type => TowerType.Mortar;

    private float _launchSpeed;
    private float _launchProgress;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        float x = _targetingRange + 0.251f;
        float y = -_mortar.position.y;

        _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
    }

    public override void GameUpdate()
    {
        _launchProgress += Time.deltaTime * _shotsPerSecond;

        while (_launchProgress > 1)
        {
            if (IsAcquireTarget(out TargetPoint target))
            {
                Launch(target);
                _launchProgress -= 1;
            }
            else
            {
                _launchProgress = 0.999f;
            }
        }
    }

    private void Launch(TargetPoint target)
    {
        Vector3 launchPoint = _mortar.position;
        Vector3 targetPoint = target.Position;
        targetPoint.y = 0f;

        Vector2 directoin;
        directoin.x = targetPoint.x - launchPoint.x;
        directoin.y = targetPoint.z - launchPoint.z;

        float x = directoin.magnitude;
        float y = -launchPoint.y;
        directoin /= x;

        float g = 9.81f;
        float s = _launchSpeed;
        float s2 = s * s;

        float r = s2 * s2 - g * (g* x * x + 2f * y * s2);
        float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;

        _mortar.localRotation = Quaternion.LookRotation(new Vector3(directoin.x, tanTheta, directoin.y));

        Vector3 launchVelocity = new Vector3(s * cosTheta * directoin.x, s * sinTheta, s * cosTheta * directoin.y);
        Game.SpawnShell().Initialize(launchPoint, targetPoint, launchVelocity);
    }
}