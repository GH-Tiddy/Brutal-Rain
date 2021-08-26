using UnityEngine;

public class HittersAI : MonoBehaviour {

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float min = -7, max = 7;
    [SerializeField] private int BulletSpeed = 85;
    [SerializeField] private float maxAngle = 1.047f;
    [SerializeField] private bool stream = false;

    void Start() {
        if (PlayerPrefs.GetInt("level") != 16) {
            BulletAI bullet = new BulletAI(rb, min, max, BulletSpeed, maxAngle, stream);
            bullet.StartPos();
        } else {
            rb.AddForce(new Vector2(0, -30));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Walls")) {
            Vector2 vel = rb.velocity;
            rb.velocity = new Vector2(-vel[0], vel[1]);
            /*Vector2 inDirection = rb.velocity;
            Vector2 inNormal = collision.contacts[0].normal;
            Vector2 dir = Vector2.Reflect(inDirection, inNormal).normalized;
            //rb.velocity = BulletSpeed * dir;
            Vector2 vel = rb.velocity;
            rb.velocity = Vector2.zero;
            Debug.Log(vel);
            rb.AddForce(new Vector2(vel[0] * dir[0], vel[1] * dir[1]));*/
        }  else if (collision.CompareTag("BulletTrigger")) {
            Vector2 vel = rb.velocity;
            rb.velocity = new Vector2(vel[0], -vel[1]);
        } else if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Trigger") {
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Player") {
            if (PlayerPrefs.GetInt("level") != 16) {
                FindObjectOfType<GameManager>().LevelFailed();
            }
            Destroy(gameObject);
        }
    }

}