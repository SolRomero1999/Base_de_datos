using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManagment : MonoBehaviour
{

    
    [SerializeField] TextMeshProUGUI _categoryText;
    [SerializeField] TextMeshProUGUI _questionText;
    
    string _correctAnswer;

    [SerializeField] Button[] _buttons = new Button[3];

    private List<string> _answers = new List<string>();

    private bool queryCalled;

    private Color _originalButtonColor;


    private void Start()
    {
        queryCalled = false;

        _originalButtonColor = _buttons[0].GetComponent<Image>().color;
    }

    void Update()
    {
        CategoryAndQuestionQuery();


    }

    private void CategoryAndQuestionQuery()
    {
        if (!queryCalled)
        {
            _categoryText.text = PlayerPrefs.GetString("SelectedTrivia");

            int randomQuestionIndex = Random.Range(0, GameManager.Instance.responseList.Count);

            _questionText.text = GameManager.Instance.responseList[randomQuestionIndex].QuestionText;
            _correctAnswer = GameManager.Instance.responseList[randomQuestionIndex].CorrectOption;

            //agrego a la lista de answers las 3 answers

            _answers.Add(GameManager.Instance.responseList[randomQuestionIndex].Answer1);
            _answers.Add(GameManager.Instance.responseList[randomQuestionIndex].Answer2);
            _answers.Add(GameManager.Instance.responseList[randomQuestionIndex].Answer3);

            // la mixeo con el método Shuffle (ver script Shuffle List)

            _answers.Shuffle();

            // asigno estos elementos a los textos de los botones

            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = _answers[i];

                int index = i; // Captura el valor actual de i en una variable local -- SINO NO FUNCA!

                _buttons[i].onClick.AddListener(() => OnButtonClick(index));
            }


            queryCalled = true;
        }
                
    }

    private void OnButtonClick(int buttonIndex)
    {
        string selectedAnswer = _buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text;

        if (selectedAnswer == _correctAnswer)
        {
            Debug.Log("¡Respuesta correcta!");
            ChangeButtonColor(buttonIndex, Color.green);
            Invoke("RestoreButtonColor", 2f);
            Invoke("NextAnswer", 2f);
        }
        else
        {
            Debug.Log("Respuesta incorrecta. Inténtalo de nuevo.");
            
            ChangeButtonColor(buttonIndex, Color.red);
            Invoke("RestoreButtonColor", 2f);
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
    }


}
