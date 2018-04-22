using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;

public class GameRecorder : MonoBehaviour {

    string addActionToTable = "http://web.ist.utl.pt/ist165821/AddActionToTable.php";
    string getTable = "http://web.ist.utl.pt/ist165821/DisplayTable.php";
    string createTable = "http://web.ist.utl.pt/ist165821/CreateTable.php";
    string getAllTables = "http://web.ist.utl.pt/ist165821/GetAllTables.php";

    public List<string> tableNames;

    public List<PlayerMovementController.Action> tableActions;

    private void Start()
    {
        tableActions = new List<PlayerMovementController.Action>();
        //StartCoroutine(runActions());
        //StartCoroutine(CreateTable("Mordokay3"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
           //StartCoroutine(runActions());
        }
    }
    IEnumerator runActions()
    {
        //yield return StartCoroutine(CreateTable("Mordokay3"));

        yield return StartCoroutine(PostScores("Mordokay3", "t", "0"));
        yield return StartCoroutine(PostScores("Mordokay3", "r", "1"));
        yield return StartCoroutine(PostScores("Mordokay3", "t", "1"));
        yield return StartCoroutine(PostScores("Mordokay3", "b", "1"));
        yield return StartCoroutine(PostScores("Mordokay3", "t", "0"));
        yield return StartCoroutine(PostScores("Mordokay3", "l", "1"));
        yield return StartCoroutine(PostScores("Mordokay3", "t", "0"));
        yield return StartCoroutine(PostScores("Mordokay3", "f", "1"));
    }

    public IEnumerator PostScores(string tableName, string action, string value)
    {
        string post_url = addActionToTable + "?actionName=\"" + WWW.EscapeURL(action) + "\"&actionValue=" + WWW.EscapeURL(value) +
            "&name=" + WWW.EscapeURL(tableName);

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done
    }

    public IEnumerator GetAllTables()
    {
        string post_url = getAllTables;

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        string auxResult = hs_post.text.Substring( 0, hs_post.text.Length - 1);
        string[] names = auxResult.Split(' ');
        for(int i = 0; i < names.Length; i++)
        {
            tableNames.Add(names[i]);
            //Debug.Log(i + " : " + tableNames[i]);
        }
    }

    public IEnumerator CreateTable(string name)
    {
        string post_url = createTable + "?name=" + WWW.EscapeURL(name.ToString());

        WWW hs_post = new WWW(post_url);
        yield return hs_post;

        Debug.Log(hs_post.text);
    }

    //Gets the table data and converts it to an Actions List

    public IEnumerator GetTable(string tableName)
    {
        //Debug.Log("tableName: " + tableName);
        string post_url = getTable + "?name=" + WWW.EscapeURL(tableName);
        WWW hs_get = new WWW(post_url);
        yield return hs_get;
        //Debug.Log(hs_get.text);
        ParseActions(hs_get.text.Substring(0, hs_get.text.Length - 1));
        //Debug.Log(hs_get.text);
    }

    void ParseActions(string actions)
    {
        if (actions != null)
        {
            string[] myActions = actions.Split('|');
            for (int i = 0; i < myActions.Length; i++)
            {
                string[] actionData = myActions[i].Split(' ');
                tableActions.Add(new PlayerMovementController.Action(actionData[0].ToString(), float.Parse(actionData[1])));
            }
        }
    }
}
