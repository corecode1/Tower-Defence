using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private GameBoard _board;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameTileContentFactory _factory;
    [SerializeField] private WarFactory _warFactory;
    [SerializeField] private GameScenario _scenario;

    private GameBehaviourCollection _enemies = new GameBehaviourCollection();
    private GameBehaviourCollection _nonEnemies = new GameBehaviourCollection();
    private TowerType _currentTowerType;
    private GameScenario.State _actoveScenario;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private static Game _instance;

    private void OnEnable()
    {
        _instance = this;
    }

    private void Start()
    {
        _board.Initialize(_boardSize, _factory);
        _actoveScenario = _scenario.Begin();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentTowerType = TowerType.Laser;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentTowerType = TowerType.Mortar;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }

        _actoveScenario.Progress();

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate(); 
        _nonEnemies.GameUpdate();
    }

    public static void SpawnEnemy(EnemyFactory factory, EnemyType type)
    {
        GameTile spawnPoint = _instance._board.GetSpawnPoint(Random.Range(0, _instance._board.SpawnPointCount));
        Enemy enemy = factory.Get(type);
        enemy.SpawnOn(spawnPoint);
        _instance._enemies.Add(enemy);
    }

    private void HandleTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleTower(tile, _currentTowerType);
            }
            else
            {
                _board.ToggleWall(tile);
            }
        }
    }

    private void HandleAlternativeTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);

        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleDestination(tile);
            }
            else
            {
                _board.ToggleSpawnPoint(tile);
            }
        }
    }

    public static Shell SpawnShell()
    {
        Shell shell = _instance._warFactory.Shell;
        _instance._nonEnemies.Add(shell); 
        return shell;
    }
    
    public static Explosion SpawnExplosion()
    {
        Explosion explosion = _instance._warFactory.Explosion;
        _instance._nonEnemies.Add(explosion); 
        return explosion;
    }
}