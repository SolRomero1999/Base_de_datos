using UnityEngine;
using Supabase;
using Supabase.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Postgrest.Models;

public class DatabaseManager : MonoBehaviour
{
    string supabaseUrl = "https://shauynznsmtuqfhvukvo.supabase.co"; 
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNoYXV5bnpuc210dXFmaHZ1a3ZvIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDYzOTY0NjAsImV4cCI6MjA2MTk3MjQ2MH0.vLyeh7ZjtwsIYV7mWCl7jHL6KBYOpj1wVY4AK9aL-D8"; 

    Supabase.Client clientSupabase;

    public int index;
    async void Start()
    {
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);
        
        index = PlayerPrefs.GetInt("SelectedIndex");


        await LoadTriviaData(index);
    }

    async Task LoadTriviaData(int index)
    {
        var response = await clientSupabase
            .From<question>()
            .Where(question => question.trivia_id == index)
            .Select("id, question, answer1, answer2, answer3, correct_answer, trivia_id, trivia(id, category)")
            .Get();

        GameManager.Instance.currentTriviaIndex = index;

        GameManager.Instance.responseList = response.Models;

        print("Response from query: "+ response.Models.Count);
        print("ResponseList from GM: "+ GameManager.Instance.responseList.Count);

    


    }

}
