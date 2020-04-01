using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : BulletSharedClass
{
    [SerializeField] float range = 5f;
    [SerializeField] float lifeTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale += new Vector3(0, range, 0);
        transform.Translate( direction * (range + 1) );

        transform.Rotate(Quaternion.LookRotation(direction).eulerAngles);
        transform.Rotate(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( (other.CompareTag("Player") || other.CompareTag("Enemy")) && (other.gameObject != shooter) )
        {
            other.GetComponent<Entity>().InflictDamage(damage);
        }
    }
}
