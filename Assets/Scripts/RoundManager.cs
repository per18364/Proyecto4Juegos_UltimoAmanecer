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
    private int zombiesPerRound = 1;
    public TextMeshProUGUI roundText;
    private int zombieBaseHP = 100; // HP base de los zombies

    // Añadir AudioSource para la música de fondo
    private AudioSource backgroundMusic;

    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();

        // Verificar si el AudioSource está asignado y reproducir la música
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true; // Asegurarse de que la música se repita
            backgroundMusic.Play();
        }

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
        roundText.text = "Ronda " + roundNumber;
        int zombiesToSpawn = zombiesPerRound + (roundNumber - 1) * 2;
        int zombieHP = zombieBaseHP + ((roundNumber - 1) / 2) * 50; // Incrementa el HP cada 2 rondas

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            SpawnZombie(zombieHP);
        }
    }

    void SpawnZombie(int hp)
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject zombie = Instantiate(zombiePrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
        zombie.GetComponent<Zombie>().SetHP(hp);
    }
}
