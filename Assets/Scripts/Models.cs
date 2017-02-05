using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public string name;
    public string uuid; //index
}

[Serializable]
public class LevelScore
{
    public string player_uuid; //index
    public string level_number;
    public float score;

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["player_uuid"] = player_uuid;
        result["level_number"] = level_number;
        result["score"] = score;

        return result;
    }
}

public class PlayerPrefSrings
{
    public readonly static string player_data = "player_data";
}
