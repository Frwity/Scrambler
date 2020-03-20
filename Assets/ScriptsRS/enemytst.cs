using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Skills
{
    
    
    
}


public class enemytst : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private MonoBehaviour TEST;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TEST.Invoke("SkillCall", 0);
    }
}
