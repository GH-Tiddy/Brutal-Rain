using UnityEngine;

public class ShrinkersAI : MonoBehaviour {

    [SerializeField] private Rigidbody2D rb;

    void Start() {
        BulletAI bullet = new BulletAI(rb);
        bullet.StartPos();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Walls") {
            Vector2 vel = rb.velocity;
            rb.velocity = new Vector2(-vel[0], vel[1]);
        } else if (collision.tag == "Ground" || collision.tag == "Trigger") {
            Destroy(gameObject);
        } else if (collision.tag == "Player") {
            rb.velocity = Vector3.zero;
            FindObjectOfType<PlayerMovement>().Shrink();
            Destroy(gameObject);
        }
    }

}