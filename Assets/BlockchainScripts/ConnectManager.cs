using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectManager : MonoBehaviour
{
    public void ChangeToScenePlay()
    {
        SceneManager.LoadScene("ShopScene");
    }
}
