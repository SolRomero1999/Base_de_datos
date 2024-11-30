using UnityEngine;
using Supabase;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class Ranking : MonoBehaviour
{
    string supabaseUrl = "https://kdeuepqvsbzorvtzlvtm.supabase.co"; // URL de tu Supabase
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImtkZXVlcHF2c2J6b3J2dHpsdnRtIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzIxODk1ODcsImV4cCI6MjA0Nzc2NTU4N30.uP62sNgRm1iiu_XzTmph71woKcZZURxOrxNdtC435no"; // Llave de tu Supabase

    Supabase.Client clientSupabase;

    List<trivia> trivias = new List<trivia>();
    List<score> attempts = new List<score>();
    List<usuarios> users = new List<usuarios>();

    [SerializeField] private List<TMP_Text> buttonLabels;  // Textos de los botones
    [SerializeField] private List<UnityEngine.UI.Button> categoryButtons;  // Referencias a los botones
    [SerializeField] TMP_Text generalRanking;  // Referencia al texto de ranking general
    [SerializeField] TMP_Text categoryRanking;  // Referencia al texto de ranking por categoría

    public static int SelectedTriviaId { get; private set; }
    public static string SelectedCategory { get; private set; }

    async void Start()
    {
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);

        await SelectTrivias();
        AssignCategoriesToButtons();

        await LoadAttemptData();
        await LoadUser();

        ShowGeneral();
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

    async Task LoadAttemptData()
    {
        var response = await clientSupabase
            .From<score>()
            .Select("*")
            .Get();

        if (response != null)
        {
            attempts = response.Models;
        }
    }

    async Task LoadUser()
    {
        var response = await clientSupabase
            .From<usuarios>()
            .Select("*")
            .Get();

        if (response != null)
        {
            users = response.Models;
        }
    }

    void AssignCategoriesToButtons()
    {
        if (trivias.Count < 6)
        {
            Debug.LogError("No hay suficientes categorías en la base de datos.");
            return;
        }

        for (int i = 0; i < categoryButtons.Count; i++)
        {
            if (i < trivias.Count)
            {
                string category = trivias[i].category;
                buttonLabels[i].text = category;

                int buttonIndex = i; // Captura el índice para usarlo en la función
                categoryButtons[i].onClick.AddListener(() => OnCategoryButtonClickedIntermediate(buttonIndex));
            }
            else
            {
                categoryButtons[i].gameObject.SetActive(false); // Oculta botones adicionales
            }
        }
    }

    public void OnCategoryButtonClickedIntermediate(int buttonIndex)
    {
        string category = trivias[buttonIndex].category;
        int triviaId = trivias[buttonIndex].id;

        OnCategoryButtonClicked(category, triviaId);
    }

    void OnCategoryButtonClicked(string category, int triviaId)
    {
        SelectedCategory = category;
        SelectedTriviaId = triviaId;

        ShowCategory(category);
    }

    void ShowGeneral()
    {
        var sortedUsers = attempts.OrderByDescending(x => x.puntaje).Take(3);  // Solo los 3 mejores

        string generalRankingText = "  USUARIO           PUNTAJE\n";
        foreach (var intento in sortedUsers)
        {
            var user = users.FirstOrDefault(u => u.id == intento.usuario_id);
            if (user != null)
            {
                generalRankingText += $"{user.username,-24} {intento.puntaje}\n";
            }
        }

        generalRanking.text = generalRankingText;
    }

    void ShowCategory(string category)
    {
        var selectedCategory = trivias.FirstOrDefault(t => t.category == category);

        if (selectedCategory != null)
        {
            var categoryUsers = attempts.Where(x => x.categoria_id == selectedCategory.id)
                .OrderByDescending(x => x.puntaje)
                .Take(3);  // Solo los 3 mejores

            string categoryRankingText = "  USUARIO           PUNTAJE\n";
            foreach (var intento in categoryUsers)
            {
                var user = users.FirstOrDefault(u => u.id == intento.usuario_id);
                if (user != null)
                {
                    categoryRankingText += $"{user.username,-24} {intento.puntaje}\n";
                }
            }

            categoryRanking.text = categoryRankingText;
        }
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("TriviaSelectScene");
    }
}

