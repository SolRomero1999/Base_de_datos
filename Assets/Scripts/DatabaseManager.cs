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


    async void Start()
    {
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);
        
        index = PlayerPrefs.GetInt("SelectedIndex");
        _selectedTrivia = PlayerPrefs.GetString("SelectedTrivia");

        //print(_selectedTrivia);

        await LoadTriviaData(index);
    }

    async Task LoadTriviaData(int index)
    {
        var response = await clientSupabase
            .From<question>()
            .Where(question => question.trivia_id == index)
            .Select("id, question, correct_answer, trivia_id, trivia(id, category)")
            .Get();

        GameManager.Instance.currentTriviaIndex = index;
        GameManager.Instance.triviaName = response.Models[0].trivia.category;

        GameManager.Instance.responseList = response.Models;
        //print(GameManager.Instance.responseList[0].QuestionText);




        //foreach (var item in response.Models)
        //{
        //    GameManager.Instance._responseList.Add(item);
        //}

        //string questions = response.Models[0].QuestionText;
        //string correct_answer = response.Models[0].CorrectOption;
        //string triviaName = response.Models[0].trivia.category;

        //Debug.Log("Pregunta: " + questions);
        //Debug.Log("Respuesta: " + correct_answer);
        //Debug.Log("Trivia: " + triviaName);


    }

}
