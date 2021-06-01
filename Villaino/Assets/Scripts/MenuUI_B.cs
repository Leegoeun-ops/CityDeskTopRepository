using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuUI_B : MonoBehaviour
{
    [SerializeField]
    private Button button;
    //[SerializeField]
    //private Text text;
    public GameObject text;
    public GameObject switchUI;
    //public void Camappear5Sec()
    //{
    //    playerCam.SetActive(true);
    //    Destroy(cityview, 5f);
    //}
    // Start is called before the first frame update
    void Start()
    {
        Invoke("disappear5Sec", 5f);
        //Destroy(text, 15f);
        Invoke("appear10Sec", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void appear10Sec()
    {
        switchUI.SetActive(true);
        Destroy(switchUI, 10f);
    }
    public void disappear5Sec()
    {
        text.SetActive(true);
        Destroy(text, 5f);
    }
}
