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
        // Recuperamos el puntaje final guardado en PlayerPrefs y lo mostramos en la pantalla
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0); // 0 es el valor por defecto si no se encuentra el puntaje
        _finalScoreText.text = "Puntaje Final: " + finalScore.ToString();

        // Llamar a la función para insertar el puntaje en Supabase
        InsertarPuntaje(finalScore);
    }

    public async void InsertarPuntaje(int finalScore)
    {
        // Obtener el usuario actual (deberías tener un sistema de autenticación)
        // Suponiendo que el ID de usuario se guarda en PlayerPrefs o de alguna manera en tu juego
        int usuarioId = PlayerPrefs.GetInt("UsuarioID", 0);  // Obtener el ID del usuario actual

        // Obtener la categoría actual (esto puede venir de un sistema que mantenga la categoría seleccionada)
        int categoriaId = PlayerPrefs.GetInt("CategoriaID", 0);  // Obtener el ID de la categoría actual

        // Crear el nuevo puntaje
        var nuevoPuntaje = new score
        {
            usuario_id = usuarioId,      // ID del usuario
            categoria_id = categoriaId,  // ID de la categoría de trivia
            puntaje = finalScore         // Puntaje final obtenido
        };

        // Debug: Mostrar los datos que se intentarán insertar
        Debug.Log("Intentando insertar puntaje:");
        Debug.Log("Puntaje: " + finalScore);
        Debug.Log("UsuarioID: " + usuarioId);
        Debug.Log("CategoriaID: " + categoriaId);

        // Crear el cliente de Supabase
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);

        // Insertar el puntaje en la tabla 'score'
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
            Debug.LogError("Error al insertar puntaje");
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

