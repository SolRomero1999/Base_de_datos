using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public TriviaManager triviaManager;

    public List<question> responseList; //lista donde guardo la respuesta de la query hecha en la pantalla de selección de categoría

    public int currentTriviaIndex = 0;

    public static GameManager Instance { get; private set; }


    void Awake()
    {
        // Configura la instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Para mantener el objeto entre escenas
        }
        else
        {
            Destroy(gameObject);
        }

    }


    void Start()
    {

        StartTrivia();

    }

    void StartTrivia()
    {
        // Cargar la trivia desde la base de datos
        //triviaManager.LoadTrivia(currentTriviaIndex);

        //print(responseList.Count);

    }

    //public void NextQuestion()
    //{
    //    currentTriviaIndex++;
    //    if (currentTriviaIndex < triviaManager.totalTrivias)
    //    {
    //        StartTrivia();
    //    }
    //    else
    //    {
    //        // Mostrar puntaje final y guardar en la base de datos
    //        // Mostrar ranking
    //    }
    //}

    private void Update()
    {
        
    }
}

