using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{

    private PlayerBody()
    {
    }

    private static PlayerBody Instiation = new PlayerBody();
    public static PlayerBody GetInitiation()
    {
        return Instiation;
    }

    public GameObject Head;

    public GameObject Body;

    public List<GameObject> Back;

    public GameObject LeftHand;

    public GameObject RightHand;

    public GameObject LeftLeg;

    public GameObject RightLeg;

    private void Start()
    {
        Back = new List<GameObject>(10);
    }
}
