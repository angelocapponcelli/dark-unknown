using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SendToOurGoogle : MonoBehaviour {
    
    [SerializeField] private InputField _like;
    [SerializeField] private InputField _dislike;
    [SerializeField] private Slider _likingGrade;
    [SerializeField] private InputField _changes;
    [SerializeField] private InputField _bugReport;
    
    public void SendFeedback()
    {
        string like = _like.text;
        string dislike = _dislike.text;
        int likingGrade = (int) _likingGrade.value; 
        string changes = _changes.text;
        string bugReport = _bugReport.text;
        StartCoroutine(PostFeedback(like, dislike, likingGrade, changes, bugReport));
    }
    
    IEnumerator PostFeedback(string like, string dislike, int likingGrade,  string changes, string bugReport) 
    {
        string URL =
            "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdSsVGbdWbe3MWd3c7QOjVaiVojSCH5jGvPaa1zKFCdm31FnQ/formResponse";
        
        WWWForm form = new WWWForm();

        form.AddField("entry.81966288", like);
        form.AddField("entry.904612865", dislike);
        form.AddField("entry.61401012", likingGrade);
        form.AddField("entry.921904117", changes);
        form.AddField("entry.1955456742", bugReport);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

        print(www.error);
        
        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
        www.Dispose();
        // at the end go back to the main menu
        MenuManager.Instance.OpenMainMenu();
    }
}