using System;
using System.Collections.Generic;

[Serializable]
public class GameBehaviourCollection
{
    private List<GameBehaviour> _behaviours = new List<GameBehaviour>();

    public void Add(GameBehaviour behaviour)
    {
        _behaviours.Add(behaviour);
    }

    public void GameUpdate()
    {
        for (int i = _behaviours.Count - 1; i >= 0; i--)
        {
            if (!_behaviours[i].GameUpdate())
            {
                _behaviours.RemoveAt(i);
            }
        }
    }
}