using Michsky.MUIP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsUI : MonoBehaviour
{
    [SerializeField] private ButtonManager restartButton;
    [SerializeField] private ButtonManager quitButton;
    
    private void Start()
    {
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
