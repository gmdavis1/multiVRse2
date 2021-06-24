using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Keeps track of the previous scene.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null && IsApplicationQuitting == false)
            {
                _instance = FindObjectOfType<SceneLoader>();
                if (_instance == null && IsApplicationQuitting == false)
                {
                    GameObject newObj = new GameObject("SceneLoader");
                    _instance = newObj.AddComponent<SceneLoader>();

                    DontDestroyOnLoad(_instance.gameObject);
                }
            }

            return _instance;
        }
    }

    public static bool HasInstance => (_instance != null);

    private static SceneLoader _instance = null;

    private static bool IsApplicationQuitting = false;

    /// <summary>
    /// The previous scene loaded.
    /// </summary>
    public string PrevScene { get; private set; } = string.Empty;

    private void Awake()
    {
        //Ensure only one instance
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        IsApplicationQuitting = true;
    }

    public void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
    {
        //Store previous scene
        PrevScene = SceneManager.GetActiveScene().name;

        //Load new scene
        SceneManager.LoadScene(sceneName, loadSceneMode);
    }
}
