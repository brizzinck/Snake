using Extension;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SnakeMover : MonoBehaviour
{
    public static SnakeMover Instance => _instance;
    public UnityAction OnCollision;
    public UnityAction OnCollisionObstacle;
    [SerializeField] private Button _toLeft;
    [SerializeField] private Button _toLRight;
    private float _delayMove = 0.5f;
    private float _curentTimeDelay;
    private Vector2Int _direction = Vector2Int.zero;
    private static SnakeMover _instance;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _toLeft.onClick.AddListener(ToLeftDirection);
        _toLRight.onClick.AddListener(ToRightDirection);
    }
    private void Update()
    {
        ChangeDirection();
        _curentTimeDelay += Time.deltaTime;
        if (_curentTimeDelay > _delayMove)
        {
            MoveSnake();
            _curentTimeDelay = 0;
        }
    }
    private void MoveSnake()
    {
        ViewNextCell();
        Vector3 lastDirection = Snake.Instance.HeadSnake.position;
        Snake.Instance.HeadSnake.position += _direction.Vector2IntToVectro3();
        Map.Instance.SetNewCell(Snake.Instance.HeadSnake.gameObject, Snake.Instance.HeadSnake.position.Vector3ToVectro2Int());
        for (int i = 0; i < Snake.Instance.TailsSnake.Count; i++)
        {
            Vector3 temp = Snake.Instance.TailsSnake[i].transform.position;
            Snake.Instance.TailsSnake[i].transform.position = lastDirection;
            Map.Instance.SetNewCell(Snake.Instance.TailsSnake[i].gameObject, 
                Snake.Instance.TailsSnake[i].transform.position.Vector3ToVectro2Int());
            lastDirection = temp;
            if (Snake.Instance.TailsSnake[i].transform.position == Snake.Instance.HeadSnake.position)
            {
                OnCollision?.Invoke();
                enabled = false;
                return;
            }
        }
        Map.Instance.ClearCell(lastDirection.Vector3ToVectro2Int());
        ControllBlockInput(true);
    }
    private void ToLeftDirection()
    {
        Snake.Instance.HeadSnake.transform.rotation = Quaternion.Euler(0, 0, Snake.Instance.HeadSnake.transform.eulerAngles.z + 90);
        ChangeDirection();
        ControllBlockInput();
    }
    private void ToRightDirection()
    {
        Snake.Instance.HeadSnake.transform.rotation = Quaternion.Euler(0, 0, Snake.Instance.HeadSnake.transform.eulerAngles.z - 90);
        ChangeDirection();
        ControllBlockInput();
    }
    private void ChangeDirection()
    {
        _direction = Direction.GetDirectionFromEuler(Snake.Instance.Coordinate, 
            (int)Snake.Instance.HeadSnake.transform.eulerAngles.z);
    }
    private void ControllBlockInput(bool interactable = false)
    {
        _toLeft.interactable = interactable;
        _toLRight.interactable = interactable;
    }
    private void ViewNextCell()
    {
        Vector2Int coordinate = Snake.Instance.HeadSnake.position.Vector3ToVectro2Int() + _direction;
        if (Map.Instance.GetObjectFromCell(coordinate) != null)
        {
            if (CheckTypeCollision<Border>(coordinate))
            {
                OnCollision?.Invoke();
                enabled = false;
            }
            else if (CheckTypeCollision<Obstacle>(coordinate))
            {
                OnCollisionObstacle?.Invoke();
            }
            else if (CheckTypeCollision<Fruit>(coordinate))
            {
                Snake.Instance.AddTail();
                Map.Instance.MoveFruitToNewCell();
                if (_delayMove > 0.1f)
                    _delayMove -= 0.025f;
            }
        }
    }
    private bool CheckTypeCollision<T>(Vector2Int coordinate)
    {
        if (Map.Instance.GetObjectFromCell(coordinate).GetComponent<T>() != null)
            return true;
        return false;
    }
}
public static class Direction
{
    public static Vector2Int GetDirectionFromEuler(Vector2Int coordinate, int euler)
    {
        if (euler == 0)
            return new Vector2Int(coordinate.x + 1, coordinate.y);
        if (euler == 90)
            return new Vector2Int(coordinate.x, coordinate.y + 1);
        if (euler == 180)
            return new Vector2Int(coordinate.x - 1, coordinate.y);
        if (euler == 270)
            return new Vector2Int(coordinate.x, coordinate.y - 1);
        return Vector2Int.zero;
    }
}
