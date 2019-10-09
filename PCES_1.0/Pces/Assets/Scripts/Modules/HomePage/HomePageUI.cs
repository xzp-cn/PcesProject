using UnityEngine;

public class HomePageUI : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //transform.Find("describe").GetComponentInChildren<Text>().text;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        Debug.Log("HomePageUI  ");
    }
}
