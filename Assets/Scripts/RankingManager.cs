using UnityEngine;
using Supabase;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class Ranking : MonoBehaviour
{
    string supabaseUrl = "https://shauynznsmtuqfhvukvo.supabase.co"; 
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNoYXV5bnpuc210dXFmaHZ1a3ZvIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDYzOTY0NjAsImV4cCI6MjA2MTk3MjQ2MH0.vLyeh7ZjtwsIYV7mWCl7jHL6KBYOpj1wVY4AK9aL-D8"; 

    Supabase.Client clientSupabase;

    List<trivia> trivias = new List<trivia>();
    List<score> attempts = new List<score>();
    List<usuarios> users = new List<usuarios>();

    [Header("General Ranking References")]
    [SerializeField] private List<TMP_Text> generalUserTexts;  // Textos para los nombres de usuarios generales
    [SerializeField] private List<TMP_Text> generalScoreTexts; // Textos para los puntajes generales

    [Header("Category Ranking References")]
    [SerializeField] private List<TMP_Text> categoryUserTexts;  // Textos para los nombres de usuarios por categoría
    [SerializeField] private List<TMP_Text> categoryScoreTexts; // Textos para los puntajes por categoría

    [Header("Category Buttons")]
    [SerializeField] private List<TMP_Text> buttonLabels;  // Textos de los botones
    [SerializeField] private List<UnityEngine.UI.Button> categoryButtons;  // Referencias a los botones

    [Header("Category Panel")]
    [SerializeField] private GameObject categoryPanel; // Panel que cambiará de color
    [SerializeField] private TMP_Text panelText;       // Texto del panel que cambiará de color

    public static int SelectedTriviaId { get; private set; }
    public static string SelectedCategory { get; private set; }

    private Dictionary<string, string> categoryHexColors = new Dictionary<string, string>
    {
        { "Arte", "#E40025" },         
        { "Ciencia", "#56CB63" },     
        { "Deportes", "#F38C01" },    
        { "Historia", "#F5DB24" },    
        { "Geografía", "#3574D3" },   
        { "Entretenimiento", "#E440B0" } 
    };

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

        UpdatePanelAppearance(category);
        ShowCategory(category);
    }

    void UpdatePanelAppearance(string category)
    {
        // Verificar si la categoría tiene un color hexadecimal definido
        if (categoryHexColors.TryGetValue(category, out string hexColor))
        {
            if (ColorUtility.TryParseHtmlString(hexColor, out Color panelColor))
            {
                // Cambiar el color del panel
                categoryPanel.GetComponent<UnityEngine.UI.Image>().color = panelColor;

                // Cambiar el texto del panel
                panelText.text = category;
                panelText.color = Color.white; // Mantener el texto blanco
            }
            else
            {
                Debug.LogWarning($"No se pudo convertir el color hexadecimal a Color: {hexColor}");
            }
        }
        else
        {
            Debug.LogWarning($"Categoría no encontrada en el diccionario de colores: {category}");
        }
    }

    void ShowGeneral()
    {
        var sortedUsers = attempts.OrderByDescending(x => x.puntaje).Take(3).ToList();  // Solo los 3 mejores

        for (int i = 0; i < generalUserTexts.Count; i++)
        {
            if (i < sortedUsers.Count)
            {
                var user = users.FirstOrDefault(u => u.id == sortedUsers[i].usuario_id);
                if (user != null)
                {
                    generalUserTexts[i].text = user.username;         // Asignar nombre de usuario
                    generalScoreTexts[i].text = sortedUsers[i].puntaje.ToString(); // Asignar puntaje
                }
            }
            else
            {
                generalUserTexts[i].text = "";  // Limpiar texto si no hay más usuarios
                generalScoreTexts[i].text = "";
            }
        }
    }

    void ShowCategory(string category)
    {
        var selectedCategory = trivias.FirstOrDefault(t => t.category == category);

        if (selectedCategory != null)
        {
            var categoryUsers = attempts.Where(x => x.categoria_id == selectedCategory.id)
                .OrderByDescending(x => x.puntaje)
                .Take(3)
                .ToList();  // Solo los 3 mejores

            for (int i = 0; i < categoryUserTexts.Count; i++)
            {
                if (i < categoryUsers.Count)
                {
                    var user = users.FirstOrDefault(u => u.id == categoryUsers[i].usuario_id);
                    if (user != null)
                    {
                        categoryUserTexts[i].text = user.username;         // Asignar nombre de usuario
                        categoryScoreTexts[i].text = categoryUsers[i].puntaje.ToString(); // Asignar puntaje
                    }
                }
                else
                {
                    categoryUserTexts[i].text = "";  // Limpiar texto si no hay más usuarios
                    categoryScoreTexts[i].text = "";
                }
            }
        }
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("TriviaSelectScene");
    }
}