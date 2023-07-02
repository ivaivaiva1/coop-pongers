using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerPaddle : MonoBehaviour
{

    [SerializeField]
    private GameObject SaveWall;

    [SerializeField]
    private float saveTime;

    public int whatPaddle;

    public float speed;

    private Vector3 initialPosition;

    private Vector2 _direction;

    private Rigidbody2D _rigidbody;

    public GameManager gameManager;

    private SpriteRenderer sprite;

    public Color baseColor;

    public Color MagicColor1;

    public Color MagicColor2;

    public Color white;

    public void resetPaddle()
    {
        transform.DOScaleY(5.3f, 0.5f);
        transform.DOMove(initialPosition, 0.5f);
        sprite.DOColor(baseColor, 0.5f);
    }

    public void resetColor()
    {
        sprite.DOColor(baseColor, 0.5f);
    }

    public void paddleRain()
    {
        transform.DOScaleY(10f, 0.5f);
        transform.DOMove(initialPosition, 0.5f);
        sprite.DOColor(white, 0.5f);
    }

    public void paddleLava()
    {
        transform.DOScaleY(2f, 0.5f);
        transform.DOMove(initialPosition, 0.5f);
    }

    public void paddleMagic()
    {
        if(GameManager.IsMagic == 1)
        {
           if(whatPaddle == 0)
           {
              sprite.DOColor(MagicColor2, 0.5f);
           } else
           {
              sprite.DOColor(MagicColor1, 0.5f);
           } 
        } else if(GameManager.IsMagic == 2)
        {
           if(whatPaddle == 0)
           {
              sprite.DOColor(MagicColor1, 0.5f);
           } else
           {
              sprite.DOColor(MagicColor2, 0.5f);
           } 
        }
    }


    void Awake()
    {
        initialPosition = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void isKeysMoviment()
    {
        if (GameManager.IsKeys == true)
        {
            if (whatPaddle == 0)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    _direction = Vector2.up;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    _direction = Vector2.down;
                }
                else
                {
                    _direction = Vector2.zero;
                }
            }
            else if (whatPaddle == 1)
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    _direction = Vector2.up;
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    _direction = Vector2.down;
                }
                else
                {
                    _direction = Vector2.zero;
                }
            }
        }
    }

    void isInvertedMoviment()
    {
        if (GameManager.IsInverted == true)
        {
            if (whatPaddle == 1)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    _direction = Vector2.up;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    _direction = Vector2.down;
                }
                else
                {
                    _direction = Vector2.zero;
                }
            }
            else if (whatPaddle == 0)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    _direction = Vector2.up;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    _direction = Vector2.down;
                }
                else
                {
                    _direction = Vector2.zero;
                }
            }
        }
    }
  
    void normalMoviment()
    {
        if (GameManager.IsInverted == false && GameManager.IsKeys == false)
        {
            if (whatPaddle == 0)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    _direction = Vector2.up;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    _direction = Vector2.down;                
                }
                else
                {
                    _direction = Vector2.zero;
                }
            }
            else if (whatPaddle == 1)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    _direction = Vector2.up;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    _direction = Vector2.down;
                }
                else
                {
                    _direction = Vector2.zero;
                }
            }
        }
    }

    void Update()
    {
       normalMoviment();
       isInvertedMoviment();
       isKeysMoviment();
       SaveWallVoid();
    //    if(GameManager.IsRain == true)
    //    {
    //       transform.localScale = new Vector3(0.5f, 10f, 0f);
    //    } else if(GameManager.IsLava == true)
    //    {
    //       transform.localScale = new Vector3(0.5f, 2f, 0f);
    //    } else
    //    {
    //       transform.localScale = new Vector3(0.5f, 5.3f, 0f);
    //    }
    }

    private void SaveWallVoid()
    {
       if(saveTime > 0)
       {
          saveTime = saveTime - 0.1f;
          SaveWall.SetActive(true);
       } else 
       {
          SaveWall.SetActive(false);
       }
    }

    void FixedUpdate() 
    {
        if(_direction.sqrMagnitude != 0)
        {
           _rigidbody.AddForce(_direction * this.speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Ball"))
        {
           if(GameManager.IsLava == true)
           {
              Destroy(other.gameObject);
              gameManager.Lost();
           } else 
           {
             saveTime = 8f;
           }
        }
    }
}
