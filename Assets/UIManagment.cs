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

    private int _questionSelected;

    private void Start()
    {
        
    }

    void Update()
    {
        _categoryText.text = GameManager.Instance.triviaName;

        _questionText.text = GameManager.Instance.responseList[1].QuestionText;

        //print(GameManager.Instance.responseList[0].QuestionText);
    }
}
