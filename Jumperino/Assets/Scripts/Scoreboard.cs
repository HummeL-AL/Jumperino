using System.Linq;
using System.Collections;
using Firebase.Database;
using UnityEngine;
using TMPro;
using static Global;
using static PlayerData;

public class Scoreboard : MonoBehaviour
{
    public static int choosedPanel = 0;

    public GameObject linePrefab;
    public static Transform scoreboardTable;

    private void Awake()
    {
        scoreboardTable = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0);
    }

    public void InitializeScoreboard()
    {
        if(IsConnected())
        {
            transform.GetChild(0).gameObject.SetActive(false);

            if(!_nicknameEntered)
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(1).gameObject.SetActive(false);
                UpdateScoreboard();
            }
        }
    }

    public void UpdateScoreboard()
    {
        switch(choosedPanel)
        {
            case 0:
                {
                    StartCoroutine(SetScoreboardTable("High score"));
                    break;
                }
            case 1:
                {
                    StartCoroutine(SetScoreboardTable("Total jumps"));
                    break;
                }
            case 2:
                {
                    StartCoroutine(SetScoreboardTable("Total coins"));
                    break;
                }
        }
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public IEnumerator SetScoreboardTable(string sortValue)
    {
        DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;

        var DBTask = root.Child("Devices").OrderByChild(sortValue).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        DataSnapshot snap = DBTask.Result;
        foreach(Transform child in scoreboardTable)
        {
            Destroy(child.gameObject);
        }

        GameObject createdLine = Instantiate(linePrefab, scoreboardTable);
        createdLine.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "NICKNAME";
        createdLine.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = sortValue;

        foreach (DataSnapshot childSnapshot in snap.Children.Reverse<DataSnapshot>())
        {
            createdLine = Instantiate(linePrefab, scoreboardTable);

            Debug.Log(childSnapshot.Child("Nickname"));
            createdLine.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = childSnapshot.Child("Nickname").Value.ToString();
            createdLine.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = childSnapshot.Child(sortValue).Value.ToString();
        }
    }

    public void ChangePanel(int changeTo)
    {
        choosedPanel = changeTo;
        UpdateScoreboard();
    }
}
