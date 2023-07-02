using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MenuController : MonoBehaviour
{

    public GameObject menuButtons;

    public bool playPressed;

    public Color playPressedColor;

    public Color playColorBase;

    public Color playColorGo;

    public TextMeshProUGUI playText;

    public GameManager gameManager;

    

    void Start() 
    {
        StartCoroutine("playBlink");
        menuButtons.SetActive(true);
    }

    public void blinkPlayButton()
    {
       StartCoroutine("EnableMenu");
    }

    IEnumerator EnableMenu()
    {
        yield return new WaitForSeconds(3f);
        AudioController.Instance.PlayMusic("Menu");
        StartCoroutine("playBlink");
        menuButtons.SetActive(true);
    }

    IEnumerator playBlink()
    {
        while(playPressed == false)
        {
           yield return new WaitForSeconds(1f);
           if(playPressed == false)
           {
              playText.DOColor(playColorGo, 0.3f).SetEase(Ease.Linear);
              //playText.color = playColorGo;
           }
           yield return new WaitForSeconds(1f);
           if(playPressed == false)
           {
              playText.DOColor(playColorBase, 0.5f).SetEase(Ease.Linear);
              //playText.color = playColorBase;
           }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isGame == false && Input.GetKeyDown(KeyCode.Return))
        {
           if (!playPressed)
           {
                AudioController.Instance.musicSource.Stop();
                AudioController.Instance.PlaySFX("Start");
                playPressed = true;
                playText.DOColor(playPressedColor, 0.8f).SetEase(Ease.Linear);
                StartCoroutine("startGame");
           }
        }
    }

    IEnumerator startGame()
    {
        yield return new WaitForSeconds(2f);
        AudioController.Instance.PlayMusic("Game");
        menuButtons.SetActive(false);
        playPressed = false;
        playText.color = playColorBase;
        gameManager.StartGame();
    }
    
}
