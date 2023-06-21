using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Snake : MonoBehaviour
{
    public UnityAction<int> OnAddTail;
    public Vector2Int Coordinate => new Vector2Int((int)transform.position.x, (int)transform.position.y);
    public List<TailSnake> TailsSnake => _tailsSnake;
    public Transform HeadSnake => _headSnake;
    public static Snake Instance { get; private set; }

    [SerializeField] private Transform _headSnake;
    [SerializeField] private TailSnake _baseTail;
    [SerializeField] private List<TailSnake> _tailsSnake = new List<TailSnake>();

    private void Awake()
    {
        Instance = this;
    }
    public void AddTail()
    {
        TailSnake tail = Instantiate(_baseTail, -_tailsSnake[^1].transform.forward, Quaternion.identity, transform);
        _tailsSnake.Add(tail);
        Map.Instance.SetNewCell(tail.gameObject, tail.transform.position);
        OnAddTail?.Invoke(_tailsSnake.Count);
    }
}
