using UnityEngine;

public static class SaveData
{
    private const string UnlockedKey = "UnlockedLevels";

    public static int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(UnlockedKey, 1); // Default: only level 1 unlocked
    }

    public static void SetUnlockedLevel(int level)
    {
        PlayerPrefs.SetInt(UnlockedKey, level);
    }
}