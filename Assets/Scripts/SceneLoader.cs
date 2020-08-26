using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string startScene = "";

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(startScene);
    }
}
