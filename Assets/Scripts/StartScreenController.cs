using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    [SerializeField]
    private int sceneNo;

    private void Update()
    {
        if (Input.anyKeyDown)
            NextScene();
    }
    public void NextScene()
    {
        SceneManager.LoadScene(sceneNo);
    }
}
