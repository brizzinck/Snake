using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private Color _loseColor;
    [SerializeField] private string _loseText;
    [SerializeField] private Color _winColor;
    [SerializeField] private string _winText;
    [SerializeField] private Image _backPanel;
    [SerializeField] private TextMeshProUGUI _infoText;
    public void DisplayLosePanel()
    {
        _backPanel.color = _loseColor;
        _infoText.text = _loseText;
    }
    public void DisplayWinPanel()
    {
        _backPanel.color = _winColor;
        _infoText.text = _winText;
    }
}
