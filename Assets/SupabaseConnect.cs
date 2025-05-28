using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Thoughts
{
    public int id;
    public string created_at;
    public int level_id;
    public string thought;
}

public class SupabaseConnect : MonoBehaviour
{
    const string supabaseUrl = "https://ucsbfndaebyymdpyfcyp.supabase.co";
    const string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVjc2JmbmRhZWJ5eW1kcHlmY3lwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDU2Nzk0MjUsImV4cCI6MjA2MTI1NTQyNX0.-25EfDw8_d9i_uqRFTwYJCQ4eRJObAJf0Kb0E6Ff284";

    // void Start()
    // {
    //     // First add a thought, then retrieve thoughts for level -1
    //     StartCoroutine(AddThought("This is a test thought from Unity", -1));

    //     // Get thoughts with a callback to demonstrate how to use the returned array
    //     StartCoroutine(GetThoughtsByLevel(-1, OnThoughtsReceived));
    // }

    // Example callback function to process the thoughts array
    void OnThoughtsReceived(Thoughts[] thoughts)
    {
        if (thoughts != null)
        {
            Debug.Log($"Callback received {thoughts.Length} thoughts");
            // You can now use the thoughts array here
            // For example, populate a UI list, process the data, etc.
        }
        else
        {
            Debug.LogWarning("Callback received null thoughts array");
        }
    }

    public static IEnumerator AddThought(string thoughtText, int levelId)
    {
        Debug.Log($"Adding thought: {thoughtText} for level ID: {levelId}");

        // Create the JSON payload
        string jsonPayload = JsonUtility.ToJson(new ThoughtPayload
        {
            thought = thoughtText,
            level_id = levelId
        });

        Debug.Log($"JSON payload: {jsonPayload}");

        // Set up the POST request to Supabase
        string endpoint = $"{supabaseUrl}/rest/v1/thoughts";
        UnityWebRequest request = new UnityWebRequest(endpoint, "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonPayload));
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set headers
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Prefer", "return=representation"); // This tells Supabase to return the created record

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error adding thought: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}");
        }
        else
        {
            Debug.Log($"Thought added successfully. Response: {request.downloadHandler.text}");

            try
            {
                // Parse the response to get the created thought
                // Note: Supabase returns an array with a single object
                string rawJson = request.downloadHandler.text;
                if (rawJson.StartsWith("[") && rawJson.EndsWith("]"))
                {
                    // Remove the array brackets to get a single object
                    rawJson = rawJson.Substring(1, rawJson.Length - 2);
                }

                Thoughts createdThought = JsonUtility.FromJson<Thoughts>(rawJson);
                Debug.Log($"Created thought with ID: {createdThought.id}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error parsing response: {e.Message}");
            }
        }
    }

    // Helper class for creating the JSON payload
    [System.Serializable]
    private class ThoughtPayload
    {
        public string thought;
        public int level_id;
    }

    // Class to hold a list of thoughts for JSON deserialization
    [System.Serializable]
    private class ThoughtList
    {
        public Thoughts[] thoughts;
    }

    // Define a delegate type for the callback
    public delegate void ThoughtsCallback(Thoughts[] thoughts);

    // Modified function to accept a callback and return the thoughts array
    public static IEnumerator GetThoughtsByLevel(int levelId, ThoughtsCallback callback = null)
    {
        Debug.Log($"Getting thoughts for level ID: {levelId}");

        // Use the correct PostgREST filter format with eq operator
        string endpoint = $"{supabaseUrl}/rest/v1/thoughts?level_id=eq.{levelId}&select=*";
        UnityWebRequest request = UnityWebRequest.Get(endpoint);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Thoughts[] thoughts = null; // Initialize return value

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error getting thoughts: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}");
        }
        else
        {
            try
            {
                string rawJson = request.downloadHandler.text;
                Debug.Log($"Raw Thoughts JSON: {rawJson}");

                // Wrap the JSON array in an object for JsonUtility to parse
                string wrappedJson = "{\"thoughts\":" + rawJson + "}";
                ThoughtList thoughtList = JsonUtility.FromJson<ThoughtList>(wrappedJson);

                if (thoughtList != null && thoughtList.thoughts != null)
                {
                    // Store the thoughts array for return
                    thoughts = thoughtList.thoughts;

                    Debug.Log($"Found {thoughts.Length} thoughts for level {levelId}");
                    foreach (Thoughts thought in thoughts)
                    {
                        Debug.Log($"Thought ID: {thought.id}");
                        Debug.Log($"Created At: {thought.created_at}");
                        Debug.Log($"Level ID: {thought.level_id}");
                        Debug.Log($"Thought: {thought.thought}");
                        Debug.Log("===================================");
                    }
                }
                else
                {
                    Debug.LogWarning($"No thoughts found for level {levelId} or parsing failed");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error parsing JSON: {e.Message}");
            }
        }

        // If a callback was provided, invoke it with the thoughts array
        if (callback != null)
        {
            callback(thoughts);
        }
    }
}
