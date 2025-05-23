using UnityEngine;
using Supabase;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class TriviaSelectionWithButtons : MonoBehaviour
{
    string supabaseUrl = "https://shauynznsmtuqfhvukvo.supabase.co"; 
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNoYXV5bnpuc210dXFmaHZ1a3ZvIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDYzOTY0NjAsImV4cCI6MjA2MTk3MjQ2MH0.vLyeh7ZjtwsIYV7mWCl7jHL6KBYOpj1wVY4AK9aL-D8"; 

    Supabase.Client clientSupabase;
    List<trivia> trivias = new List<trivia>();

    [SerializeField] private List<TMP_Text> buttonLabels; // Textos de los botones
    [SerializeField] private List<UnityEngine.UI.Button> categoryButtons; // Referencia a los 6 botones

    // Referencias estáticas para acceder desde otros scripts
    public static string selectedCategory;  // Categoria seleccionada
    public static int selectedTriviaId;    // Id de la trivia seleccionada

    async void Start()
    {
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);

        await SelectTrivias();
        AssignCategoriesToButtons();
    }

    async Task SelectTrivias()
    {
        var response = await clientSupabase
            .From<trivia>()
            .Select("*")
            .Get();

        if (response != null)
        {
            trivias = response.Models;
        }
    }

    void AssignCategoriesToButtons()
    {
        // Asegúrate de que haya exactamente 6 categorías
        if (trivias.Count < 6)
        {
            Debug.LogError("No hay suficientes categorías en la base de datos.");
            return;
        }

        for (int i = 0; i < 6; i++)
        {
            string category = trivias[i].category;
            buttonLabels[i].text = category;

            int triviaId = trivias[i].id;
            categoryButtons[i].onClick.AddListener(() => OnCategoryButtonClicked(category, triviaId));
        }
    }

    void OnCategoryButtonClicked(string category, int triviaId)
    {
        // Asignar las referencias estáticas
        selectedCategory = category;
        selectedTriviaId = triviaId;

        PlayerPrefs.SetInt("SelectedIndex", triviaId);
        PlayerPrefs.SetString("SelectedTrivia", category);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadRankingaScene()
    {
        SceneManager.LoadScene("Ranking");
    }
}
