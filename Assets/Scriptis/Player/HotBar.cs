using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    public int SlotsNumber = 4;
    [SerializeField] private GameObject[] hotBar;
    private int currentItemOnHotbar = 0;
    [SerializeField] private GameObject activeItem;
    public PlayerBody player;

    private void Start()
    {
        hotBar = new GameObject[SlotsNumber];
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBody>();
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown((KeyCode) 49+i))
            {
                currentItemOnHotbar = i;
                SetActiveItem();
            }
        }
    }

    private void SetActiveItem()
    {
        activeItem = hotBar[currentItemOnHotbar];
        player.RightHand = activeItem;
    }

    public bool AddToHotBar(GameObject obj)
    {
        for(int i = 0;i < 4; i++)
        {
            if (hotBar[i] == null)
            {
                hotBar[i] = obj;
                return true;
            }
        }

        return false;
    }

}
