using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class lightleveltest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}