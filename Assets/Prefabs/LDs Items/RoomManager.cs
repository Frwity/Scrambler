using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [HideInInspector] public Vector3 respawnPoint;

    private GameObject[] childCopyList;
    private GameObject[] childList;
    private GameObject doorCopy;
    private int nbChild;
    private int resetMax = 2;

    void Start()
    {
        nbChild = transform.childCount;
        childCopyList = new GameObject[nbChild - 2];
        childList = new GameObject[nbChild - 2];

        for (int i = 2; i < nbChild; ++i)
        {
            childList[i - 2] = transform.GetChild(i).gameObject;
            if (!transform.GetChild(i).gameObject.CompareTag("Enemy"))
                resetMax++;
            childCopyList[i - 2] = Instantiate(transform.GetChild(i).gameObject, transform.GetChild(0));
            childCopyList[i - 2].SetActive(false);
            if (transform.GetChild(i).gameObject == door)
                doorCopy = childCopyList[i - 2];
        }
    }


    void Update()
    {
        if (transform.childCount <= resetMax)
        {
            Destroy(door);
        }
    }

    public void ResetRoom()
    {
        for (int i = 0; i < childList.Length; ++i)
        {
            Destroy(childList[i]);
        }
        for (int i = 0; i < childCopyList.Length; ++i)
        {

            childList[i] = Instantiate(childCopyList[i].gameObject, childCopyList[i].transform.position, childCopyList[i].transform.rotation, transform);
            childList[i].SetActive(true);

            if (childCopyList[i].gameObject == doorCopy)
                door = childList[i];

            if (childList[i].GetComponent<Entity>())
                childList[i].GetComponent<Entity>().ResetEntity();
        }
    }
}
