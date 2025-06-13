using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private List<Image> heartImages = new();

    public void SetLives(int currentLives, int maxLives)
    {
        // Wenn Herzanzahl nicht passt
        if (heartImages.Count != maxLives)
        {
            foreach (var heart in heartImages)
            {
                Destroy(heart.gameObject);
            }

            heartImages.Clear();

            for (int i = 0; i < maxLives; i++)
            {
                var heartGO = Instantiate(heartPrefab, transform);
                var img = heartGO.GetComponent<Image>();
                heartImages.Add(img);
            }
        }

        // Aktualisiere Sprite
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].sprite = i < currentLives ? fullHeartSprite : emptyHeartSprite;
        }
    }
}
