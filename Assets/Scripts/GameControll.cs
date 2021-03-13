using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControll : MonoBehaviour
{
    private static GameControll instance;
    public static GameControll Instance { get => instance; set => instance = value; }

    List<Transform> guys;

    [SerializeField]
    Transform starterCharacter;

    bool ended;
    public bool Ended { get => ended; set => ended = value; }

    void Awake()
    {
        if (instance == null)
            instance = this;
        ended = false;
        guys = new List<Transform>();
        guys.Add(starterCharacter);
    }

    void Update()
    {
        transform.position = GetAvaragePosition();
    }

    private Vector3 GetAvaragePosition()
    {
        Vector3 vec = Vector3.zero;
        for (int i = 0; i < guys.Count; i++)
            vec += guys[i].position;
        return vec / guys.Count + new Vector3(15, 9, 0);
    }

    public void JoinedToCrew(Transform newGuy)
    {
        guys.Add(newGuy);
        UIController.Instance.SetGuysCount(GetGuysCount());
    }
    public void LeftFromCrew(Transform buriedGuy)
    {
        guys.Remove(buriedGuy);
        Destroy(buriedGuy.gameObject);
        UIController.Instance.SetGuysCount(GetGuysCount());
    }

    internal int GetGuysCount()
    {
        return guys.Count;
    }
}
