using System;
using UnityEngine;

[Serializable]
public class EnemySpawnSequence
{
    [SerializeField] private EnemyFactory _factory;
    [SerializeField] private EnemyType _type;
    [SerializeField] [Range(1f, 100f)] private int _amount = 1;
    [SerializeField] [Range(1f, 100f)] private int _coolDown = 1;
    
    public struct State
    {
        private EnemySpawnSequence _sequence;
        private int _count;
        private float _cooldDown;

        public State(EnemySpawnSequence sequence)
        {
            _sequence = sequence;
            _count = 0;
            _cooldDown = sequence._coolDown;
        }

        public float Progress(float deltaTime)
        {
            _cooldDown += deltaTime;

            while (_cooldDown >= _sequence._coolDown)
            {
                _cooldDown -= _sequence._coolDown;

                if (_count >= _sequence._amount)
                {
                    return _cooldDown;
                }

                _count++;
                Game.SpawnEnemy(_sequence._factory, _sequence._type);
            }

            return -1f;
        }
    }

    public State Begin() => new State(this);
}