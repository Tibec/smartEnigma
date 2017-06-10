using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Utils
{
    private static System.Random random = new System.Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static void LoadScene(string name)
    {
        SceneLoader loader = GameObject.FindObjectOfType<SceneLoader>();
        Assert.IsNotNull(loader, "Cannot found SceneLoader !");
        loader.LoadScene(name);
    }
}
