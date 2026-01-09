using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTimer : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    [SerializeField] private float timeSeconds = 30.0f;

    private float _timer = 0.0f;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= timeSeconds)
        {
            _timer = 0.0f;
            ChangeScene();
        }
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
