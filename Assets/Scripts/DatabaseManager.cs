using UnityEngine;
using Supabase;
using Supabase.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Postgrest.Models;

public class DatabaseManager : MonoBehaviour
{
    string supabaseUrl = "https://zkyndewjwgeydidqerqx.supabase.co";
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InpreW5kZXdqd2dleWRpZHFlcnF4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MTEwMzM1MjQsImV4cCI6MjAyNjYwOTUyNH0.Mq1PucFgRp2Vyxv3tFZs709iSRmj2jV8oXvlzLmGBP0";

    Supabase.Client clientSupabase;

    public int index;
    private string _selectedTrivia;

    //UI
    [SerializeField] private 

    /// <summary>
    /// //////
    /// </summary>
    //public static DatabaseManager Instance { get; private set; }



    //void Awake()
    //{
    //    // Configura la instancia
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject); // Opcional, para mantener el objeto entre escenas
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    /// <summary>
    /// /////////
    /// </summary>

    async void Start()
    {
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);
        
        index = PlayerPrefs.GetInt("SelectedIndex");
        _selectedTrivia = PlayerPrefs.GetString("SelectedTrivia");

        print(_selectedTrivia);

        await LoadData(index);
    }

    async Task LoadData(int index)
    {
        var response = await clientSupabase
            .From<question>()
            .Where(question => question.trivia_id == index)
            .Select("id, question, correct_answer, trivia_id, trivia(id, category)")
            .Get();

        string questions = response.Models[0].QuestionText;
        string correct_answer = response.Models[0].CorrectOption;
        string triviaName = response.Models[0].trivia.category;

        Debug.Log("Pregunta: " + questions);
        Debug.Log("Respuesta: " + correct_answer);
        Debug.Log("Trivia: " + triviaName);


    }
}
