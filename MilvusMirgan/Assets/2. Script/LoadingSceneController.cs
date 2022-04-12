using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public static string nextScene;
    public Sprite[] images;

    [SerializeField] Text LoadingMsg;
    [SerializeField] Image Dancer;
    private int count = 0;
    private AsyncOperation op;
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
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            yield return null;
            Debug.Log(op.progress);
            if (op.progress >= 0.9f)
                Invoke("GoNextScene",1f);
        }
        yield break;
    }

    void GoNextScene()
    {
        Debug.Log(true);
        op.allowSceneActivation = true;
    }

    void DuringWait()
    {
        if (LoadingMsg.text == "Loading....") LoadingMsg.text = "Loading";
        else LoadingMsg.text += ".";
        Dancer.sprite = images[count%4];
        count++;
        Invoke("DuringWait",0.2f);
    }

}
