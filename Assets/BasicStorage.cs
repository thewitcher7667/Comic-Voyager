using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStorage : MonoBehaviour
{
    [SerializeField]
    List<GameObject> STORAGE;

    public GameObject item1_test1;
    public GameObject item1_test2;
    public GameObject item1_test3;


    public int storage_size=10;
    [SerializeField]
    int selected_index;

    void Start()
    {
        STORAGE = new List<GameObject>(storage_size);

        Add(item1_test1);
        Add(item1_test2);
        Add(item1_test3);
        Remove(selected_index);
        Add(item1_test2);
    }
    void Select(int i)
    {
        selected_index = i;
    }
    void Deselect()
    {
        selected_index = -1;
    }
    void Add(GameObject inserted_item)
    {
        STORAGE.Add(inserted_item);
    }
    void Remove(int removed_item)
    {
        if (removed_item>=0)
        {
            STORAGE.RemoveAt(removed_item);
        }else { Debug.LogWarning("Tried to remove index:" + removed_item); }
    }
    void Swap()
    {

    }
}
