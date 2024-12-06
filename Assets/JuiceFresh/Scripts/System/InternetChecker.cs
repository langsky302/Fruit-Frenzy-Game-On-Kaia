using System;
using System.Collections;
using UnityEngine;

namespace JuiceFresh.Scripts.System
{
    
    public class InternetChecker : MonoBehaviour
    {
        public static InternetChecker THIS;
        private void Awake()
        {
            if(THIS == null)
            {
                THIS = this;
            }
            else if(THIS != this) Destroy(gameObject);
        }

        public void CheckInternet(bool showPopup, Action<bool> result=null)
        {
            StartCoroutine(_CheckInternet(showPopup, result));
        }
        IEnumerator _CheckInternet(bool showPopup, Action<bool> result=null)
        {
            WWW www = new WWW("http://85.119.150.22/gettime.php");
            yield return www;
            if (www.text == "")
            {
                if(showPopup)                
                    Instantiate(Resources.Load<GameObject>("Popups/NoInternet"), GameObject.Find
                    ("CanvasGlobal").transform);
                result?.Invoke(false);
            }
            else result?.Invoke(true);
        }
    }
}