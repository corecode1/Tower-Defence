using System;
using System.Collections.Generic;

[Serializable]
public class EnemyCollection
{
    private List<Enemy> _enemies = new List<Enemy>();

    public void Add(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void GameUpdate()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            if (!_enemies[i].GameUpdate())
            {
                _enemies.RemoveAt(i);
            }
        }
    }
}