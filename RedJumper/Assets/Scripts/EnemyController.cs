using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    bool isOnGround = false;
    bool isFacingRight = false;
    public Transform leftBorder, rightBorder;
    float speed = 2f;
    Rigidbody2D rigid;
    Animator anim;
    public int HP;
    bool isDie = false;
    public AudioClip audioTakingDamage;
    private AudioSource audio;
    public bool isBoss;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        audio = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnGround && !isDie)
        {
            if (isFacingRight)
            {
                MoveRight();
            }

            else
            {
                MoveLeft();
            }

            if ((transform.position.x >= rightBorder.position.x && isFacingRight) || (transform.position.x <= leftBorder.position.x && !isFacingRight))
            {
                Flip();
            }
        }
    }

    void MoveRight()
    {
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        if (!isFacingRight)
        {
            Flip();
        }
    }

    void MoveLeft()
    {
        Vector3 pos = transform.position;
        pos.x -= speed * Time.deltaTime;
        transform.position = pos;
        if (isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        isFacingRight = !isFacingRight;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Tile"))
        {
            isOnGround = true;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Tile"))
        {
            isOnGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Tile"))
        {
            isOnGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag.Equals("Player_Side"))
        {
            audio.PlayOneShot(audioTakingDamage);

            if(isFacingRight)
            {
                other.attachedRigidbody.velocity = new Vector2(3f, 5f);
            }

            else if(!isFacingRight)
            {
                other.attachedRigidbody.velocity = new Vector2(-3f, 5f);
            }

            if(isBoss)
            {
                Data.score -= 10;
            }

            else
            {
                Data.score -= 6;
            }
        }

        if(other.transform.tag.Equals("Player_Foot"))
        {
            Transform enemyParent = gameObject.transform.parent;

            audio.PlayOneShot(audioTakingDamage);
            other.attachedRigidbody.velocity = Vector2.up * 7f;
            HP--;

            if(HP <= 0)
            {
                isDie = true;
                rigid.velocity = Vector2.zero;
                anim.SetBool("IsDie", true);

                if(isBoss)
                {
                    Data.score += 50;
                    StartCoroutine(DelayEndScene());
                }

                else
                {
                    Data.score += 20;
                    Destroy(transform.gameObject, 1f);
                }
            }
        }
    }

    IEnumerator DelayEndScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("EndScene");
    }
}
