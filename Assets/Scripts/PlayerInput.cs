using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public static string PlayerName;

    public GameObject InputField;

    public void SaveName()
    {
        PlayerName = InputField.GetComponent<Text>().text;
    }
}
