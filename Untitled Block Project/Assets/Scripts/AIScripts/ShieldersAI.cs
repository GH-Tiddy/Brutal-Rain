using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldersAI : MonoBehaviour {

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite sprite;
    [SerializeField] private CircleCollider2D circCollider;
    [SerializeField] private PolygonCollider2D polyCollider;
    private bool isShield = false;

    void Start() {
        BulletAI bullet = new BulletAI(rb, bulletSpeed: 120);
        bullet.StartPos();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isShield) {
            if (collision.tag == "Bullet" || collision.tag == "MovingFreezer") {
                FindObjectOfType<PlayerMovement>().ReverseMotion();
                Destroy(collision.gameObject);
                Destroy(gameObject);
            } else if (collision.tag == "Shielder") {
                Destroy(collision.gameObject);
            } else if (collision.tag == "Exploder") {
                FindObjectOfType<PlayerMovement>().ResetReverse();
            }
        } else {
            if (collision.tag == "Walls") {
                Vector2 vel = rb.velocity;
                rb.velocity = new Vector2(-vel[0], vel[1]);
            } else if (collision.tag == "Ground" || collision.tag == "Trigger") {
                Destroy(gameObject);
            } else if (collision.tag == "Player") {
                sr.sprite = sprite;
                rb.velocity = Vector3.zero;
                GameObject player = GameObject.FindWithTag("Player");
                float s = FindObjectOfType<PlayerMovement>().GetSize();
                transform.localScale = new Vector2(s, s);
                transform.parent = player.transform;
                transform.position = FindObjectOfType<PlayerMovement>().GetPos();
                FindObjectOfType<PlayerMovement>().ReverseMotion();
                isShield = true;
                circCollider.enabled = false;
                polyCollider.enabled = true;
                float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
                if (player.transform.position.x < ((-screenWidth/2) +1.92494f)) FindObjectOfType<PlayerMovement>().MovexPos((-screenWidth / 2) + 1.92494f);
                Destroy(rb);
            }
        }
    }
}
