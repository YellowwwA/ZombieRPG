using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public GameObject Name;

    public void OpenTab()
    {
        gameObject.SetActive(true);
    }
    public void closeTab()
    {
        gameObject.SetActive(false);
    }
    public void RemoveText()
    {
        Name.transform.GetChild(2).GetComponent<Text>().text = "";
    }
    public void InputText(string word)
    {
        string existingText = Name.transform.GetChild(2).GetComponent<Text>().text;
        Name.transform.GetChild(2).GetComponent<Text>().text = existingText + word;
        Debug.Log(word);
    }
}