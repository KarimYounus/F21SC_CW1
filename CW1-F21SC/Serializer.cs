using System;
using System.IO;
using System.Text.Json;

namespace CW1_F21SC;


/// <summary>
/// This class is responsible for serializing and deserializing the user settings, bookmarks and history to and from JSON files.
/// </summary>
public static class Serializer
{
    public static void SerializeFile<T>(T obj, string fileName)
    {
        if (!SupportedType(obj)) 
            throw new Exception("Invalid object type");

        var jsonString = JsonSerializer.Serialize(obj); //Serialize the object into a JSON string
        File.WriteAllText(fileName, jsonString); //Write the JSON string to the file
    } 
    
    public static object DeserializeFile<T>(T obj, string fileName)
    {
        var jsonString = File.ReadAllText(fileName); //Read the JSON string from the file

        if (!SupportedType(obj)) 
            throw new Exception("Invalid object type");
        
        return JsonSerializer.Deserialize<T>(jsonString); //Deserialize the JSON string into an object
    }
    
    // Method to check if the type is supported
    private static bool SupportedType<T>(T obj)
    {
        return typeof(T) == typeof(UserSettings) || typeof(T) == typeof(UserBookmarks) || typeof(T) == typeof(UserHistory);
    }
    
}
