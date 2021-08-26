using UnityEngine;

public class FreezerAI : MonoBehaviour {

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D box;

    void Start() {
        BulletAI bullet = new BulletAI(rb, min: 0, freezer: true);
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
            transform.localScale = new Vector3(1.6f, 1.6f, 1);
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.16f);
            box.isTrigger = false;
            tag = "FrozenFreezer";
            Destroy(rb);

            if (!FindObjectOfType<GameManager>().GetFirstFreezer()) {
                FindObjectOfType<GameManager>().SetFreezer(transform.position);
            }

        } else if (collision.tag == "Bullet" || collision.tag == "FrozenFreezer") {
            Destroy(collision.gameObject);
        }
    }

}