using UnityEngine;

public class FinisherGame : MonoBehaviour
{
    [SerializeField] private int _scoreForWin = 10;
    [SerializeField] private EndPanel _endPanel;
    private void Start()
    {
        Snake.Instance.OnAddTail += Win;
        SnakeMover.Instance.OnCollision += Lose;
        SnakeMover.Instance.OnCollisionObstacle += FastLose;
    }
    private void Win(int countTail)
    {
        if (_scoreForWin <= countTail)
        {
            SnakeMover.Instance.enabled = false;
            _endPanel.DisplayWinPanel();
            _endPanel.gameObject.SetActive(true);
        }
    }
    private void Lose()
    {
        _endPanel.DisplayLosePanel();
        _endPanel.gameObject.SetActive(true);
    }
    private void FastLose() => SceneController.ReloadCurrentScene();
}
