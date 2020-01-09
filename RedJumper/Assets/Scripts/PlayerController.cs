using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    bool isJump = false;
    bool isDead = false;
    bool isInWater = false;
    int idMove = 0;
    float speed = 3f;
    Animator anim;
    public AudioClip audioPickUpCoin, audioTakingDamage;
    private AudioSource audio;
    public Transform startingPoint;
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("DecreaseScoreInWater", 1.0f, 1.0f);
        rigid = GetComponent<Rigidbody2D>();

        audio = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            Idle();
        }

        Move();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Water"))
        {
            speed = 1f;
            isInWater = true;
        }

        else
        {
            speed = 3f;
            isInWater = false;
        }

        if (isJump)
        {
            anim.ResetTrigger("Jump");
            isJump = false;

            if(idMove == 1 || idMove == 2)
            {
                anim.SetTrigger("Run");
            }

            if (idMove == 0)
            {
                anim.SetTrigger("Idle");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.ResetTrigger("Idle");
        anim.ResetTrigger("Run");
        anim.SetTrigger("Jump");

        isJump = true;
    }

    void DecreaseScoreInWater()
    {
        if (isInWater)
        {
            if (Data.score > 0)
            {
                Data.score -= 2;
            }

            audio.PlayOneShot(audioTakingDamage);
        }

        else
        {
            Data.score -= 0;
        }
    }

    public void MoveLeft()
    {
        idMove = 1;
    }

    public void MoveRight()
    {
        idMove = 2;
    }

    private void Move()
    {
        if(idMove == 1 && !isDead)
        {
            if (!isJump)
            {
                anim.SetTrigger("Run");
            }
            transform.Translate(-1 * Time.deltaTime * speed, 0, 0);
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }

        if(idMove == 2 && !isDead)
        {
            if (!isJump)
            {
                anim.SetTrigger("Run");
            }
            transform.Translate(1 * Time.deltaTime * speed, 0, 0);
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    private void Jump()
    {
        if (!isJump)
        {
            rigid.AddForce(Vector2.up * 300f);
        }
    }

    private void Idle()
    {
        if (!isJump)
        {
            anim.ResetTrigger("Jump");
            anim.ResetTrigger("Run");
            anim.SetTrigger("Idle");
        }

        idMove = 0;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Coin"))
        {
            audio.PlayOneShot(audioPickUpCoin, 0.2f);
            Data.score += 15;
            Destroy(collision.gameObject);
        }

        if (collision.transform.tag.Equals("Goal"))
        {
            Data.level++;
            StartCoroutine(DelayNextScene());
        }

        if (collision.transform.tag.Equals("Pitfall"))
        {
            audio.PlayOneShot(audioTakingDamage);
            Data.score -= 5;
            transform.position = startingPoint.position;
        }
    }

    IEnumerator DelayNextScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameScene" + Data.level);
    }
}
