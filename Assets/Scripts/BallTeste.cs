using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTeste : MonoBehaviour
{

    [SerializeField]
    private float lifeTime, speed;

    private Vector2 direction;

    private bool canTeleport = true;

    private SpriteRenderer sprite;

    [SerializeField]
    private List<Color> rainColor;
 
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        speed = GameManager.minSpeed;
        int ballDirection = Random.Range(1, 5);
        if (GameManager.IsRain)
        {
           int newColor = Random.Range(0, 3);
           sprite.color = rainColor[newColor];
        }
        if (GameManager.IsConfused)
        {
            StartCoroutine("ConfusedBall");
        }
        switch(ballDirection)
        {
            case 1:
                direction = Vector2.left + Vector2.up;
            break;
            case 2:
                direction = Vector2.left + Vector2.down;
            break;
            case 3:
                direction = Vector2.right + Vector2.up;
            break;
            case 4: 
                direction = Vector2.right + Vector2.down;
            break;
        }
    }

    IEnumerator ConfusedBall()
    {
        yield return new WaitForSeconds(1f);
        if(speed == 5f)
        {
           speed = 12f;
           StartCoroutine("ConfusedBall");
        } else if(speed == 12f)
        {
           speed = 5f;
           yield return new WaitForSeconds(1f);
           StartCoroutine("ConfusedBall");
        }
    }

    public float thisDirection;

    void Update()
    {
        thisDirection = direction.y;
        if(GameManager.canAccelerate)
        {
            if(lifeTime < GameManager.maxLifeTime)
            {
                lifeTime = lifeTime + Time.deltaTime;
            }
            speed = Mathf.Lerp(GameManager.minSpeed, GameManager.maxSpeed, lifeTime / GameManager.maxLifeTime);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(GameManager.easyMode)
        {
           rb.velocity = direction * (speed / 1.3f);
        }else
        {
           rb.velocity = direction * speed;
        }
    }

    void changeY()
    {
        if (direction.y > 0)
        {
            if (direction.y > 1.4f)
            {
                float changeDirection = 0.1f;
                direction.y = direction.y - changeDirection;
            }
            else
            if (direction.y < 0.6f)
            {
                float changeDirection = 0.1f;
                direction.y = direction.y + changeDirection;
            }
            else
            {
                float changeDirection = Random.Range(-0.2f, 0.2f);
                direction.y = direction.y + changeDirection;
            }
        } else 
        {
            if (direction.y < -1.4f)
            {
                float changeDirection = 0.1f;
                direction.y = direction.y + changeDirection;
            }
            else
            if (direction.y > -0.6f)
            {
                float changeDirection = 0.1f;
                direction.y = direction.y - changeDirection;
            }
            else
            {
                float changeDirection = Random.Range(-0.2f, 0.2f);
                direction.y = direction.y + changeDirection;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AudioController.Instance.PlaySFX("BallBounce");
            //changeY();
            direction.y = -direction.y;
        }

        if (collision.gameObject.CompareTag("VerticalWall"))
        {
            AudioController.Instance.PlaySFX("BallBounce");
            //changeY();
            direction.x = -direction.x;
            if(GameManager.IsRain)
            {
               speed = 5f;
               StartCoroutine("restoreSpeed");
            }
            if (GameManager.IsMagic == 1)
            {
                speed = 3f;
            }
            else if (GameManager.IsMagic == 2)
            {
                speed = 12f;
            }
        }

        if (collision.gameObject.CompareTag("VerticalSave"))
        {
            print("Saved!");
            AudioController.Instance.PlaySFX("BallBounce");
            //changeY();
            direction.x = -direction.x;
            if(GameManager.IsRain)
            {
               speed = 5f;
               StartCoroutine("restoreSpeed");
            }
            if (GameManager.IsMagic == 1)
            {
                speed = 3f;
            }
            else if (GameManager.IsMagic == 2)
            {
                speed = 12f;
            }
        }

        if (collision.gameObject.CompareTag("TeleportTop"))
        {
            if (canTeleport)
            {
                transform.position = new Vector2(transform.position.x, -5.3f);
                StartCoroutine("TeleportCooldown");
                canTeleport = false;
            }
        }

        if (collision.gameObject.CompareTag("TeleportBottom"))
        {
            if (canTeleport)
            {
                transform.position = new Vector2(transform.position.x, 5.3f);
                StartCoroutine("TeleportCooldown");
                canTeleport = false;
            }
        }

        if (collision.gameObject.CompareTag("PlayerL"))
        {
            float paddleVelo;
            paddleVelo = collision.GetComponent<Rigidbody2D>().velocity.y;
            ChangeYPaddle(paddleVelo);
            AudioController.Instance.PlaySFX("BallBounce");
            //changeY();
            direction.x = -direction.x;
            if(GameManager.IsRain)
            {
               speed = 5f;
               StartCoroutine("restoreSpeed");
            }
            if (GameManager.IsMagic == 1)
            {
                speed = 3f;
            }
            else if (GameManager.IsMagic == 2)
            {
                speed = 12f;
            }
        }

        if (collision.gameObject.CompareTag("PlayerR"))
        {
            float paddleVelo;
            paddleVelo = collision.GetComponent<Rigidbody2D>().velocity.y;
            ChangeYPaddle(paddleVelo);
            AudioController.Instance.PlaySFX("BallBounce");
            //changeY();
            direction.x = -direction.x;
            if(GameManager.IsRain)
            {
               speed = 8f;
               StartCoroutine("restoreSpeed");
            }
            if (GameManager.IsMagic == 1)
            {
                speed = 12f;
            }
            else if (GameManager.IsMagic == 2)
            {
                speed = 3f;
            }
        }
    }

    void ChangeYPaddle(float changeDir)
    {
       if(changeDir != 0)
       {
          //Paddle Subindo
          if(changeDir > 0)
          {
            if(direction.y > 0)
            {
              if(direction.y > 0.5f)
              {
                direction.y = direction.y - Random.Range(0.1f, 0.15f);
              }
            } else if(direction.y < 0)
            {
              if(direction.y > -1.4f)
              {
                direction.y = direction.y - Random.Range(0.1f, 0.15f);
              }
            }
          } else 
          //Paddle Descendo
          if(changeDir < 0)
          {
            if(direction.y > 0)
            {
              if(direction.y < 1.4f)
              {
                direction.y = direction.y + Random.Range(0.1f, 0.15f);
              }
            } else if(direction.y < 0)
            {
              if(direction.y < -0.5f)
              {
                direction.y = direction.y + Random.Range(0.1f, 0.15f);
              }
            }
          }
       }
    }

    IEnumerator restoreSpeed()
    {
        yield return new WaitForSeconds(2f);
        speed = 2f;
    }

    IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canTeleport = true;
    }
}
