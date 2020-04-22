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
        transform.Translate(dir * (speed * Time.unscaledDeltaTime));
        RectTransform tr = GetComponent<RectTransform>();
        Vector2 posi = new Vector2(tr.position.x, tr.position.y);
        
        Vector2 res = new Vector2(Screen.width, Screen.height);
        Vector2 test = (posi + (tr.rect.center));
        if (test.x > res.x)
        {
            test.x = res.x - tr.rect.center.x -1;
            transform.position = test;
        }
        else if (test.x < 0)
        {
            test.x = 0 + tr.rect.center.x +1;
            transform.position = test;
        }
        if (test.y > res.y)
        {
            test.y = res.y - tr.rect.center.y -1;
            transform.position = test;
        }
        else if (test.y < 0)
        {
            test.y = 0 + tr.rect.center.y +1;
            transform.position = test;
        }
        
        pos = test ;
        
    }
}
