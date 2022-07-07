using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObbDownloader : MonoBehaviour
{
   public string downloadURL;
   public string fileName;
   public GameObject downloadShowObject;
   public Text textProgress;
   public Image imageProgress;
   public string packageName;
   
   public void StartDownload()
   { 
      if (PlayerPrefs.GetInt("DownloadDataCompleted") == 1)
      { 
         GoNextLevel();
         return;
      }
   
      StartCoroutine(StartDownloadIE());
   }
   private IEnumerator StartDownloadIE()
   {
      var uwr = new UnityWebRequest( downloadURL, UnityWebRequest.kHttpVerbGET);

      var obbFolder = "/storage/emulated/0/Android/obb/" + packageName;
      if (!Directory.Exists(obbFolder))
         Directory.CreateDirectory(obbFolder);
      
      var path = Path.Combine(obbFolder, fileName);
      if (File.Exists(path))
      {
         File.Delete(path);
      }
      uwr.downloadHandler = new DownloadHandlerFile(path);
      uwr.SendWebRequest();
      downloadShowObject.SetActive(true);
      while (!uwr.isDone)
      {
         if (uwr.downloadProgress <= 0)
         {
            textProgress.text = " Starting Downloading Game Data !";
            yield return null;
         }
         else
         {
            textProgress.text = "Downloading: "  + ((int)(uwr.downloadProgress * 100f)) + "%";
         } 
         imageProgress.fillAmount = uwr.downloadProgress;
         yield return null;
      }
      if (uwr.result != UnityWebRequest.Result.Success)
         Debug.LogError(uwr.error);
      else
      {
         textProgress.text = "Download Completed !";
         Debug.Log("File successfully downloaded and saved to " + path);
         PlayerPrefs.SetInt("DownloadDataCompleted" , 1);
         
         using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
         {
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var intent = currentActivity.Call<AndroidJavaObject>("getIntent");
            var intentFlagActivityNoAnimation = 0x10000;
            intent.Call<AndroidJavaObject>("addFlags", intentFlagActivityNoAnimation);
            currentActivity.Call("startActivity", intent);
            if (AndroidJNI.ExceptionOccurred() != System.IntPtr.Zero)
            {
               AndroidJNI.ExceptionDescribe();
               AndroidJNI.ExceptionClear();
            }
         }
         
         Invoke(nameof(GoNextLevel) , 0.5f);
      }

   }
   public void GoNextLevel()
   {
      SceneManager.LoadScene("Scene1");
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Space))
      {
         PlayerPrefs.DeleteAll();         
      }
   }
}
