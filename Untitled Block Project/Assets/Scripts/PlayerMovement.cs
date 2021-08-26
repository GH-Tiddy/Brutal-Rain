using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    #region Variables
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private float speed = 40f;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator screenFadeAni;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject lWall2;
    [SerializeField] private GameObject lWall3;
    [SerializeField] private GameObject lWall4;
    [SerializeField] private GameObject lWall5;
    private float horizontalMove;
    private bool jump = false;
    private bool reverseMotion = false;
    #endregion

    #region Movement
    public void Start() {
        animator.SetBool("hitGround", false);
        StartCoroutine(CheckActivity());
    }

    IEnumerator CheckActivity() {
        yield return new WaitForEndOfFrame();
        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        rb.position = new Vector2(-screenWidth / 2 + 0.5f, -1.32f);
        if (lWall2.activeSelf) {
            rb.position = new Vector2(rb.position.x, rb.position.y + 1);
            if (lWall3.activeSelf) {
                rb.position = new Vector2(rb.position.x, rb.position.y + 1);
                if (lWall5.activeSelf) {
                    SetSize(0.5f);
                }
            } else if (lWall4.activeSelf) {
                SetSize(0.5f);
            }
        } else if (lWall3.activeSelf) {
            SetSize(0.5f);
        }
    }

    void Update() {   
        if (Input.touchCount > 0) {
            float xTouch = Input.GetTouch(0).position.x;
            float yTouch = Input.GetTouch(0).position.y;
            float width = ((float) Screen.width / Screen.height) * 1080;
            if (xTouch > ((120f/width) * Screen.width) && xTouch < ((520f/width) * Screen.width) && yTouch > (Screen.height * (14f/27f))) {
                horizontalMove = 0;
            } else {
                horizontalMove = (xTouch < Screen.width / 2) ? -speed : speed;
                if (reverseMotion) horizontalMove *= -1;
            }
        } else {
            horizontalMove = 0;
        }
        playerAnimator.SetFloat("xspeed", Mathf.Abs(horizontalMove));
        playerAnimator.SetFloat("yspeed", rb.velocity.y);
    }

    public void Jump() {
        jump = true;
    }

    void FixedUpdate() {
        if (jump) {
            controller.Move(0, false, true);
        } else {
            controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
        }
        jump = false;
    }
    #endregion

    #region Bullet Interactions
    public Vector2 GetPos() {
        return transform.position;
    }

    public void Shrink() {
        transform.localScale = new Vector3(transform.localScale.x * 0.8f, transform.localScale.y * 0.8f, 1);
    }

    public void SetSize(float s) {
        transform.localScale = new Vector3(s, s, 1);
    }

    public float GetSize() {
        return transform.localScale.y;
    }

    public void Grow() {
        StartCoroutine(SlowGrow());
    }

    IEnumerator SlowGrow() {
        short loopNumber = FindObjectOfType<GameManager>().GetLoopNumber();
        while (loopNumber == FindObjectOfType<GameManager>().GetLoopNumber()) {
            if (transform.localScale.x < 2) transform.localScale = new Vector3(transform.localScale.x*1.01f, transform.localScale.y*1.01f, 1);
            yield return new WaitForSeconds(0.35f);
        }
    }

    public void ReverseMotion() {
        reverseMotion = !reverseMotion;
    }

    public void ResetReverse() {
        reverseMotion = false;
    }

    public void MovexPos(float x) {
        rb.position = new Vector2(x, rb.position.y);
    }

    #endregion

    #region Collisions
    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            if (!animator.GetBool("hitGround")) {
                animator.SetBool("tryAgain", false);
                animator.SetBool("hitGround", true);
                SetSize(1);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Trigger") {
            if (PlayerPrefs.GetInt("level") == 0) {
                StartCoroutine(MoveToFirstLevel());
            } else {
                if (PlayerPrefs.GetInt("level") == 16) {
                    PlayerPrefs.SetInt("completed", 1);
                    SceneManager.LoadScene("StartScene");
                }
                StartCoroutine(MoveToNextLevel());
            }
        }
    }

    public IEnumerator MoveToFirstLevel() {
        screenFadeAni.SetBool("tutorial", true);
        yield return new WaitForSeconds(8);
        FindObjectOfType<GameManager>().LevelComplete();
        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        rb.position = new Vector2(-screenWidth / 2 + 0.5f, -1.24f);
        yield return new WaitForSeconds(2);
        screenFadeAni.SetBool("tutorial", false);
    }

    public IEnumerator MoveToNextLevel() {
        screenFadeAni.SetBool("active", true);
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<GameManager>().LevelComplete();
        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        rb.position = new Vector2(-screenWidth / 2 + 0.5f, rb.position.y);
        yield return new WaitForSeconds(0.5f);
        screenFadeAni.SetBool("active", false);
    }
    #endregion

}
