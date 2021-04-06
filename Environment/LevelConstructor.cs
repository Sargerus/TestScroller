using UnityEngine;
using System.Collections.Generic;

public class LevelConstructor
{
    private readonly int _wallCountInHeight;
    private readonly int _wallCountInWidth;
    private const float _TOPSPAWNCHANCE = 0.8f;
    private const float _POWERUPSPAWNCHANCE = 0.5f;

    private int _lastFreePosition;
    private readonly Pool _pool;

    private bool _startLevelWasGenerated = false;
    private enum Direction
    {
        LEFT = -1,
        TOP = 0,
        RIGHT = 1
    }

    private readonly Vector3 _scaleVector;
    private Vector3 _startSpawningPosition;
    private Transform _lastInLastSpawnedRow;
    private GameObject _powerUpHolder;
    private PowerUpStorage _powerUpStorage;

    public LevelConstructor(int heightCount, int widthCount)
    {
        _wallCountInHeight = heightCount;
        _wallCountInWidth = widthCount;

        _pool = Pool.GetInstance();
        _powerUpHolder = (GameObject)Resources.Load("Prefabs/PowerUpHolder");
        _powerUpStorage = new PowerUpStorage();

        Vector3 _cameraTopLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 10f));
        Vector3 _cameraBottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10f));

        _scaleVector = new Vector3((_cameraBottomRight.x - _cameraTopLeft.x) / _wallCountInWidth, (_cameraTopLeft.y - _cameraBottomRight.y) / _wallCountInHeight, 1);

        _lastFreePosition = (_wallCountInWidth - 1) / 2;

        _startSpawningPosition = new Vector3(_cameraTopLeft.x + _scaleVector.x / 2, _cameraTopLeft.y + _scaleVector.y / 2, 10f);
    }

    public void GenerateStartLevel()
    {
        if(!_startLevelWasGenerated)
            for (int i = 0; i < _wallCountInHeight + 2; i++)
                GenerateLine();

        _startLevelWasGenerated = true;
    }

    public void GenerateLine()
    {
        GameObject wall = null;

        if (_lastInLastSpawnedRow)
            _startSpawningPosition.y = _lastInLastSpawnedRow.position.y + _scaleVector.y;

        List<int> way = DigWay();

        for (int i = 0; i < _wallCountInWidth; i++)
        {
            if (way.Contains(i)) continue;

            wall = _pool.GetFromPool();
            wall.transform.localScale = _scaleVector;
            wall.transform.position = new Vector3(_startSpawningPosition.x + i * _scaleVector.x, _startSpawningPosition.y, 10);
        }

        if (Random.value <= _POWERUPSPAWNCHANCE)
        {
            GameObject ph = GameObject.Instantiate(_powerUpHolder, new Vector3(_startSpawningPosition.x + _scaleVector.x * way[way.Count-1], _startSpawningPosition.y, 10), Quaternion.identity);
            ph.transform.localScale = new Vector3(_scaleVector.x / 3, _scaleVector.y / 6, _scaleVector.z);
            ph.AddComponent(_powerUpStorage.GetRandomPowerUp());
            ph.GetComponent<PowerUp>().enabled = false;
        }
            
        wall.AddComponent<Wall>();
        _lastInLastSpawnedRow = wall.transform;
    }

    private List<int> DigWay()
    {
        List<int> way = new List<int>();
        Direction lastDigDirection = Direction.TOP;
        Random.InitState(System.DateTime.Now.Millisecond);

        do
        {
            way.Add(_lastFreePosition += (int)lastDigDirection);
            Dig(ref lastDigDirection);

        } while (lastDigDirection != Direction.TOP);

        return way;
    }

    private void Dig(ref Direction lastDirection)
    {
        if (_lastFreePosition == 0)
        {

            lastDirection = (lastDirection == Direction.LEFT || Random.value <= _TOPSPAWNCHANCE) ? Direction.TOP : Direction.RIGHT;

            return;
        }
                  
        if(_lastFreePosition == _wallCountInWidth - 1)
        {

            lastDirection = (lastDirection == Direction.RIGHT || Random.value <= _TOPSPAWNCHANCE) ? Direction.TOP : Direction.LEFT;

            return;
        }
            
        if (lastDirection == Direction.TOP)
        {
            lastDirection = (Direction)Random.Range(-1, 2);
            return;
        }
            
        lastDirection = Random.value <= _TOPSPAWNCHANCE ? Direction.TOP : lastDirection;
    }

    public Vector3 GetScaleVector() => _scaleVector;
}
