using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIEnd : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _finalScoreText; // Referencia al TextMeshPro para mostrar el puntaje

    void Start()
    {
        // Recuperamos el puntaje final guardado en PlayerPrefs y lo mostramos en la pantalla
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0); // 0 es el valor por defecto si no se encuentra el puntaje
        _finalScoreText.text = "Puntaje Final: " + finalScore.ToString();
    }
public void LoadTriviaScene()
{
    // Destruir las instancias solo si aún no han sido destruidas
    if (GameManager.Instance != null)
    {
        Destroy(GameManager.Instance.gameObject);
    }

    if (UIManagment.Instance != null)
    {
        Destroy(UIManagment.Instance.gameObject);
    }

    // Cargar la escena de selección de trivia
    SceneManager.LoadScene("TriviaSelectScene");
}

}
