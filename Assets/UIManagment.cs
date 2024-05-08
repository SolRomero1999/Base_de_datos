using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManagment : MonoBehaviour
{

    
    [SerializeField] TextMeshProUGUI _categoryText;
    [SerializeField] TextMeshProUGUI _questionText;

    [SerializeField] Button[] _buttons = new Button[3];

    private bool queryCalled;


    private void Start()
    {
        queryCalled = false;
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

            _questionText.text = GameManager.Instance.responseList[Random.Range(0, GameManager.Instance.responseList.Count)].QuestionText;

            queryCalled = true;
        }

        //print(GameManager.Instance.responseList[0].QuestionText);
    }
}
