using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField] private Transform _arrow;

    private GameTile _up;
    private GameTile _down;
    private GameTile _left;
    private GameTile _right;
    private GameTile _nextOnPath;
    private int _distance;
    public bool HasPath => _distance != int.MaxValue;
    public bool IsAlternative { get; set; }

    public int x, y;

    private Quaternion _upRotation = Quaternion.Euler(90f, 0f, 0f);
    private Quaternion _rightRotation = Quaternion.Euler(90f, 90f, 0f);
    private Quaternion _downRotation = Quaternion.Euler(90f, 180f, 0f);
    private Quaternion _leftRotation = Quaternion.Euler(90f, 270f, 0f);

    private GameTileContent _content;

    public GameTile NextTileOnPath => _nextOnPath;
    
    public Vector3 ExitPoint { get; private set; }
    public Direction PathDirection { get; private set; }
    public GameTileContent Content
    {
        get => _content;
        set
        {
            if(_content != null)
            {
                _content.Recycle();
            }

            _content = value;
            _content.transform.localPosition = transform.localPosition;
        }
    }
  
    public static void MakeRightLeftNeighbours(GameTile right, GameTile left)
    {
        left._right = right;
        right._left = left;
    }

    public static void MakeUpDownNeighbours(GameTile up, GameTile down)
    {
        up._down = down;
        down._up = up;
    }

    public void ClearPath()
    {
        _distance = int.MaxValue;
        _nextOnPath = null;
    }

    public void BecomeDestination()
    {
        _distance = 0;
        _nextOnPath = null;
        ExitPoint = transform.position;
    }

    public GameTile GrowPathUp() => GrowPathTo(_up, Direction.Down);
    public GameTile GrowPathDown() => GrowPathTo(_down, Direction.Up);
    public GameTile GrowPathRight() => GrowPathTo(_right, Direction.Left);
    public GameTile GrowPathLeft() => GrowPathTo(_left, Direction.Right);

    public void ShowPath()
    {
        if (_distance == 0)
        {
            _arrow.gameObject.SetActive(false);
            return;
        }

        _arrow.gameObject.SetActive(true);
        _arrow.localRotation =
            _nextOnPath == _up ? _upRotation :
            _nextOnPath == _right ? _rightRotation :
            _nextOnPath == _down ? _downRotation :
            _leftRotation;
    }

    private GameTile GrowPathTo(GameTile neighbour, Direction direction)
    {
        if (!HasPath || neighbour == null || neighbour.HasPath)
        {
            return null;
        }

        neighbour._distance = _distance - 1;
        neighbour._nextOnPath = this;
        neighbour.ExitPoint = neighbour.transform.localPosition + direction.GetHalfVector();
        neighbour.PathDirection = direction;
        return neighbour.Content.IsBlockingPath ? null : neighbour;
    }
}