using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [HideInInspector] public Vector3 respawnPoint;

    private GameObject[] childCopyList;
    private GameObject[] childList;
    private int nbChild;

    void Start()
    {
        nbChild = transform.childCount;
        childCopyList = new GameObject[nbChild - 2];
        childList = new GameObject[nbChild - 2];

        for (int i = 2; i < nbChild; ++i)
        {
            childList[i - 2] = transform.GetChild(i).gameObject;
            childCopyList[i - 2] = Instantiate(transform.GetChild(i).gameObject, transform.GetChild(0));
            childCopyList[i - 2].SetActive(false);
        }
    }


    void Update()
    {

    }

    public void ResetRoom()
    {
        for (int i = 0; i < childList.Length; ++i)
        {
            Destroy(childList[i]);
        }
        for (int i = 0; i < childCopyList.Length; ++i)
        {
            childList[i] = Instantiate(childCopyList[i].gameObject);
            childList[i].SetActive(true);
        }
    }
}
