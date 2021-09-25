using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    private static LayerMask? _enemyLayerMask;

    private static Collider[] _buffer = new Collider[100];

    public static int BufferedCount { get; private set; }
    public Enemy Enemy { get; private set; }
    public Vector3 Position => transform.position;
    public float ColliderSize { get; private set; }

    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
        ColliderSize = GetComponent<SphereCollider>().radius * transform.localScale.x;
    }

    public static bool FillBuffer(Vector3 position, float range)
    {
        _enemyLayerMask ??= 1 << LayerMask.NameToLayer("Enemy");
        Vector3 top = position;
        top.y += 3f;
        BufferedCount = Physics.OverlapCapsuleNonAlloc(position, top, range, _buffer, _enemyLayerMask.Value);

        return BufferedCount > 0;
    }

    public static TargetPoint GetBuffered(int index)
    {
        var target = _buffer[index].GetComponent<TargetPoint>();
        return target;
    }
}