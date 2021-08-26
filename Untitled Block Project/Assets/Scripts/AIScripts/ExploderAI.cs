using UnityEngine;
using System.Collections;

public class ExploderAI : MonoBehaviour {

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private bool homing = false;

    void Start() {
        StartCoroutine(CheckIfHoming());
    }

    public void HomingBullet(Vector2 pos) {
        homing = true;
        float x = rb.position.x - pos.x;
        float y = rb.position.y - pos.y;
        float speedMultiplier = Mathf.Sqrt((250 * 250) / (x * x + y * y));
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(-speedMultiplier * x, -speedMultiplier * y));
    }

    IEnumerator CheckIfHoming() {
        yield return new WaitForSeconds(0.1f);
        if (!homing) {
            BulletAI bullet = new BulletAI(rb, bulletSpeed: 250);
            bullet.StartPos();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Walls") {
            Vector2 vel = rb.velocity;
            rb.velocity = new Vector2(-vel[0], vel[1]);
        } else if (collision.tag == "Bullet" || collision.tag == "MovingFreezer" || collision.tag == "Shielder") {
            Destroy(collision.gameObject);
        } else if (collision.tag != "Grower"){
            if (collision.tag == "FrozenFreezer") Destroy(collision.gameObject);
            StartCoroutine(DestroySelf());
        }
    }

    private IEnumerator DestroySelf() {
        rb.velocity = Vector2.zero;
        animator.SetBool("explode", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("explode", false);
        Destroy(gameObject);
    }

}