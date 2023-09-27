using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Action PlayerRelocateEvent, CollectedCoinsEvent, GameEndEvent;
    public static Action<int> AchieveLocationEvent;
    public static Action<int, GameObject> HitEnemyEvent;

    [Header("Ui Panels")]
    [SerializeField] ScreenFader menuPanel;
    [SerializeField] ScreenFader gamePanel, gameOverPanel;

    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton, replayButton;

    [Header("Health")]
    [SerializeField] Slider healthSlider;
    [SerializeField] int maxHealth = 100, currentHealth;

    [Header("Score")]
    [SerializeField] int currentScore = 0;
    [SerializeField] TMP_Text scoreText;

    [Header("Relocater")]
    [SerializeField] Transform[] location;
    int relocateCount = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        CleanUI();
        menuPanel.FadeIn();
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.value = currentHealth;
        currentScore = 0;

        Intilize();
    }

    private void OnEnable()
    {
        CollectedCoinsEvent += AddScore;
        HitEnemyEvent += TakeDamage;

        PlayerRelocateEvent += ReloactePlayer;
        AchieveLocationEvent += AchievedLocation;

        GameEndEvent += GameOver;
    }

    private void OnDisable()
    {
        CollectedCoinsEvent -= AddScore;
        HitEnemyEvent -= TakeDamage;

        PlayerRelocateEvent -= ReloactePlayer;
        AchieveLocationEvent -= AchievedLocation;

        GameEndEvent -= GameOver;
    }

    void Intilize()
    {
        playButton.onClick.AddListener(() =>
        {
            CleanUI();
            gamePanel.FadeIn();

        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();

        });

        replayButton.onClick.AddListener(() =>
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        });
    }

    void AddScore()
    {
        currentScore++;
        scoreText.SetText("Score: " + currentScore);
    }

    void TakeDamage(int damage, GameObject obj)
    {
        if (PlayerMovement.Instance.isHitEnemy)
        {
            Destroy(obj);
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            GameOver();
        }

        healthSlider.value = currentHealth;
    }

    void ReloactePlayer()
    {
        PlayerMovement.Instance.SetLocation(location[relocateCount].position);
    }

    void AchievedLocation(int count)
    {
        relocateCount = count;
    }


    void GameOver()
    {
        CleanUI();
        gameOverPanel.FadeIn();
    }


    void CleanUI()
    {
        menuPanel.FadeOut();
        gamePanel.FadeOut();
        gameOverPanel.FadeOut();
    }
}
