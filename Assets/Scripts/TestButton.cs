using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GameObject.Find("TestButton").GetComponent<Button>();
        button.onClick.AddListener(Test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Test(){
        print("Test");
    }
}
