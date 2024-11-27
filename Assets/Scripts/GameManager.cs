using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<question> responseList; // Lista donde guardo la respuesta de la query hecha en la pantalla de selección de categoría
    public int currentTriviaIndex = 0;
    public int randomQuestionIndex = 0;
    public List<string> _answers = new List<string>();
    public bool queryCalled;
    private int _points;
    private int _maxAttempts = 10;
    public int _numQuestionAnswered = 0;
    string _correctAnswer;
    public int currentLives = 3;

    // Lista para almacenar los índices de las preguntas ya seleccionadas
    private List<int> usedQuestionsIndices = new List<int>();

    public static GameManager Instance { get; private set; }

    void Awake()
    {
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
        // Lógica inicial de la trivia
    }

    public void CategoryAndQuestionQuery(bool isCalled)
    {
        isCalled = UIManagment.Instance.queryCalled;

        if (!isCalled)
        {
            do
            {
                randomQuestionIndex = Random.Range(0, responseList.Count);
            }
            while (usedQuestionsIndices.Contains(randomQuestionIndex));

            usedQuestionsIndices.Add(randomQuestionIndex);

            _correctAnswer = responseList[randomQuestionIndex].CorrectOption;

            _answers.Clear();
            _answers.Add(responseList[randomQuestionIndex].Answer1);
            _answers.Add(responseList[randomQuestionIndex].Answer2);
            _answers.Add(responseList[randomQuestionIndex].Answer3);

            _answers.Shuffle();

            for (int i = 0; i < UIManagment.Instance._buttons.Length; i++)
            {
                UIManagment.Instance._buttons[i].onClick.RemoveAllListeners();

                int index = i;
                UIManagment.Instance._buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = _answers[index];
                UIManagment.Instance._buttons[i].onClick.AddListener(() => UIManagment.Instance.OnButtonClick(index));
            }

            UIManagment.Instance.queryCalled = true;
            UIManagment.Instance.ResetTimer(); // Reinicia el temporizador

            if (usedQuestionsIndices.Count >= responseList.Count)
            {
                GoToTriviaSelectScene();
            }
        }
    }

    public void LoseLife()
    {
        currentLives--;

        UILivesManager uiLivesManager = FindObjectOfType<UILivesManager>();
        if (uiLivesManager != null)
        {
            uiLivesManager.UpdateLivesDisplay(currentLives);
        }

        Debug.Log("Vidas restantes: " + currentLives);

        if (currentLives <= 0)
        {
            EndRound();
        }
    }

    private void EndRound()
    {
        Debug.Log("¡Se han agotado las vidas! Fin de la ronda.");
        SceneManager.LoadScene("EndRoundScene");
    }

    private void GoToTriviaSelectScene()
    {
        SceneManager.LoadScene("TriviaSelectScene");
    }
}