using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagment : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _categoryText;
    [SerializeField] TextMeshProUGUI _questionText;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] TextMeshProUGUI _scoreText; // Para mostrar la puntuación
    private float _timer = 10f; // El temporizador inicial en segundos
    private bool _isTimerRunning = false;

    private int _score = 0; // Puntuación inicial

    string _correctAnswer;
    public Button[] _buttons = new Button[3];
    [SerializeField] Button _backButton;
    private List<string> _answers = new List<string>();
    public bool queryCalled;
    private Color _originalButtonColor;
    public static UIManagment Instance { get; private set; }

    public void LoadTriviaSelectScene()
    {
        SceneManager.LoadScene("TriviaSelectScene");
    }

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

    private void Start()
    {
        queryCalled = false;
        _originalButtonColor = _buttons[0].GetComponent<Image>().color;
    }

    void Update()
    {
        if (_isTimerRunning)
        {
            _timer -= Time.deltaTime;
            _timer = Mathf.Max(_timer, 0); // Asegura que el tiempo no se vuelva negativo

            _timerText.text = Mathf.CeilToInt(_timer).ToString(); // Muestra el tiempo restante en el texto

            if (_timer <= 0)
            {
                _isTimerRunning = false;
                OnTimeUp();
            }
        }

        _categoryText.text = PlayerPrefs.GetString("SelectedTrivia");
        _questionText.text = GameManager.Instance.responseList[GameManager.Instance.randomQuestionIndex].QuestionText;
        GameManager.Instance.CategoryAndQuestionQuery(queryCalled);
    }

    public void OnButtonClick(int buttonIndex)
    {
        string selectedAnswer = _buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text;
        _correctAnswer = GameManager.Instance.responseList[GameManager.Instance.randomQuestionIndex].CorrectOption;

        if (selectedAnswer == _correctAnswer)
        {
            Debug.Log("¡Respuesta correcta!");
            ChangeButtonColor(buttonIndex, Color.green);
            AddPoints(10); // Sumar 10 puntos por respuesta correcta
            Invoke("RestoreButtonColor", 2f);
            GameManager.Instance._answers.Clear();
            Invoke("NextAnswer", 2f);
        }
        else
        {
            Debug.Log("Respuesta incorrecta");
            GameManager.Instance.LoseLife(); // Resta una vida al jugador
            SubtractPoints(5); // Restar 5 puntos por respuesta incorrecta
            ChangeButtonColor(buttonIndex, Color.red);
            Invoke("RestoreButtonColor", 2f);
            GameManager.Instance._answers.Clear();
            Invoke("NextAnswer", 2f);
        }
    }

    private void ChangeButtonColor(int buttonIndex, Color color)
    {
        Image buttonImage = _buttons[buttonIndex].GetComponent<Image>();
        buttonImage.color = color;
    }

    private void RestoreButtonColor()
    {
        foreach (Button button in _buttons)
        {
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = _originalButtonColor;
        }
    }

    private void NextAnswer()
    {
        queryCalled = false;
        ResetTimer();
    }

    private void OnTimeUp()
    {
        Debug.Log("¡El tiempo se ha agotado!");
        GameManager.Instance.LoseLife(); // Resta una vida
        SubtractPoints(5); // Restar 5 puntos cuando el tiempo se acaba

        if (GameManager.Instance.currentLives > 0)
        {
            queryCalled = false;
            NextAnswer();
        }
    }

    public void ResetTimer()
    {
        _timer = 10f;
        _isTimerRunning = true;
    }

    public void PreviousScene()
    {
        // Destruir explícitamente las instancias de GameManager y UIManagment
        Destroy(GameManager.Instance.gameObject);
        Destroy(UIManagment.Instance.gameObject);

        // Cargar la escena anterior
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void AddPoints(int points)
    {
        _score += points;
        UpdateScoreDisplay(Color.green); // Poner en verde cuando se suman puntos
    }

    public void SubtractPoints(int points)
    {
        _score -= points;
        UpdateScoreDisplay(Color.red); // Poner en rojo cuando se restan puntos
    }

    private void UpdateScoreDisplay(Color textColor)
    {
        _scoreText.text = _score.ToString();
        StartCoroutine(ChangeScoreColor(textColor));
    }

    private IEnumerator ChangeScoreColor(Color targetColor)
    {
        _scoreText.color = targetColor;
        yield return new WaitForSeconds(1f);
        _scoreText.color = Color.white;
    }

    public int GetCurrentScore()
    {
        return _score;
    }
}
