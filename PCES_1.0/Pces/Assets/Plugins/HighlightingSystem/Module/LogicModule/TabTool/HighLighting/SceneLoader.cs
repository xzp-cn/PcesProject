using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int ox = 20;
    private int oy = 100;

    void OnGUI()
    {
        GUI.Label(new Rect(ox, oy + 10, 500, 100), "Load demo scene:");

        if (GUI.Button(new Rect(ox, oy + 30, 120, 20), "Welcome"))
        {
            //Application.LoadLevel("Welcome");
            SceneManager.LoadScene("Welcome");
        }

        if (GUI.Button(new Rect(ox, oy + 60, 120, 20), "Colors"))
        {
            //Application.LoadLevel("Colors");
            SceneManager.LoadScene("Colors");
        }

        if (GUI.Button(new Rect(ox, oy + 90, 120, 20), "Transparency"))
        {
            //SceneManager.LoadScene("Welcome");
            SceneManager.LoadScene("Welcome");
        }

        if (GUI.Button(new Rect(ox, oy + 120, 120, 20), "Occluders"))
        {
            //Application.LoadLevel("Occluders");
            SceneManager.LoadScene("Occluders");
        }

        if (GUI.Button(new Rect(ox, oy + 150, 120, 20), "Scripting"))
        {
            //Application.LoadLevel("Scripting");
            SceneManager.LoadScene("Scripting");
        }

        if (GUI.Button(new Rect(ox, oy + 180, 120, 20), "Compound"))
        {
            //Application.LoadLevel("Compound");
            SceneManager.LoadScene("Compound");
        }
    }
}