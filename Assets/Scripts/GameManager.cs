using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TriviaManager triviaManager;
    private int currentTriviaIndex = 0;

    void Start()
    {
        StartTrivia();
    }

    void StartTrivia()
    {
        // Cargar la trivia desde la base de datos
        triviaManager.LoadTrivia(currentTriviaIndex);
    }

    public void NextQuestion()
    {
        currentTriviaIndex++;
        if (currentTriviaIndex < triviaManager.totalTrivias)
        {
            StartTrivia();
        }
        else
        {
            // Mostrar puntaje final y guardar en la base de datos
            // Mostrar ranking
        }
    }
}

