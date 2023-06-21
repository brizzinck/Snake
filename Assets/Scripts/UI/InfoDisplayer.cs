using TMPro;
using UnityEngine;
public class InfoDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private void Start()
    {
        Snake.Instance.OnAddTail += UpdateInfoTextScore;
    }
    private void UpdateInfoTextScore(int score)
    {
        _scoreText.text = "Сегментов: " + score.ToString();
    }
}
