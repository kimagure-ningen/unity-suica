using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public bool isDropped = false;
    
    public GameObject evolveCard;

    private float gameOverTimer = 2f;

    public int point = 10;

    private GameMaster gameMaster;
    
    private Animator damageIndicator;

    private void Start()
    {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        if (gameMaster == null)
        {
            Debug.LogError("GameMaster is null");
        }
        
        damageIndicator = GameObject.Find("DamageIndicator").GetComponent<Animator>();
        if (damageIndicator == null)
        {
            Debug.LogError("DamageIndicator is null");
        }
    }
    
    private void Update()
    {
        if (!isDropped)
        {
            float currentMousePosX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            if (currentMousePosX > 3f)
            {
                gameObject.transform.position = new Vector2(3f, transform.position.y);
            } else if (currentMousePosX < -3f)
            {
                gameObject.transform.position = new Vector2(-3f, transform.position.y);
            }
            else
            {
                gameObject.transform.position = new Vector2(currentMousePosX, transform.position.y);
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (this.gameObject.name == collision.gameObject.name)
        {
            Debug.Log("Destroy");
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<CardManager>().evolveCard = null;
            if (evolveCard)
            {
                GameObject _card = Instantiate(evolveCard, this.transform.position, this.transform.rotation);
                gameMaster.score += _card.GetComponent<CardManager>().point;
                _card.GetComponent<Rigidbody2D>().gravityScale = 1;
                _card.GetComponent<Rigidbody2D>().simulated = true;
                _card.GetComponent<CardManager>().isDropped = true;
                
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            gameOverTimer -= Time.deltaTime;
            if (gameOverTimer < 1.8f)
            {
                damageIndicator.SetBool("isDamage", true);
            }
            if (gameOverTimer <= 0)
            {
                gameMaster.GameOver();
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            gameOverTimer = 3f;
            if (damageIndicator == null)
            {
                return;
            }
            damageIndicator.SetBool("isDamage", false);
        }
    }
}
