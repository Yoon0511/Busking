using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class JSon
{

    public static string Write(Dictionary<string, string> dic)
    {
        return JsonWriter.Serialize(dic);
    }

    public static Dictionary<string, string> Read(string json)
    {
        return JsonReader.Deserialize<Dictionary<string, string>>(json);
    }
}
