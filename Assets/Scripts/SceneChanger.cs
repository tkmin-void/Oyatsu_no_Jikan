using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("Play");

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }  
}
