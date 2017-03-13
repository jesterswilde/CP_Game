using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class levelswitchtest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}