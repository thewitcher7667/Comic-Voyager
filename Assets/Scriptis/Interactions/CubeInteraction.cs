using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInteraction : Interactable
{
    public override void OnInteract()
    {
        if (interactionType == InteractionType.Click)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                Debug.Log("Interacted");
            }

        }
        else
        {
            if (Input.GetKey(interactionKey))
            {
                Debug.Log("Interacted");
            }
        }
    }
}
