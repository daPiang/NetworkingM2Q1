using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameDisplayer : MonoBehaviour
{
    public GameObject displayPrefab;

    public void PopulateDisplay<T>(List<T> list)
    {
        if(transform.childCount > 0)
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        
        
        foreach(T item in list)
        {
            GameObject displayItem = Instantiate(displayPrefab, transform.position, Quaternion.identity);
            displayItem.GetComponent<PlayerNameDisplayItem>().nameToDisplay = item.ToString();
            displayItem.transform.SetParent(transform);
        }
    }
}
