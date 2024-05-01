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

    void Start()
    {
        // Configurar la conexión a Supabase
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);

        // Cargar datos al inicio del juego
        LoadData();
    }

    async void LoadData()
    {
        // Cargar trivias desde la base de datos
        List<trivia> trivias = await LoadTrivias();

        // Pasar datos cargados al GameManager
        //GameManager.InitializeTrivias(trivias);
    }

    async Task<List<trivia>> LoadTrivias()
    {
        var response = await clientSupabase
            .From<trivia>()
            .Select("*")
            .Get();

        if (response != null)
        {
            // La operación se completó correctamente y hay datos disponibles
            List<trivia> trivias = response.Models;

            foreach (var trivia in trivias)
            {
                // Cargar las preguntas asociadas a esta trivia
                trivia.questions = await LoadQuestionsForTrivia(trivia);
            }
            return trivias;
        }
        else
        {
            // Hubo un error en la operación
            Debug.LogError("Error loading trivias: " + response.ToString());
            return null;
        }
    }

    async Task<List<question>> LoadQuestionsForTrivia(trivia trivia)
    {
        var response = await clientSupabase
            .From<question>()
            .Where(Question => Question.trivia_id == trivia.id)
            .Select("*")
            .Get();

        if (response != null)
        {
            // La operación se completó correctamente y hay datos disponibles
            List<question> questions = response.Models;
            return questions;
        }
        else
        {
            // Hubo un error en la operación
            Debug.LogError("Error loading questions for trivia " + trivia.id + ": " + response.ToString());
            return null;
        }
    }
}
