using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using TMPro;
using unityroom.Api;

public class GameMaster : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cards;
    
    [SerializeField]
    private GameObject _card;

    private bool isDroping = false;
    
    public int score = 0;
    
    private bool gameEnd = false;
    [SerializeField] private GameObject gameOverUI;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [SerializeField]
    private AudioClip dropSound;
    [SerializeField]
    private AudioClip retrySound;
    
    private AudioSource audioSource;

    private void Start()
    {
        NewCard();
        gameOverUI.SetActive(false);
        Time.timeScale = 1;
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("DropSound is null");
        }
    }

    private void NewCard()
    {
        _card = Instantiate(cards[Random.Range(0, 5)], new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 4f), Quaternion.identity);
        isDroping = false;
    }

    private void Update()
    {
        scoreText.text = "残高: " + score + "円";
        
        if (!isDroping)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDroping = true;
                audioSource.PlayOneShot(dropSound);
                _card.GetComponent<Rigidbody2D>().gravityScale = 1;
                _card.GetComponent<Rigidbody2D>().simulated = true;
                _card.GetComponent<CardManager>().isDropped = true;
                _card = null;
                NewCard();
            }
        }
    }

    public void GameOver()
    {
        if (gameEnd)
        {
            return;
        }
        gameEnd = true;
        Time.timeScale = 0;
        UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.Always);
        Debug.Log("Game Over");
        Debug.Log(score);
        gameOverUI.SetActive(true);
    }

    public void OnRetry()
    {
        audioSource.PlayOneShot(retrySound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
