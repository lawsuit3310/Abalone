using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] NetWrkErr;
    public Text startMsg;
    [SerializeField] LoadingSceneController LoadingSceneController;
    void Start()
    {
        EnableObject(NetWrkErr,false);
    }

    // Update is called once per frame

    public void OnClick()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)//인터넷이 안될 때
        {
            EnableObject(NetWrkErr,true);
            startMsg.gameObject.SetActive(false);
            return;
            
        }
        else //연결 되었을 때
        {
            LoadingSceneController.LoadScene("Game");
            
        }
    }

    public void EnableObject(GameObject[] objects, bool status)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(status);
        }
    }

    public void ErrClose()
    {
        EnableObject(NetWrkErr,false);
        startMsg.gameObject.SetActive(true);
    }
}
