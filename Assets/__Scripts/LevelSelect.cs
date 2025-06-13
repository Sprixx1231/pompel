using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private int totalLevels = 10; // manually set in inspector

    private void Start()
    {
        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
      int unlocked = SaveData.GetUnlockedLevel();

        for (int i = 1; i <= totalLevels; i++)
        {
            GameObject buttonGo = Instantiate(buttonPrefab, transform);
            Button button = buttonGo.GetComponent<Button>();
            var buttonText = buttonGo.GetComponentInChildren<TMP_Text>();
            buttonText.text = $"{i}";

            if (i <= unlocked)
            {
                int levelIndex = i; 
                button.onClick.AddListener(() => { OnLevelSelected(levelIndex);});
            }
            else
            {
                // Disable button and make it greyed out / move to seperate func maybe
                button.interactable = false;
                buttonText.color = Color.gray;
            }
        }
    }

    private void OnLevelSelected(int levelIndex)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelIndex - 1);   // -1 weil levels bei 0 anfangen
        SceneManager.LoadScene("Game");
    }
}
