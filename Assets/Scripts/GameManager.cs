using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject restartButton;

    public void PlayerDied()
    {
        // Mostrar el botón de reinicio
        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
