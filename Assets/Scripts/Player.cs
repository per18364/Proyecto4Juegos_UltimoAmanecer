using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private int hp = 30;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private float damageResetTime = 3.0f;
    private GameObject dmg;
    private float lastDamageTime; // Tiempo del último golpe recibido
    private bool isResettingDamage;
    private int currHp;
    // Start is called before the first frame update
    void Start()
    {
        dmg = gameCanvas.transform.Find("Damage").gameObject;
        lastDamageTime = Time.time;
        isResettingDamage = false;
        currHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (dmg.activeSelf)
        {
            if (isResettingDamage)
            {
                Color dmgColor = dmg.GetComponent<Image>().color;
                dmgColor.a -= 0.2f * Time.deltaTime;
                dmg.GetComponent<Image>().color = dmgColor;
                if (dmgColor.a <= 0)
                {
                    dmg.SetActive(false);
                    isResettingDamage = false;
                }
            }
            else if (Time.time - lastDamageTime >= damageResetTime)
            {
                isResettingDamage = true;
                currHp = hp;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player took " + damage + " damage");
        currHp -= damage;
        lastDamageTime = Time.time;
        isResettingDamage = false;

        // make the damage color appear gradually
        dmg.SetActive(true);
        Color dmgColor = dmg.GetComponent<Image>().color;
        dmgColor.a += 0.33f;
        dmg.GetComponent<Image>().color = dmgColor;
        if (currHp <= 0)
        {
            Debug.Log("Player is dead");
            GameObject roundsText = gameCanvas.transform.Find("Roundtext").gameObject;
            GameObject gameOverText = gameCanvas.transform.Find("GameOverText").gameObject;
            gameOverText.GetComponent<TextMeshProUGUI>().text = "GAME OVER\n\nSobreviviste hasta " + roundsText.GetComponent<TextMeshProUGUI>().text;
            gameOverText.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
