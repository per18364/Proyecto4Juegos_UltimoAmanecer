using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private int hp = 30;
    [SerializeField] private Canvas gameCanvas;
    private GameObject dmg;
    // Start is called before the first frame update
    void Start()
    {
        dmg = gameCanvas.transform.Find("Damage").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (dmg.activeSelf)
        {
            Color dmgColor = dmg.GetComponent<Image>().color;
            dmgColor.a -= 0.2f * Time.deltaTime;
            dmg.GetComponent<Image>().color = dmgColor;
            if (dmgColor.a <= 0)
            {
                dmg.SetActive(false);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player took " + damage + " damage");
        hp -= damage;

        // make the damage color appear gradually
        dmg.SetActive(true);
        Color dmgColor = dmg.GetComponent<Image>().color;
        dmgColor.a += 0.33f;
        dmg.GetComponent<Image>().color = dmgColor;
        if (hp <= 0)
        {
            Debug.Log("Player is dead");
            GameObject gameOverText = gameCanvas.transform.Find("GameOverText").gameObject;
            gameOverText.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
