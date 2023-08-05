using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUi : MonoBehaviour
{
    [SerializeField] GameObject ui;
    [SerializeField] TextMeshProUGUI text;
    public void Show()
    {
        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void SetPosition(Vector3 position)
    {
        ui.transform.position = position;
    }

    public void SetText(string setText)
    {
        text.text = setText;
    }
}
