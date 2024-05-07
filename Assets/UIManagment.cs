using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManagment : MonoBehaviour
{

    
    [SerializeField] TextMeshProUGUI _categoryText;
    [SerializeField] Button[] _buttons = new Button[3];

    List<question> questions = new List<question>();

    private void Start()
    {
        questions = GameManager.Instance.responseList;
    }

    void Update()
    {
        _categoryText.text = GameManager.Instance.triviaName;

        print(GameManager.Instance.responseList[1].QuestionText);
    }
}
