using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIEnd : MonoBehaviour
{

public void LoadTriviaScene()
{
    SceneManager.LoadScene("TriviaSelectScene");
}

}