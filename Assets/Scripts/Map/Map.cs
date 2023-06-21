using Extension;
using UnityEngine;

public class Cell
{
    public Vector2Int Coordinate { get; }
    public GameObject ObjectOnCell { get; set; }

    public Cell(Vector2Int coordinate, GameObject objectOnCell)
    {
        Coordinate = coordinate;
        ObjectOnCell = objectOnCell;
    }
}

public class Map : MonoBehaviour
{
    public static Map Instance { get; private set; }

    [SerializeField] private Obstacle _border;
    [SerializeField] private Vector2Int _borderCount;
    [SerializeField] private Obstacle _obstacle;
    [SerializeField] private Vector2Int _obstacleCount;
    [SerializeField] private int _indentBetweenObstacle = 2;
    [SerializeField] private Fruit _fruit;

    private Cell[,] _cells;
    public void MoveFruitToNewCell(int distance = 0)
    {
        Vector2Int coordinate = GetNewCellCoordinate(distance);
        _fruit.transform.position = coordinate.Vector2IntToVectro3();
        SetNewCell(_fruit.gameObject, coordinate);
    }
    public void SetNewCell(GameObject gameObject, Vector3 coordinate)
        => SetNewCell(gameObject, coordinate.Vector3ToVectro2Int());
    public void SetNewCell(GameObject gameObject, Vector2Int coordinate)
    {
        Vector2Int index = GetCorrectedIndex(coordinate);
        if (_cells[index.x, index.y].ObjectOnCell != null)
            if (_cells[index.x, index.y].ObjectOnCell.transform.position != gameObject.transform.position)
                return; 
        _cells[index.x, index.y] = new Cell(coordinate, gameObject);
    }

    public GameObject GetObjectFromCell(Vector2Int coordinate)
    {
        Cell cell = GetFullCellWithCoordinate(coordinate);
        if (cell == null)
            return null;
        return cell.ObjectOnCell;
    }

    public void ClearCell(Vector2Int coordinate)
    {
        Vector2Int index = GetCorrectedIndex(coordinate);
        _cells[index.x, index.y].ObjectOnCell = null;
    }

    private void Awake()
    {
        Instance = this;
        InitializeMap();
    }
    private void Start()
    {
        InitializeAllProps();
    }

    private void InitializeAllProps()
    {
        InitializeBorders();
        SetNewCell(Snake.Instance.HeadSnake.gameObject, Snake.Instance.HeadSnake.position);
        SetNewCell(Snake.Instance.TailsSnake[^1].gameObject, Snake.Instance.TailsSnake[^1].transform.position);
        SpawnObstacles();
        InitializeFruit();
   }
    private void InitializeMap()
    {
        _cells = new Cell[_borderCount.x + 1, _borderCount.y + 1];
        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            for (int y = 0; y < _cells.GetLength(1); y++)
            {
                _cells[x, y] = new Cell(new Vector2Int(x, y), null);
            }
        }
    }
    private void InitializeBorders()
    {
        int width = _cells.GetLength(0);
        int height = _cells.GetLength(1);

        for (int x = -width / 2; x <= width / 2; x++)
        {
            CreateBorder(new Vector3(x, height / 2, 0), new Vector2Int(x, height / 2));
            CreateBorder(new Vector3(x, -height / 2, 0), new Vector2Int(x, -height / 2));
        }

        for (int y = -height / 2; y <= height / 2; y++)
        {
            CreateBorder(new Vector3(width / 2, y, 0), new Vector2Int(width / 2, y));
            CreateBorder(new Vector3(-width / 2, y, 0), new Vector2Int(-width / 2, y));
        }
    }
    private void CreateBorder(Vector3 position, Vector2Int cellCoordinates)
    {
        Obstacle border = Instantiate(_border, position, Quaternion.identity);
        SetNewCell(border.gameObject, cellCoordinates);
    }
    private void SpawnObstacles()
    {
        int countObstacles = Random.Range(_obstacleCount.x, _obstacleCount.y);
        for (int i = 0; i < countObstacles; i++)
        {
            Vector2Int coordinate = GetNewCellCoordinate(_indentBetweenObstacle);
            Obstacle obstacle = Instantiate(_obstacle, coordinate.Vector2IntToVectro3(), Quaternion.identity);
            SetNewCell(obstacle.gameObject, coordinate);
        }
    }
    private void InitializeFruit()
    {
        _fruit = Instantiate(_fruit);
        MoveFruitToNewCell(2);
    }

    private Vector2Int GetNewCellCoordinate(int distance)
    {
        int x = 0;
        int y = 0;
        GetFreeCell(ref x, ref y, distance);
        return new Vector2Int(x, y);
    }
    private bool IsCellAvailable(Vector2Int coordinate, int distance = 1)
    {
        int minX = coordinate.x - distance;
        int maxX = coordinate.x + distance;
        int minY = coordinate.y - distance;
        int maxY = coordinate.y + distance;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector2Int currentCoordinate = new Vector2Int(x, y);
                if (GetObjectFromCell(currentCoordinate) != null)
                    return false;      
            }
        }
        return true;
    }
    private Vector2Int GetFreeCell(ref int x, ref int y, int distance)
    {
        Vector2Int index;
        do
        {
            GetRandomIncorrectIndexCell(ref x, ref y);
            index = GetCorrectedIndex(new Vector2Int(x, y));
        }
        while (!IsCellAvailable(new Vector2Int(x, y), distance));
        return index;
    }
    private Cell GetFullCellWithCoordinate(Vector2Int coordinate)
    {
        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            for (int y = 0; y < _cells.GetLength(1); y++)
            {
                Cell cell = _cells[x, y];
                if (cell != null && cell.Coordinate == coordinate && cell.ObjectOnCell != null)
                    return cell;
            }
        }
        return null;
    }

    private Vector2Int GetCorrectedIndex(Vector2Int coordinate)
    {
        Vector2Int index = Vector2Int.zero;
        index.x = Mathf.Clamp((_cells.GetLength(0) - 1) / 2 + coordinate.x, 0, _cells.GetLength(0) - 1);
        index.y = Mathf.Clamp((_cells.GetLength(1) - 1) / 2 + coordinate.y, 0, _cells.GetLength(1) - 1);
        return index;
    }
    private void GetRandomIncorrectIndexCell(ref int x, ref int y)
    {
        x = Random.Range(-_cells.GetLength(0) / 2 + 1, _cells.GetLength(0) / 2 - 1);
        y = Random.Range(-_cells.GetLength(1) / 2 + 1, _cells.GetLength(1) / 2 - 1);
    }
}
