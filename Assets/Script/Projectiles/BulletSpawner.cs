// Bullet Spawner


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletSpawner : MonoBehaviour
{
    enum SpawnerType { Straight, Spin }

    //[SerializeField] private Transform playerTransform;

    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 1f;
    public float speed = 1f;


    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    public float firingRate = 1f;


    private GameObject spawnedBullet;
    //private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        //float angle = Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        //Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100000f * Time.deltaTime);
        //timer += Time.deltaTime;
        //if (spawnerType == SpawnerType.Spin) transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 1f);
        //if (timer >= firingRate)
        //{
            //Fire();
            //timer = 0;
        //}
    }
    public void Fire()
    {
        if (bullet)
        {
            spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            spawnedBullet.GetComponent<Bullet>().speed = speed;
            spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            //spawnedBullet.GetComponent<Bullet>().playerTransform = playerTransform;
            spawnedBullet.transform.rotation = transform.rotation;
        }
    }
}