using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public static SceneLoader SceneLoader { get; private set; }
    public static Dictionary<string, object> Data { get; private set; } = new Dictionary<string, object>();

    void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        SceneLoader.LoadStartScene();
    }

    private void Initialize()
    {
        if (Instance)
            return;
        Instance = this;
        SceneLoader = GetComponent<SceneLoader>(); 
    }
}
