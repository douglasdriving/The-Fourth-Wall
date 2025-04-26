using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class Question
{
    public int id;
    public string created_at;
    public string text;
}

[System.Serializable]
public class QuestionList
{
    public Question[] questions;
}

[System.Serializable]
public class Answer
{
    public int id;
    public string created_at;
    public string text;
    public int votes;
    public int question_id;
}

[System.Serializable]
public class AnswerList
{
    public Answer[] answers;
}


public class SupabaseConnect : MonoBehaviour
{
    string supabaseUrl = "https://ucsbfndaebyymdpyfcyp.supabase.co";
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVjc2JmbmRhZWJ5eW1kcHlmY3lwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDU2Nzk0MjUsImV4cCI6MjA2MTI1NTQyNX0.-25EfDw8_d9i_uqRFTwYJCQ4eRJObAJf0Kb0E6Ff284";

    void Start()
    {
        StartCoroutine(GetAnswers(1));
    }

    IEnumerator TestConnection()
    {

        Debug.Log("Connecting to Supabase...");

        string endpoint = $"{supabaseUrl}/rest/v1/questions";
        UnityWebRequest request = UnityWebRequest.Get(endpoint);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            string rawJson = request.downloadHandler.text;
            string wrappedJson = "{\"questions\":" + rawJson + "}";
            QuestionList questionList = JsonUtility.FromJson<QuestionList>(wrappedJson);
            foreach (Question question in questionList.questions)
            {
                Debug.Log("Question ID: " + question.id);
                Debug.Log("Created At: " + question.created_at);
                Debug.Log("Text: " + question.text);
                Debug.Log("===================================");
            }
            Debug.Log("Connection successful!");
        }
    }

    IEnumerator GetAnswers(int question_id)
    {

        Debug.Log("Getting answers for question ID: " + question_id);

        string endpoint = $"{supabaseUrl}/rest/v1/answers?question_id={question_id}&select=id,text";
        UnityWebRequest request = UnityWebRequest.Get(endpoint);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        request.SetRequestHeader("apikey", supabaseKey);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            string rawJson = request.downloadHandler.text;
            string wrappedJson = "{\"answers\":" + rawJson + "}";
            AnswerList answerList = JsonUtility.FromJson<AnswerList>(wrappedJson);
            foreach (Answer answer in answerList.answers)
            {
                Debug.Log("Answer ID: " + answer.id);
                Debug.Log("Created At: " + answer.created_at);
                Debug.Log("Text: " + answer.text);
                Debug.Log("===================================");
            }
        }

    }

}
