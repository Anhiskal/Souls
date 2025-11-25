using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;
    [SerializeField] int wordSceneIndex = 1;

    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartLoadNewGame() 
    {
        StartCoroutine(LoadNewGame());
    }

    public IEnumerator LoadNewGame() 
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(wordSceneIndex);
        yield return null;
    }

    public int GetWorlfSceneIndex() { return wordSceneIndex; }
}
