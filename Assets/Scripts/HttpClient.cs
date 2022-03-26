using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient
{
    private readonly ISerializationOption _serializationOption;

    public HttpClient(ISerializationOption serializationOption)
    {
        _serializationOption = serializationOption;
    }

    public async Task<TResultType> Get<TResultType>(string url)
    {
        try
        {
            using var www = UnityWebRequest.Get(url);

            www.SetRequestHeader("Content-Type", _serializationOption.ContentType);

            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            var result = _serializationOption.Deserialize<TResultType>(www.downloadHandler.text);

            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
    }

    public async Task<string> Post<TRequestType>(string url, TRequestType postData)
    {
        try
        {
            var request = new UnityWebRequest(url, "POST");
            
            string json = JsonUtility.ToJson(postData);
            
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();
            
            while (!operation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {request.error}");

            return request.downloadHandler.text;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Post)} failed: {ex.Message}");
            return default;
        }
    }
}