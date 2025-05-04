using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIEnd : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _finalScoreText; // Referencia al TextMeshPro para mostrar el puntaje

    string supabaseUrl = "https://shauynznsmtuqfhvukvo.supabase.co"; 
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNoYXV5bnpuc210dXFmaHZ1a3ZvIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDYzOTY0NjAsImV4cCI6MjA2MTk3MjQ2MH0.vLyeh7ZjtwsIYV7mWCl7jHL6KBYOpj1wVY4AK9aL-D8"; 

    Supabase.Client clientSupabase;

    void Start()
    {
        // Inicializa el cliente Supabase solo una vez
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);
        
        // Recuperamos el puntaje final real usando GetCurrentScore() de UIManagment
        int finalScore = UIManagment.Instance.GetCurrentScore();  // Obtiene el puntaje actual
        _finalScoreText.text = "Puntaje Final: " + finalScore.ToString();

        // Llamar a la función para insertar el puntaje en Supabase
        InsertarPuntaje(finalScore);
    }

    public async void InsertarPuntaje(int finalScore)
    {
        // Inicializa el cliente Supabase
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);

        // Consulta el último ID de la tabla 'score'
        var ultimoIdResultado = await clientSupabase
            .From<score>()
            .Select("id")
            .Order(score => score.id, Postgrest.Constants.Ordering.Descending)
            .Get();

        int nuevoId = 1; // Valor predeterminado si la tabla está vacía

        if (ultimoIdResultado.Models.Count > 0)
        {
            // Obtén el último ID y calcula el nuevo
            nuevoId = ultimoIdResultado.Models[0].id + 1;
        }

        // Crear el nuevo puntaje con el ID calculado
        var nuevoPuntaje = new score
        {
            id = nuevoId,
            usuario_id = SupabaseManager.currentUserId, // Usamos la variable estática del SupabaseManager
            categoria_id = TriviaSelectionWithButtons.selectedTriviaId, // ID de la categoría de trivia
            puntaje = finalScore // Puntaje final obtenido
        };

        // Insertar el nuevo puntaje
        var resultado = await clientSupabase
            .From<score>()
            .Insert(new[] { nuevoPuntaje });

        // Verificar si la inserción fue exitosa
        if (resultado.ResponseMessage.IsSuccessStatusCode)
        {
            Debug.Log("Puntaje insertado correctamente");
        }
        else
        {
            Debug.LogError("Error al insertar puntaje: " + resultado.ResponseMessage.Content);
        }
    }


    public void LoadTriviaScene()
    {
        // Destruir las instancias solo si aún no han sido destruidas
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        if (UIManagment.Instance != null)
        {
            Destroy(UIManagment.Instance.gameObject);
        }

        // Cargar la escena de selección de trivia
        SceneManager.LoadScene("TriviaSelectScene");
    }
}
