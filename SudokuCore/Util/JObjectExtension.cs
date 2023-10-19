using Newtonsoft.Json.Linq;

namespace MokuSakura.Sudoku.Core.Util;

/// <summary>
/// JObject扩展
/// </summary>
public static class JObjectExtensions
{
    /// <summary>
    /// 将JObject转化成字典
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static IDictionary<string, object> ToDictionary(this JObject json)
    {
        Dictionary<string, object>? propertyValuePairs = json.ToObject<Dictionary<string, object>>();
        if (propertyValuePairs == null)
        {
            throw new ArgumentException($"Cannot convert {json} to Dictionary");
        }

        ProcessJObjectProperties(propertyValuePairs);
        ProcessJArrayProperties(propertyValuePairs);
        return propertyValuePairs;
    }

    private static void ProcessJObjectProperties(IDictionary<string, object> propertyValuePairs)
    {
        var objectPropertyNames = (from property in propertyValuePairs
            let propertyName = property.Key
            let value = property.Value
            where value is JObject
            select propertyName).ToList();

        objectPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToDictionary((JObject)propertyValuePairs[propertyName]));
    }

    private static void ProcessJArrayProperties(IDictionary<string, object> propertyValuePairs)
    {
        var arrayPropertyNames = (from property in propertyValuePairs
            let propertyName = property.Key
            let value = property.Value
            where value is JArray
            select propertyName).ToList();

        arrayPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToArray((JArray)propertyValuePairs[propertyName]));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static object[] ToArray(this JArray array)
    {
        return array.ToObject<object[]>()!.Select(ProcessArrayEntry).ToArray();
    }

    private static object ProcessArrayEntry(object value)
    {
        if (value is JObject jObject)
        {
            return ToDictionary(jObject);
        }

        if (value is JArray array)
        {
            return ToArray(array);
        }

        return value;
    }
}