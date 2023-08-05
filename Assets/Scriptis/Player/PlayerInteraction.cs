using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Transform cam;
    public float maxDist = 10f;
    [SerializeField] PlayerInteractionUi interaction; 
    void Start()
    {
        interaction.Hide();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        bool rayc = Physics.Raycast(player.position,cam.TransformDirection(Vector3.forward),out hit, maxDist);
        if(rayc)
        {
            Interactable obj;
            bool active =  hit.transform.gameObject.TryGetComponent<Interactable>(out obj);

            if(active)
            {

                //interaction.SetPosition(hit.transform.position);
                interaction.SetText(obj.interactionKey.ToString());
                interaction.Show(); 

                obj.OnInteract();
            }
            else
                interaction.Hide(); 
        }
        else
            interaction.Hide();

    }
}
