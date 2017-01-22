using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundProfileBehaviour : MonoBehaviour
{
    public AudioClip GetRandomClip(List<AudioClip> list)
    {
        return list.GetRandom();
    }
}


//Yeah, this should be somewhere else..
public static class ListExtensions
{
    public static T GetRandom<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
            return default(T);
        int i = Random.Range(0, list.Count);
        return list[i];
    }
}