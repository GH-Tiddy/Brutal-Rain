using UnityEngine;

//Script for the enemy AI and lifeblood
public class BulletAI {

    #region Variables
    private Rigidbody2D rb;
    private float min;
    private float max;
    private int bulletSpeed;
    private float maxAngle;
    private bool stream;
    private bool freezer;
    #endregion

    public BulletAI(Rigidbody2D rb, float min = -7, float max = 7, int bulletSpeed = 85, float maxAngle = 1.047f, bool stream = false, bool freezer = false) {
        this.rb = rb;
        this.min = min;
        this.max = max;
        this.bulletSpeed = bulletSpeed;
        this.maxAngle = maxAngle;
        this.stream = stream;
        this.freezer = freezer;
    }

    public void StartPos() {
        //Set the bullet at a random position on the top of the screen
        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        if (min == -7) min = -screenWidth / 2 + 1.3f;
        if (max == 7) max = screenWidth / 2 - 1.3f;
        float pos = Random.Range(min, max);
        rb.position = new Vector2(pos, 5);
        //Set it on a random angle and move it forward
        float dir = Random.Range(0, maxAngle);
        float x = bulletSpeed * Mathf.Sin(dir);
        float y = bulletSpeed * Mathf.Cos(dir);
        if (stream) {
            if (pos > (max + min / 2)) x *= -1;
        } else if (!freezer || pos > 4) {
            if (System.Convert.ToBoolean(Random.Range(0, 2))) x *= -1;
        }
        rb.AddForce(new Vector2(x, -y));
    }

}