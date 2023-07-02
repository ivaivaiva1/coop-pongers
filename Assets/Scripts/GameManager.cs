using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    //Game Core
    private float scoreMultiplier = 1f;

    public static bool easyMode;

    public bool isGame;

    public MenuController menuController;

    public GameObject ball;

    public float roundTimer;

    public float aliveTimeCount;

    public List<float> aliveTime;

    public float actualScore;

    public List<float> roundScore;

    public List<TextMeshProUGUI> roundScoreTXT;

    public int actualRound;

    public TextMeshProUGUI actualScoreTXT;

    public Image life1, life2, life3;

    private int lifeCount;

    void Awake() 
    {
        //easyMode = true;
        //Application.targetFrameRate = 120;
    }

    public void StartGame()
    {
        ResetGameVars();
        StartCoroutine("NewGameCouroutine");
    }

    IEnumerator NewGameCouroutine()
    {
        CallModes();
        modeText.enabled = true;
        life1.enabled = true;
        life2.enabled = true;
        life3.enabled = true;
        lifeCount = 3;
        yield return new WaitForSeconds(3f);
        isGame = true;
        modeText.enabled = false;
        newBall();
    }

    private void ResetGameVars()
    {
        for (int i = 0; i < roundScoreTXT.Count; i++)
        {
           roundScore[i] = 0f;
           roundScoreTXT[i].text = ("$0");
           modePool[i] = 0;
        }
        roundTimer = 0;
        actualScoreTXT.text = ("0");
        actualScore = 0;
        actualRound = 0;
    }

    void Update()
    {
        if (isGame)
        {
            if (roundTimer >= ballSpawnTime)
            {
                if(transform.childCount < maxBalls)
                {
                newBall();
                roundTimer = 0;
                }
            }
            if(lifeCount != 0 && transform.childCount != 0)
            {
                aliveTimeCount = aliveTimeCount + Time.deltaTime;
                roundTimer = roundTimer + Time.deltaTime;
                actualScore = actualScore + (0.05f * scoreMultiplier);
                actualScoreTXT.text = actualScore.ToString("#");
            }
        }
    }

    public void newBall()
    {
        if(IsLava)
        {
           Instantiate(LavaBall, new Vector3(0f, Random.Range(-5.55f, 5.55f), 0f), Quaternion.identity, transform);
        } else
        {
           Instantiate(ball, new Vector3(Random.Range(-2f, 2f), Random.Range(-5.5f, 5.5f), 0f), Quaternion.identity, transform);
        }    
    }



    //Called when Player die
    IEnumerator NewRound()
    {
        roundScore[actualRound] = actualScore;
        roundScoreTXT[actualRound].text = ("$") + roundScore[actualRound].ToString("#");
        if (actualRound <= 4)
        {
            roundTimer = 0;
            aliveTime[actualRound] = aliveTimeCount;
            aliveTimeCount = 0;
            actualScoreTXT.text = ("0");
            actualRound = actualRound + 1;
            CallModes();
            modeText.enabled = true;
            lifeCount = 3;
            life1.enabled = true;
            life2.enabled = true;
            life3.enabled = true;
            actualScore = 0;
            yield return new WaitForSeconds(3f);
            modeText.enabled = false;
            newBall();
        }
        //Called when Game is Over
        else
        {
            AudioController.Instance.musicSource.Stop();
            AudioController.Instance.PlaySFX("Game Over");
            isGame = false;
            menuController.blinkPlayButton();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
           if(IsLava == false)
           {
                Destroy(collision.gameObject);
                actualScore += 30 * scoreMultiplier;
                Lost();
           }
        }
    }

    public void Lost()
    {
        AudioController.Instance.PlaySFX("Lost");
        lifeCount--;
        if (lifeCount == 2)
        {
            life3.enabled = false;
            newBall();
        }
        else if (lifeCount == 1)
        {
            life2.enabled = false;
            newBall();
        }
        else if (lifeCount == 0)
        {
            int bolas = transform.childCount;
            actualScore = actualScore + ((30 * scoreMultiplier) * bolas);
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            life1.enabled = false;
            UncallModes();
            StartCoroutine("NewRound");
        }
    }

   
    //---------------------------------------------------------------------------
    //Alternative Modes

    public PlayerPaddle Paddle1, Paddle2;

    public float ballSpawnTime = 10f;

    public static bool canAccelerate = true;

    public static float minSpeed = 5f;

    public static float maxSpeed = 9f;

    public static float maxLifeTime = 30f;

    public int maxBalls = 6;

    public TextMeshProUGUI modeText;

    private int whatMode;

    public List<int> modePool;

    //IsBlink
    public GameObject BlinkOBJ;
    public static bool IsBlink;

    //IsInverted
    public static bool IsInverted;

    //IsGravity
    public Rigidbody2D paddle1;
    public Rigidbody2D paddle2;

    //IsRain
    public static bool IsRain;

    //IsTeleporting
    public GameObject TeleportWall;

    //IsLava
    public GameObject LavaBall;
    public static bool IsLava;
    public GameObject IsLavaWalls;

    //IsKeys
    public static bool IsKeys;  

    //IsMagic
    public static int IsMagic;   

    //IsConfused
    public static bool IsConfused;       

 
    //Crazy Mode--    Paranoia + Ball is Lava, Wrong Keys + is inverted, High Gravity + Magic Paddles, Ball is Rain + Teleport
    private void CallModes()
    {
        bool repeated = false;
        while(repeated == false)
        { 
           repeated = true;
           whatMode = Random.Range(1, 11);
           for(int i = 0; i < modePool.Count; i++)
           {
              if(whatMode == modePool[i])
              {
                repeated = false;
              }
           }
        }
        modePool[actualRound] = whatMode; 
        //whatMode = Random.Range(9, 10);
        switch (whatMode)
        {
            case 1:
                AudioController.Instance.PlaySFX("Inverted");
                modeText.text = ("Inverted Keys");
                ballSpawnTime = 5f;
                maxBalls = 3;
                minSpeed = 5f;
                maxSpeed = 10f;
                maxLifeTime = 30f;
                IsKeys = true;
                break;
            case 2:
                AudioController.Instance.PlaySFX("ballIsLava");
                modeText.text = ("Ball is Lava");
                Paddle1.paddleLava();
                Paddle2.paddleLava();
                IsLava = true;
                canAccelerate = false;
                IsLavaWalls.SetActive(true);
                ballSpawnTime = 5f;
                maxBalls = 8;
                minSpeed = 8f;
                scoreMultiplier = 0.5f;
                break;
            //balanced
            case 3:
                AudioController.Instance.PlaySFX("Teleporting");
                modeText.text = ("Teleporting Walls");
                TeleportWall.SetActive(true); 
                maxBalls = 4;
                maxSpeed = 7f;
                ballSpawnTime = 20f;
                maxLifeTime = 30f;
                break;
            //balanced
            case 4:
                AudioController.Instance.PlaySFX("BallsRain");
                modeText.text = ("Balls Rain");
                Paddle1.paddleRain();
                Paddle2.paddleRain();
                IsRain = true;
                ballSpawnTime = 1f;
                canAccelerate = false;
                minSpeed = 2f;
                maxBalls = 20;
                maxLifeTime = 5f;
                break;
            case 5:
                AudioController.Instance.PlaySFX("Just Pong");
                modeText.text = ("Just Pong");
                maxBalls = 1;
                minSpeed = 5f;
                maxSpeed = 20f;
                maxLifeTime = 15f;
                break;
            //balanced
            case 6: 
                AudioController.Instance.PlaySFX("Paranoia");
                modeText.text = ("Paranoia");
                IsBlink = true;
                BlinkOBJ.SetActive(true);
                maxLifeTime = 7f;
                maxBalls = 3;
                break;
            case 7:
                AudioController.Instance.PlaySFX("WrongSides");
                modeText.text = ("Wrong Sides");
                IsInverted = true;
                break;
            case 8:
                AudioController.Instance.PlaySFX("HighGravity");
                modeText.text = ("High Gravity");
                paddle1.gravityScale = 2f;
                paddle2.gravityScale = 2f;
                break;
            case 9:
                AudioController.Instance.PlaySFX("MagicPaddles");
                modeText.text = ("Magic Paddles");
                canAccelerate = false;
                IsMagic = Random.Range(1,3);
                Paddle1.paddleMagic();
                Paddle2.paddleMagic();
                ballSpawnTime = 5f;
                minSpeed = 5f;
                maxBalls = 8;
                break;
            //balanced
            case 10:
                AudioController.Instance.PlaySFX("MadBalls");
                modeText.text = ("Mad Balls");
                canAccelerate = false;
                maxBalls = 4;
                IsConfused = true;
                break;
        }
    }

    private void UncallModes()
    {
        scoreMultiplier = 1f;
        maxBalls = 6;
        ballSpawnTime = 10f;
        canAccelerate = true;
        minSpeed = 5f;
        maxSpeed = 9f;
        maxLifeTime = 30f;
        switch (whatMode)
        {
            case 1:
                IsKeys = false;
                break;
            case 2:
                Paddle1.resetPaddle();
                Paddle2.resetPaddle();
                IsLava = false;
                IsLavaWalls.SetActive(false);
                break;
            case 3:
                TeleportWall.SetActive(false); 
                break;
            case 4:
                Paddle1.resetPaddle();
                Paddle2.resetPaddle();
                IsRain = false;
                break;
            case 5:
    
                break;
            case 6:
                IsBlink = false;
                BlinkOBJ.SetActive(false);
                break;
            case 7:
                IsInverted = false;
                break;
            case 8:
                paddle1.gravityScale = 0f;
                paddle2.gravityScale = 0f;
                break;
            case 9:
                Paddle1.resetColor();
                Paddle2.resetColor();
                IsMagic = 0;
                break;
            case 10:
                IsConfused = false;
                break;
        }
    }
}
