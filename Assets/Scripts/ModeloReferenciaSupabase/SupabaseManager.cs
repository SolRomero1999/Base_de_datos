using UnityEngine;
using Supabase;
using Supabase.Interfaces;
using System.Threading.Tasks;
using Postgrest.Models;
using TMPro;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;

public class SupabaseManager : MonoBehaviour
{
    [Header("Campos de Interfaz")]
    [SerializeField] TMP_InputField _userIDInput;
    [SerializeField] TMP_InputField _userPassInput;
    [SerializeField] TextMeshProUGUI _stateText;

    string supabaseUrl = "https://shauynznsmtuqfhvukvo.supabase.co"; 
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNoYXV5bnpuc210dXFmaHZ1a3ZvIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDYzOTY0NjAsImV4cCI6MjA2MTk3MjQ2MH0.vLyeh7ZjtwsIYV7mWCl7jHL6KBYOpj1wVY4AK9aL-D8"; 

    Supabase.Client clientSupabase;

    private usuarios _usuarios = new usuarios();

    public static int currentUserId; 

    public async void UserLogin()
    {
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);

        var all_users = await clientSupabase
            .From<usuarios>()
            .Select("*")
            .Get();

        // Imprime todos los usuarios para verificar si la consulta devuelve algo
        Debug.Log("All users: " + all_users.Content);

        // Filtra según los datos de login
        var login_password = await clientSupabase
            .From<usuarios>()
            .Select("password, id")
            .Where(usuarios => usuarios.username == _userIDInput.text)
            .Get();

        // Verifica la respuesta y muestra el contenido
        if (login_password != null && login_password.Models.Count > 0)
        {
            Debug.Log("User found: " + login_password.Models[0].username);  // Mostrar el username encontrado en la base de datos

            if (login_password.Models[0].password.Equals(_userPassInput.text))
            {
                print("LOGIN SUCCESSFUL");
                _stateText.text = "LOGIN SUCCESSFUL";
                _stateText.color = Color.green;

                // Guardar el id del usuario de manera global
                currentUserId = login_password.Models[0].id;

                // Cargar la siguiente escena después de un login exitoso
                SceneManager.LoadScene("TriviaSelectScene");
            }
            else
            {
                print("WRONG PASSWORD");
                _stateText.text = "WRONG PASSWORD";
                _stateText.color = Color.red;
            }
        }
        else
        {
            print("No user found or invalid response.");
            _stateText.text = "No user found or invalid response.";
            _stateText.color = Color.red;
        }
    }

    public async void InsertarNuevoUsuario()
    {
        // Inicializa el cliente Supabase
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);

        // Consultar el último id utilizado (ID = index)
        var ultimoId = await clientSupabase
            .From<usuarios>()
            .Select("id")
            .Order(usuarios => usuarios.id, Postgrest.Constants.Ordering.Descending) // Ordenar en orden descendente para obtener el último id
            .Get();

        int nuevoId = 1; // Valor predeterminado si la tabla está vacía

        if (ultimoId != null)
        {
            nuevoId = ultimoId.Model.id + 1; // Incrementar el último id
        }

        // Crear el nuevo usuario con el nuevo id
        var nuevoUsuario = new usuarios
        {
            id = nuevoId,
            username = _userIDInput.text,
            age = Random.Range(0, 100), // Luego creo el campo que falta en la UI
            password = _userPassInput.text,
        };

        // Insertar el nuevo usuario
        var resultado = await clientSupabase
            .From<usuarios>()
            .Insert(new[] { nuevoUsuario });

        // Verifico el estado de la inserción 
        if (resultado.ResponseMessage.IsSuccessStatusCode)
        {
            _stateText.text = "Usuario Correctamente Ingresado";
            _stateText.color = Color.green;
        }
        else
        {
            _stateText.text = "Error en el registro de usuario";
            _stateText.text = resultado.ResponseMessage.ToString();
            _stateText.color = Color.red;
        }
    }
}
