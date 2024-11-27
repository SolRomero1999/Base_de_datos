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
    private float _timer = 10f; // El temporizador inicial en segundos
    private bool _isTimerRunning = false;

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
            Invoke("RestoreButtonColor", 2f);
            GameManager.Instance._answers.Clear();
            Invoke("NextAnswer", 2f);
        }
        else
        {
            Debug.Log("Respuesta incorrecta");
            GameManager.Instance.LoseLife(); // Resta una vida al jugador
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

}