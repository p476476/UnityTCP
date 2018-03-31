using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLogController : MonoBehaviour {

    [SerializeField]
    private GameObject textTemplate;

    private Color text_color;

    private List<GameObject> text_log_list;

    private int MaxLog = 20;
    private void Start()
    {
        text_log_list = new List<GameObject>();
        text_color = Color.white;
    }
    public void LogText(string myString)
    {

        if (text_log_list.Count == MaxLog)
        {
            GameObject temp = text_log_list[0];
            Destroy(text_log_list[0]);
            text_log_list.Remove(temp);
        }

        GameObject new_log = Instantiate(textTemplate) as GameObject;
        new_log.SetActive(true);

        new_log.GetComponentInChildren<TextLog>().setText(myString, text_color);
        new_log.transform.SetParent(textTemplate.transform.parent, false);

        text_log_list.Add(new_log);


    }

}
