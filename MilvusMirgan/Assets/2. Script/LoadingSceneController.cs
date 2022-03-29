using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public static string nextScene;
    public Image[] images;

    [SerializeField] Text LoadingMsg;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());

        DuringWait();
    }


    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        if (op.progress > 0.9f || op.isDone)
            op.allowSceneActivation = true;
        yield break;
    }

    void DuringWait()
    {
        Debug.Log("true");
        if (LoadingMsg.text == "Loading....") LoadingMsg.text = "Loading";
        else LoadingMsg.text += ".";
        Invoke("DuringWait",1f);
    }

}
