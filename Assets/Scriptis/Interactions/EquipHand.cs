using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipHand : Interactable
{
    public override void OnInteract()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            if(player.Back == null)
            {
                player.RightHand = gameObject;

            }
            else
            {
                player.Back.Add(gameObject);
                hotBar.AddToHotBar(gameObject);
            }
            gameObject.SetActive(false);
        }
    }
}
