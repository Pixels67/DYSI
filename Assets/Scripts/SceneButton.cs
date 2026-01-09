using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class SceneButton : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    private void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ChangeScene);
    }

    private void ChangeScene()
    {
        if (sceneIndex == -1)
        {
            Application.Quit();
        }
        
        SceneManager.LoadScene(sceneIndex);
    }
}