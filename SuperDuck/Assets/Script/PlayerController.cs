using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;
    public float doulbJumpSpeed;
    public float climbSpeed;
    public float restoreTime;

    private AudioSource myAudioSource;

    private int cherryQty;
    public Text uiText;

    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private BoxCollider2D myFeet;
    private bool isGround;
    private bool canDoubleJump;
    private bool isOneWayPlatform;

    private bool isLadder;
    private bool isClimbing;

    private bool isJumping;
    private bool isFalling;
    private bool isDoubleJumping;
    private bool isDoubleFalling;

    private float playerGravity;

    private PlayerInputActions controls;
    public GameObject ArrowPrefab;
    private Vector2 move;
    private DateTime predatetime;

    private string levelWord;
    private string tmpLevelWord;

    private bool isFirst;
    public static AudioSource audioSrc;
    public static AudioClip pickWord;

    public bool playWordMp3;
    public bool chkplayWordMp3;
    public bool getKey;

    void Awake()
    {
        controls = new PlayerInputActions();

        controls.GamePlay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.GamePlay.Move.canceled += ctx => move = Vector2.zero;
        controls.GamePlay.Jump.started += ctx => Jump();
        controls.GamePlay.ArrowHit.started += ctx => Shoot();

        isFirst = true;
        playWordMp3 = false;
        chkplayWordMp3 = false;
        getKey = false;
    }

    void OnEnable()
    {
        controls.GamePlay.Enable();
    }

    void OnDisable()
    {
        controls.GamePlay.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        playerGravity = myRigidbody.gravityScale;

        isFirst = true;
        predatetime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isGameAlive)
        {
            CheckAirStatus();
            Flip();
            Run();
            //Jump();
            Climb();
            Attack();
            CheckGrounded();
            CheckLadder();
            SwitchAnimation();
            OneWayPlatformCheck();

            if (isFirst)
            {
                levelWord = EnglishBoxController.levelWord;
                tmpLevelWord = levelWord;
                isFirst = false;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                if (playWordMp3)
                {
                    playWordMp3 = false;
                    //關卡單字聲音
                    audioSrc = GetComponent<AudioSource>();
                    pickWord = Resources.Load<AudioClip>("mp3/" + levelWord);
                    audioSrc.PlayOneShot(pickWord);
                    chkplayWordMp3 = true;
                    uiText.text = "請取得拿鑰匙!";
                }

            }
        }
    }

    void CheckGrounded()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("MovingPlatform")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("DestructibleLayer")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        isOneWayPlatform = myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
    }

    void CheckLadder()
    {
        isLadder = myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        //Debug.Log("isLadder:" + isLadder);
    }

    void Flip()
    {
        bool plyerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (plyerHasXAxisSpeed)
        {
            if (myRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (myRigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void Run()
    {
        //float moveDir = Input.GetAxis("Horizontal");
        //Debug.Log("moveDir = " + moveDir.ToString());
        //Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidbody.velocity.y);
        //myRigidbody.velocity = playerVel;
        //bool plyerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //myAnim.SetBool("Run", plyerHasXAxisSpeed);

        Vector2 playerVelocity = new Vector2(move.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnim.SetBool("Run", playerHasXAxisSpeed);
    }

    void Jump()
    {
        //if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
                myAnim.SetBool("Jump", true);
                Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
                myRigidbody.velocity = Vector2.up * jumpVel;
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    myAnim.SetBool("DoubleJump", true);
                    Vector2 doubleJumpVel = new Vector2(0.0f, doulbJumpSpeed);
                    myRigidbody.velocity = Vector2.up * doubleJumpVel;
                    canDoubleJump = false;
                }
            }
        }
    }

    void Climb()
    {
        float moveY = Input.GetAxis("Vertical");

        if (isClimbing)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveY * climbSpeed);
            canDoubleJump = false;
        }

        if (isLadder)
        {
            if (moveY > 0.5f || moveY < -0.5f)
            {
                myAnim.SetBool("Jump", false);
                myAnim.SetBool("DoubleJump", false);
                myAnim.SetBool("Climbing", true);
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveY * climbSpeed);
                myRigidbody.gravityScale = 0.0f;
            }
            else
            {
                if (isJumping || isFalling || isDoubleJumping || isDoubleFalling)
                {
                    myAnim.SetBool("Climbing", false);
                }
                else
                {
                    myAnim.SetBool("Climbing", false);
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0.0f);

                }
            }
        }
        else
        {
            myAnim.SetBool("Climbing", false);
            myRigidbody.gravityScale = playerGravity;
        }

        if (isLadder && isGround) // 
        {
            myRigidbody.gravityScale = playerGravity;
        }

        //Debug.Log("myRigidbody.gravityScale:"+ myRigidbody.gravityScale);
    }

    void Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            myAnim.SetTrigger("Attack");
            grown(true);
        }
    }

    void Shoot()
    {
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
        Instantiate(ArrowPrefab, transform.position, transform.rotation);
        grown(false);
    }

    void grown(bool size)
    {
        // Gets the local scale of game object
        Vector2 objectScale = transform.localScale;
        // Sets the local scale of game object
        if (size)
        {
            myRigidbody.transform.localScale = new Vector3(objectScale.x * 2, objectScale.y * 2);
        }
        else
        {
            myRigidbody.transform.localScale = new Vector3(objectScale.x / 2, objectScale.y / 2);
        }
    }

    void SwitchAnimation()
    {
        myAnim.SetBool("Idle", false);
        if (myAnim.GetBool("Jump"))
        {
            if (myRigidbody.velocity.y < 0.0f)
            {
                myAnim.SetBool("Jump", false);
                myAnim.SetBool("Fall", true);
            }
        }
        else if (isGround)
        {
            myAnim.SetBool("Fall", false);
            myAnim.SetBool("Idle", true);
        }

        if (myAnim.GetBool("DoubleJump"))
        {
            if (myRigidbody.velocity.y < 0.0f)
            {
                myAnim.SetBool("DoubleJump", false);
                myAnim.SetBool("DoubleFall", true);
            }
        }
        else if (isGround)
        {
            myAnim.SetBool("DoubleFall", false);
            myAnim.SetBool("Idle", true);
        }
    }

    void OneWayPlatformCheck()
    {
        if (isGround && gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }

        float moveY = Input.GetAxis("Vertical");
        if (isOneWayPlatform && moveY < -0.1f)
        {
            gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
            Invoke("RestorePlayerLayer", restoreTime);
        }
    }

    void RestorePlayerLayer()
    {
        if (!isGround && gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    void CheckAirStatus()
    {
        isJumping = myAnim.GetBool("Jump");
        isFalling = myAnim.GetBool("Fall");
        isDoubleJumping = myAnim.GetBool("DoubleJump");
        isDoubleFalling = myAnim.GetBool("DoubleFall");
        isClimbing = myAnim.GetBool("Climbing");
        //Debug.Log("isJumping:" + isJumping);
        //Debug.Log("isFalling:" + isFalling);
        //Debug.Log("isDoubleJumping:" + isDoubleJumping);
        //Debug.Log("isDoubleFalling:" + isDoubleFalling);
        //Debug.Log("isClimbing:" + isClimbing);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string ctag = collision.tag;
        DateTime triggerTime = DateTime.Now;

        if (ctag.Length == 1)
        {
            switch (collision.tag)
            {
                case "cherry":
                    Destroy(collision.gameObject);
                    cherryQty += 1;
                    //uiText.text = "吃到櫻桃" + cherryQty + "顆";

                    break;
                default:
                    int timer = GetSubSeconds(predatetime, triggerTime);
                    if (timer > 2)
                    {
                        this.checkWord(collision.tag);
                        Destroy(collision.gameObject);
                        predatetime = triggerTime;
                    }
                    break;
            }

            if (tmpLevelWord == string.Empty)
            {
                //關卡單字圖片                
                UnityEngine.Object cubePreb;
                GameObject cube;

                cubePreb = Resources.Load("pic/" + levelWord, typeof(GameObject));
                cube = Instantiate(cubePreb, new Vector2(0, 0), Quaternion.identity) as GameObject;
                uiText.text = "恭喜過關~~";
            }

        }
        else
        {
            switch (collision.tag)
            {
                case "cherry":
                    Destroy(collision.gameObject);
                    cherryQty += 1;
                    //uiText.text = "吃到櫻桃" + cherryQty + "顆";
                    break;
                case "Trash":
                    Debug.Log("Player is In TrashBin");
                    break;
                case "Key":
                    if (chkplayWordMp3)
                    {
                        uiText.text = "已得拿鑰匙!";
                        Destroy(collision.gameObject);
                        getKey = true;
                    }
                    else
                    {
                        if (tmpLevelWord == string.Empty)
                        {
                            uiText.text = "*****請播放單字聆聽!*****";
                        }
                    }
                    break;
                case "Door":
                    if (getKey)
                    {
                        Application.LoadLevel("SceneL2");
                    }
                    else
                    {
                        if (tmpLevelWord == string.Empty)
                        {
                            uiText.text = "*****請取得鑰匙*****";
                        }
                    }
                    break;
                case "DoorL2":
                    if (getKey)
                    {
                        Application.LoadLevel("SceneL3");
                    }
                    else
                    {
                        if (tmpLevelWord == string.Empty)
                        {
                            uiText.text = "*****請取得鑰匙*****";
                        }
                    }
                    break;
                case "DoorL3":
                    if (getKey)
                    {
                        Application.LoadLevel("SceneL4");
                    }
                    else
                    {
                        if (tmpLevelWord == string.Empty)
                        {
                            uiText.text = "*****請取得鑰匙*****";
                        }
                    }
                    break;
                case "DoorL4":
                    if (getKey)
                    {
                        Application.LoadLevel("SceneHome");
                    }
                    else
                    {
                        if (tmpLevelWord == string.Empty)
                        {
                            uiText.text = "*****請取得鑰匙*****";
                        }
                    }
                    break;
                case "DoorHome":
                    if (getKey)
                    {
                        Application.LoadLevel("SceneConcert");
                    }
                    else
                    {
                        if (tmpLevelWord == string.Empty)
                        {
                            uiText.text = "*****請取得鑰匙*****";
                        }
                    }
                    break;
                case "Enemy":
                case "EnemyBat":

                    Vector2 objectScale = transform.localScale;
                    if (!(objectScale.x < 0.6))
                    {
                        grown(false);

                    }
                    break;
                default:
                    //audioSrc = GetComponent<AudioSource>();
                    //pickWord = Resources.Load<AudioClip>("mp3/" + levelWord);
                    //audioSrc.PlayOneShot(pickWord);

                    if (collision.tag == levelWord)
                    {
                        uiText.text = "~~請按V聽發音~~";
                        playWordMp3 = true;
                    }

                    break;
            }
        }

    }

    /// <summary>
    /// 获取间隔秒数
    /// </summary>
    /// <param name="startTimer"></param>
    /// <param name="endTimer"></param>
    /// <returns></returns>
    public int GetSubSeconds(DateTime startTimer, DateTime endTimer)
    {
        TimeSpan startSpan = new TimeSpan(startTimer.Ticks);

        TimeSpan nowSpan = new TimeSpan(endTimer.Ticks);

        TimeSpan subTimer = nowSpan.Subtract(startSpan).Duration();

        //返回间隔秒数（不算差的分钟和小时等，仅返回秒与秒之间的差）
        //return subTimer.Seconds;

        //返回相差时长（算上分、时的差值，返回相差的总秒数）
        return (int)subTimer.TotalSeconds;
    }

    private void checkWord(string word)
    {
        if (tmpLevelWord.Substring(0, 1) == word)
        {
            Debug.Log("word: " + word);
            tmpLevelWord = tmpLevelWord.Remove(0, 1);
            Debug.Log("levelWord: " + tmpLevelWord);
            uiText.text += word;
        }
        else
        {
            uiText.text += "順序錯了!";
            Application.LoadLevel("Scene1");
        }

    }

}
