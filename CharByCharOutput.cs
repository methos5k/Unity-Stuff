using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharByCharOutput : MonoBehaviour {
    public InputField inputField;
    public Text outText;
    const float outRate = 1f / 40f;
    WaitForSeconds wfs = new WaitForSeconds(outRate);
    List<string> outBuffer = new List<string>();

    void Start()
    {
        outText.text = "";
        StartCoroutine(ShowText());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(inputField.text != "")
            {
                outBuffer.Add("Msg: " + inputField.text + "\n");
                inputField.text = "";
                inputField.Select();
                inputField.ActivateInputField();
            }
        }
    }
    IEnumerator ShowText()
    {
        while (true)
        {
            if (outBuffer.Count > 0)
            {
                string s = outBuffer[0];
                for (int i = 0; i < s.Length; ++i)
                {
                    outText.text += s[i];
                    yield return wfs;
                }
                outBuffer.RemoveAt(0);
            }
            else yield return wfs;
        }
    }
}
