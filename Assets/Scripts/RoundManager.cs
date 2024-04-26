using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public GameObject zombiePrefab; // Prefab del zombie
    public Transform[] spawnPoints; // Puntos de spawn para los zombies
    private int roundNumber = 0;
    private int zombiesPerRound = 4;
    public TextMeshProUGUI roundText;

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Zombie").Length == 0)
        {
            // Si no hay zombies, inicia la siguiente ronda
            StartRound();
        }
    }

    void StartRound()
    {
        roundNumber++;
        roundText.text = "Round " + roundNumber;
        int zombiesToSpawn = zombiesPerRound + (roundNumber - 1) * 2;

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(zombiePrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
    }
}
