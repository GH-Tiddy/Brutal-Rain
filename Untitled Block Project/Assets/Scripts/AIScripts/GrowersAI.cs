using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowersAI : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;

    void Start() {
        //HomingBullet();
    }

    private void Update() {
        HomingBullet();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            rb.velocity = Vector2.zero;
            FindObjectOfType<PlayerMovement>().Grow();
            Destroy(gameObject);
        }
    }

    private void HomingBullet() {
        Vector2 pos = FindObjectOfType<PlayerMovement>().GetPos();
        float x = rb.position.x - pos.x;
        float y = rb.position.y - pos.y;
        float speedMultiplier = Mathf.Sqrt((100 * 100) / (x * x + y * y));
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(-speedMultiplier * x, -speedMultiplier * y));
    }

}