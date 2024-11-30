using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIEnd : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _finalScoreText; // Referencia al TextMeshPro para mostrar el puntaje

    string supabaseUrl = "https://kdeuepqvsbzorvtzlvtm.supabase.co"; // URL de tu Supabase
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImtkZXVlcHF2c2J6b3J2dHpsdnRtIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzIxODk1ODcsImV4cCI6MjA0Nzc2NTU4N30.uP62sNgRm1iiu_XzTmph71woKcZZURxOrxNdtC435no"; // Llave de tu Supabase

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
