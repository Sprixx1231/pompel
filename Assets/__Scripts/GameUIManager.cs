using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }
    
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject winUI;
    
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HidePauseMenu();
        HideWinMenu();
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
    }
    
    public void HidePauseMenu()
    {
        
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void HideWinMenu()
    {
        Time.timeScale = 1f;
        winUI.SetActive(false);
    }
    
    public void ShowWinMenu()
    {
        Time.timeScale = 0f;
        winUI.SetActive(true);
    }
    

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
}
