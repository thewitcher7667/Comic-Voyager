using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    public enum InteractionType
    {
        Click,
        Hold
    }

    public KeyCode interactionKey = KeyCode.E;

    public float intearcationHoldKeySeconds = 3f;

    public InteractionType interactionType = InteractionType.Click;

    public PlayerBody player;

    public HotBar hotBar;

    public abstract void OnInteract();

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBody>();
        hotBar = GameObject.FindWithTag("Player").GetComponent<HotBar>();
    }
}
