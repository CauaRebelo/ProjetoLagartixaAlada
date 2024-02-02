using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletHellInArc : MonoBehaviour
{
    public int bulletsAmount = 10;
    public float startAngle = 90f, endAngle = 270f;
    public float attackDelay;
    public float attackInterval;
    public float moveSpeed;
    public float lifeTime;

    private Vector2 bulletMoveDirection;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("Fire", attackDelay, attackInterval);
    }

    void OnEnable()
    {
        InvokeRepeating("Fire", attackDelay, attackInterval);
    }

    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletsAmount + 1; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletHellPool.bulletHellPool.GetBullet();
            bul.transform.position = transform.position;
            bul.transform.rotation = transform.rotation;
            bul.GetComponent<NewBulletHell>().lifeTime = lifeTime;
            bul.GetComponent<NewBulletHell>().moveSpeed = moveSpeed;
            bul.GetComponent<NewBulletHell>().SetMoveDirection(bulDir);
            bul.SetActive(true);

            angle += angleStep;
        }
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
