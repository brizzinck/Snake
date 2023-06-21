using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneController : MonoBehaviour
{
    [SerializeField] private Button _reloadButton;
    public static void ReloadCurrentScene() =>
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    private void Start() => _reloadButton.onClick.AddListener(ReloadCurrentScene);
}
