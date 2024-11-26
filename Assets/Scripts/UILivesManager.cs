using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILivesManager : MonoBehaviour
{
    [SerializeField] private List<Image> lifeImages; // Lista de imágenes para las vidas
    [SerializeField] private Sprite fullLifeSprite;  // Sprite para una vida llena
    [SerializeField] private Sprite emptyLifeSprite; // Sprite para una vida vacía

    private void Start()
    {
        // Asegúrate de que las imágenes comiencen con todas las vidas llenas
        UpdateLivesDisplay(GameManager.Instance.currentLives);
    }

    public void UpdateLivesDisplay(int lives)
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            if (i < lives)
            {
                lifeImages[i].sprite = fullLifeSprite; // Mostrar vida llena
            }
            else
            {
                lifeImages[i].sprite = emptyLifeSprite; // Mostrar vida vacía
            }
        }
    }
}
