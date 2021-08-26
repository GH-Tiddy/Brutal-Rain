using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour {

    [SerializeField] private bool left;

    void Start() {
        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        float x = left ? -screenWidth / 2 + 9 : screenWidth / 2 - 9;
        transform.position = new Vector2(x, 0);
    }

}
