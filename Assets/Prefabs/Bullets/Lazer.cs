using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : BulletSharedClass
{
    [SerializeField] float range = 5f;
    [SerializeField] float lifeTime = 5f;

    [SerializeField] GameObject lazerBullet = null;

    GameObject firedLazer = null;

    // Start is called before the first frame update
    protected override void Start()
    {
        if ( Physics.Raycast(transform.position, direction, out RaycastHit hit, range, LayerMask.GetMask("Entity")) )
        {
            Vector3 toHit = hit.point - shooter.transform.position;

            firedLazer = Instantiate(lazerBullet, transform.position, Quaternion.identity);

            firedLazer.transform.Translate(toHit / 2);
            firedLazer.transform.localScale = new Vector3(0.25f, 0.25f, toHit.x);
            firedLazer.transform.localRotation = Quaternion.LookRotation(toHit);

            firedLazer.GetComponent<BulletSharedClass>().shooter = shooter;
        }
        else
        {
            Destroy(gameObject);
        }

        shooterTag = shooter.gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
            Destroy(firedLazer);
        }
    }
}
