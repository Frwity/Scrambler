using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class pointer : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    public Vector2 pos; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = new Vector2(Input.GetAxis("RHorizontal"), Input.GetAxis("RVertical"));
        transform.Translate(dir * (speed * Time.smoothDeltaTime));
        RectTransform tr = GetComponent<RectTransform>();
        Vector2 posi = new Vector2(tr.position.x, tr.position.y);
        
        Vector2 res = new Vector2(Screen.width, Screen.height);
        float t = res.x / res.y;
        pos = (posi + (tr.rect.center)) ;
        //Debug.Log(pos);
    }
}
