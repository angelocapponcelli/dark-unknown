using System.Collections;
using Menu;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour {

    private string[] _videogames_names = new string[26]
    {
        "AnotherD",
        "Arrow",
        "Be-Headed",
        "CloudDiver",
        "CursedHeroOfThePast",
        "DarkUnknown",
        "Don'TLetLukeFall",
        "DreamDiary",
        "FeedMe,Quack!",
        "Freshaliens",
        "HarmonyInDarkness",
        "HotPotato",
        "JourneyToAntartica",
        "JustTheTwoOfUs",
        "LaserKnight",
        "NoClip",
        "Paradox!",
        "ParkourNews",
        "PumpDownTheFlame",
        "Reset::Relive",
        "Shade",
        "SleepInvasion",
        "TheFelineParadox",
        "TiedRivals",
        "Zorball",
        ">>Test"
    };

    enum VideoGamesName
    {
        AnotherD=0,
        Arrow=1,
        BeHeaded=2,
        CloudDiver=3,
        CursedHeroOfThePast=4,
        DarkUnknown=5,
        DonTLetLukeFall=6,
        DreamDiary=7,
        FeedMeQuack=8,
        Freshaliens=9,
        HarmonyInDarkness=10,
        HotPotato=11,
        JourneyToAntartica=12,
        JustTheTwoOfUs=13,
        LaserKnight=14,
        NoClip=15,
        Paradox=16,
        ParkourNews=17,
        PumpDownTheFlame=18,
        ResetRelive=19,
        Shade=20,
        SleepInvasion=21,
        TheFelineParadox=22,
        TiedRivals=23,
        Zorball=24,
        Test=25
    };
    
    [SerializeField] private VideoGamesName Videogame;
    [SerializeField] private InputField Feedback;
    
    public void SendFeedback()
    {
        string feedback = Feedback.text;
        StartCoroutine(PostFeedback(_videogames_names[(int) Videogame],feedback));
    }
    
    IEnumerator PostFeedback(string videogame_name, string feedback) 
    {
        string URL =
            "https://docs.google.com/forms/d/e/1FAIpQLSdyQkpRLzqRzADYlLhlGJHwhbKZvKJILo6vGmMfSePJQqlZxA/formResponse";
        
        WWWForm form = new WWWForm();

        form.AddField("entry.631493581", videogame_name);
        form.AddField("entry.1313960569", feedback);

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
        
        // at the end go back to the main menu
        MenuManager.Instance.OpenMainMenu();
    }
}