using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLog : MonoBehaviour {
   
    public void setText(string myText,Color text_color)
    {
        GetComponent<Text>().text = myText;
        GetComponent<Text>().color = text_color;
    }


}
