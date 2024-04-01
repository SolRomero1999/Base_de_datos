using UnityEngine;
using Supabase;
using Supabase.Interfaces;
using System.Threading;
using Postgrest.Models;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class SupabaseManager : MonoBehaviour

{
    //[Header("Login")]
    //public string user_id = string.Empty;
    //public string user_pass = string.Empty;

    [Header("Agrega Nuevo Usuario")]
    public string new_user_id = string.Empty;
    public string new_user_pass = string.Empty;
    public int new_user_age = 0;

    [Header("Campos de Interfaz")]
    [SerializeField] TMP_InputField _userIDInput;
    [SerializeField] TMP_InputField _userPassInput;

    string supabaseUrl = "https://zkyndewjwgeydidqerqx.supabase.co";
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InpreW5kZXdqd2dleWRpZHFlcnF4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MTEwMzM1MjQsImV4cCI6MjAyNjYwOTUyNH0.Mq1PucFgRp2Vyxv3tFZs709iSRmj2jV8oXvlzLmGBP0";

    Supabase.Client clientSupabase;

    private usuarios _usuarios = new usuarios();

    async void Start()
    {
        // UserLogin();

    }

    public async void UserLogin()
    {
        // Initialize the Supabase client
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);

        // prueba
        var test_response = await clientSupabase
            .From<usuarios>()
            .Select("*")
            .Get();
        Debug.Log(test_response.Content);



        // filtro según datos de login
        var login_password = await clientSupabase
          .From<usuarios>()
          .Select("password")
          .Where(usuarios => usuarios.username == _userIDInput.text)
          .Get();


        if (login_password.Model.password.Equals(_userPassInput.text))
        {
            print("LOGIN SUCCESSFUL");
        }
        else
        {
            print("WRONG PASSWORD");
        }
    }

    public async void InsertarNuevoUsuario()
    {
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
            //PARA HACERLO DESDE EL INSPECTOR
            //id = nuevoId,
            //username = new_user_id,
            //age = new_user_age,
            //password = new_user_pass

            //PARA HACERLO DESDE LA UI
            id = nuevoId,
            username = _userIDInput.text,
            age = Random.Range(0, 100), //luego creo el campo que falta en la UI
            password = _userPassInput.text,
        };


        // Insertar el nuevo usuario
        var resultado = await clientSupabase
            .From<usuarios>()
            .Insert(new[] { nuevoUsuario });

        //// Verificar si la inserción fue exitosa
        //if (resultado.Error == null)
        //{
        //    Debug.Log("Usuario agregado exitosamente");
        //}
        //else
        //{
        //    Debug.LogError("Error al agregar usuario: " + resultado.Error.Message);
        //}
    }
}

