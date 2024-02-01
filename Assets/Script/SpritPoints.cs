using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritPoints : MonoBehaviour
{
    public float spritPoints;
    public float acceleration;
    public float deceleration;
 
    private Rigidbody2D physics;
    private Vector2 direction;
    private Transform playerTransform;
 
    private void Start() {
        physics = GetComponent<Rigidbody2D>();
        physics.gravityScale = 0;
        playerTransform = GameObject.FindWithTag("Player").transform;
        physics.drag = deceleration;
    }
 
    private void FixedUpdate() {
        direction = (playerTransform.position - this.gameObject.transform.position).normalized;
        Vector2 directionalForce = direction * acceleration;
        physics.AddForce(directionalForce * Time.deltaTime, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>() != null)
            {
                col.gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>().mudarSouls(spritPoints);
                spritPoints = 0;
                Destroy(this.gameObject);
            }
        }
    }
}
