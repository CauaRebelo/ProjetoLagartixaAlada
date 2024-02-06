using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletHellInSpiral : MonoBehaviour
{
    public float angle = 0f;
    public float lifeTime = 5f;
    public float moveSpeed = 5f;
    private Vector2 bulletMoveDirection;
    public void Fire(float angleEnd)
    {

        for (int i = 0; i <= 1; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle + 180f * i) * Mathf.Deg2Rad);
            float bulDirY = transform.position.y + Mathf.Cos((angle + 180f * i) * Mathf.Deg2Rad);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletHellPool.bulletHellPool.GetBullet();
            bul.transform.position = transform.position;
            bul.transform.rotation = transform.rotation;
            bul.GetComponent<NewBulletHell>().lifeTime = lifeTime;
            bul.GetComponent<NewBulletHell>().moveSpeed = moveSpeed;
            bul.GetComponent<NewBulletHell>().SetMoveDirection(bulDir);
            bul.SetActive(true);
        }
        angle += angleEnd;
        if(angle >= 360f)
        {
            angle -= 360f;
        }
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
