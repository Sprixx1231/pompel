using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int TotalLevelCount() => levels.Count;

    [Header("Level Setup")]
    [SerializeField] private List<LevelData> levels = new List<LevelData>();
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Wood wood;
    
    [SerializeField] private Interstitial interstitialAd;

    private LevelData _currentLevel;
    private int _currentLevelIndex = 0;
    private int _currentHits;
    private int _currentLives;
    private bool _isGameOver = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {

        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 0);
        LoadLevel(selectedLevel);
        
    }

    public void LoadLevel(int index)
    {
        print("lade level" + index);
        if (index < 0 || index >= levels.Count)
        {
            Debug.LogWarning("Level index out of range.");
            return;
        }

        _currentLevelIndex = index;
        _currentLevel = levels[_currentLevelIndex];

        ResetLevel();
        ResetUi();

        //load next level
        if (wood != null)
        {
            wood.InitFromLevel(_currentLevel);
        }
    }

    private void ResetUi()
    {
        if (levelText != null)
        {
            levelText.text = $"Level {_currentLevelIndex + 1}";
        }
        
        //TODO: UI
        healthUI.SetLives(_currentLives, _currentLevel.maxLives);
        
    }
    
    private void ResetLevel()
    {
        _isGameOver = false;
        _currentHits = 0;
        _currentLives = _currentLevel.maxLives;

        // Messer vom Holz entfernen
        if (wood != null)
        {
            foreach (Transform child in wood.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    
    public void RegisterHit()
    {
        if (_isGameOver) return;

        _currentHits++;
        if (_currentHits >= levels[_currentLevelIndex].hitsToWin)
        {
            WinLevel();
        }
    }

    public void RegisterMiss()
    {
        if (_isGameOver) return;

        _currentLives--;
        healthUI.SetLives(_currentLives, _currentLevel.maxLives); 
        
        if (_currentLives <= 0)
        {
            LoseLevel();
            LoadLevel(_currentLevelIndex);
        }
    }

    private void WinLevel()
    {
        _isGameOver = true;
        Debug.Log("Level gewonnen!");
        
        if (Random.value < 0.5f) {
            interstitialAd.ShowAd();
        }
        
        int nextLevel = _currentLevelIndex + 1; //
        if (nextLevel > SaveData.GetUnlockedLevel())
        {
            SaveData.SetUnlockedLevel(nextLevel);
        }
        
        GameUIManager.Instance.ShowWinMenu();
        // Lade nächstes Level nach kurzer Pause
        //Invoke(nameof(LoadNextLevel), 1.5f);
    }

    private void LoseLevel()
    {
        _isGameOver = true;
        Debug.Log("Verloren!");
        // TODO: Restart UI zeigen
    }

    public void LoadNextLevel()
    {
        GameUIManager.Instance.HideWinMenu();
        _currentLevelIndex++;

        if (_currentLevelIndex < levels.Count)
        {
            LoadLevel(_currentLevelIndex);
        }
        else
        {
            Debug.Log("Alle Levels abgeschlossen!");
            // TODO: Endscreen oder Neustart
            LoadLevel(0); //von anfang an
        }
    }

    public bool IsGameOver() => _isGameOver;
}

[System.Serializable]
public class LevelData
{
    public int hitsToWin = 5;
    public float targetRotationSpeed = 100f;
    public int maxLives = 3;

    public RotationType rotationType = RotationType.Constant;
    public float directionChangeInterval = 2f;
    public AnimationCurve rotationCurve; 
}

[System.Serializable]
public enum RotationType
{
    Constant,
    PingPong,
    Random,
    DirectionChange,
    EaseInOut,
    CurveDriven
}
