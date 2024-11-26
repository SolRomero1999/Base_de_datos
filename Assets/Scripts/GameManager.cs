using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<question> responseList; //lista donde guardo la respuesta de la query hecha en la pantalla de selección de categoría
    public int currentTriviaIndex = 0;
    public int randomQuestionIndex = 0;
    public List<string> _answers = new List<string>();
    public bool queryCalled;
    private int _points;
    private int _maxAttempts = 10;
    public int _numQuestionAnswered = 0;
    string _correctAnswer;
    public int currentLives = 3;

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
        queryCalled = false;
    }

    void StartTrivia()
    {

    }
    
    public void CategoryAndQuestionQuery(bool isCalled)
    {
        isCalled = UIManagment.Instance.queryCalled;

        if (!isCalled)
        {
            randomQuestionIndex = Random.Range(0, GameManager.Instance.responseList.Count);

            _correctAnswer = GameManager.Instance.responseList[randomQuestionIndex].CorrectOption;

            _answers.Clear(); // Limpia la lista de respuestas previas
            _answers.Add(GameManager.Instance.responseList[randomQuestionIndex].Answer1);
            _answers.Add(GameManager.Instance.responseList[randomQuestionIndex].Answer2);
            _answers.Add(GameManager.Instance.responseList[randomQuestionIndex].Answer3);

            // Mezcla las respuestas
            _answers.Shuffle();

            // Asigna las respuestas a los botones
            for (int i = 0; i < UIManagment.Instance._buttons.Length; i++)
            {
                // Limpia los listeners anteriores
                UIManagment.Instance._buttons[i].onClick.RemoveAllListeners();

                // Asigna la nueva respuesta al botón
                int index = i;
                UIManagment.Instance._buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = _answers[index];
                UIManagment.Instance._buttons[i].onClick.AddListener(() => UIManagment.Instance.OnButtonClick(index));
            }

            UIManagment.Instance.queryCalled = true;
        }
    }

 // Método para restar una vida
    public void LoseLife()
    {
        currentLives--;

        // Actualiza la visualización de las vidas en la UI
        UILivesManager uiLivesManager = FindObjectOfType<UILivesManager>();
        if (uiLivesManager != null)
        {
            uiLivesManager.UpdateLivesDisplay(currentLives);
        }

        Debug.Log("Vidas restantes: " + currentLives);

        // Si el jugador se queda sin vidas, se pasa a la escena de fin de ronda
        if (currentLives <= 0)
        {
            EndRound();
        }
    }

    // Método para finalizar la ronda
    private void EndRound()
    {
        Debug.Log("¡Se han agotado las vidas! Fin de la ronda.");
        SceneManager.LoadScene("EndRoundScene"); // Cambia a la escena de fin de ronda
    }
}