using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;
    public float checkRadius;

    public Text livesText;
    public Text score;

    private int scoreValue = 0;
    private int lives = 3;

    private bool facingRight = true;
    private bool isOnGround, gameOver, hitLevel2;

    public Transform groundcheck;

    public Transform level1, level2;
    
    public LayerMask allGround;

    public Animator anim;

    public GameObject winObj, loseObj;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        livesText.text = lives.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        if(!gameOver)
        {
             rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        }
       
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
        AnimPlayer(hozMovement, vertMovement);
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Coin" && !gameOver)
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(other.gameObject);

            if(scoreValue == 4 && !hitLevel2 && !gameOver)
            {
                hitLevel2 = true;
                transform.position = level2.position;
                lives = 3;
                livesText.text = lives.ToString();
            }
            if(scoreValue == 8 && !gameOver)
            {
                gameOver = true;
                winObj.SetActive(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Enemy" && !gameOver)
        {
            lives--;
            livesText.text = lives.ToString();
            Destroy(col.gameObject);
            if(lives <= 0)
            {
                loseObj.SetActive(true);    
                speed = 0;
                gameOver = true;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W) && !gameOver)
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetInteger("State", 3);
            }
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    void AnimPlayer(float hozSpeed, float vertSpeed)
    {
        if(!gameOver)
        {
            if(isOnGround && hozSpeed != 0 && vertSpeed == 0)
            {
                anim.SetInteger("State", 2);
            }
            if(isOnGround && hozSpeed == 0 && vertSpeed == 0)
            {
                anim.SetInteger("State", 1);
            }
            if(!isOnGround && vertSpeed > 0)
            {
                anim.SetInteger("State", 3);
            }
        }
 
    }
}
