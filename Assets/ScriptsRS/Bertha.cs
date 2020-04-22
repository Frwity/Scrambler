using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
public class Bertha : MonoBehaviour
{
    [SerializeField] private float Cooldown = 0f;
    //[SerializeField] private float Imprecision = 0f;
    [SerializeField] private float shootForce = 0f;
    //[SerializeField] private float damage = 0f;
    [SerializeField] private GameObject DroneToSpawn = null;
    public SpawnPointData spawnPoint = null;
    private DroneSkill ds = null;
    private DroneAi dai = null;
    private Entity Drone = null;
    [SerializeField] private GameObject bullet = null;
    private float currCooldown = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint.ParentPos = transform;
        Drone = Instantiate(DroneToSpawn, spawnPoint.SpawnPointPos, Quaternion.identity).GetComponent<Entity>();
        GetComponentInChildren<Teleport>().toTP = Drone.transform;
        
        if (Drone != null)
        {
            dai = Drone.GetComponent<DroneAi>();
            ds = Drone.GetComponent<DroneSkill>();
            ds.myBertha = this;
            ds.OnShoot.AddListener(shootBullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Drone)
            return;

        if (currCooldown < Cooldown)
        {
            currCooldown += Time.smoothDeltaTime;
            return;
        }

        if (Drone.isPlayerInSight && dai.isActive)
        { 
            shootBullet();
            currCooldown = 0.0f;
        }
    }

    void shootBullet()
    {
        if (!Drone)
            return;

        if (currCooldown < Cooldown)
        {
            return;
        }

        transform.GetChild(2).GetComponent<ParticleSystem>().Play();

        GameObject bu = Instantiate(bullet, transform.position + transform.up * 10, Quaternion.identity);
        bu.transform.localScale += Vector3.one;
        
        Physics.IgnoreCollision(bu.GetComponent<Collider>(), Drone.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(bu.GetComponent<Collider>(), GetComponent<Collider>(), true);
        
        Rigidbody rb = bu.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * shootForce, ForceMode.Impulse);

        BulletSharedClass buSCI = bu.GetComponent<BulletSharedClass>();
        buSCI.shooter = gameObject;
        buSCI.direction = Vector3.up;

        
        currCooldown = 0;
    }
    
    private void OnDrawGizmos()
    {
            Gizmos.color = spawnPoint.color;
            spawnPoint.ParentPos = transform;
            if (!spawnPoint.enabled)
            {
                Color c = Gizmos.color;
                c.a = 0.5f;
                Gizmos.color = c;
            }
            Gizmos.DrawSphere(spawnPoint.SpawnPointPos , spawnPoint.radius);
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(Bertha)), CanEditMultipleObjects]
public class SpawnEditor : Editor
{
    
    public virtual void OnSceneGUI()
    {
        Bertha b = (Bertha) target;

        var vari = b.spawnPoint;
            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition =Handles.PositionHandle(vari.SpawnPointPos, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                //Undo.RecordObject(var, "Change Look At Target Position");
                vari.SpawnPointPos = newTargetPosition;
            }
       
    }
    
    
}
#endif