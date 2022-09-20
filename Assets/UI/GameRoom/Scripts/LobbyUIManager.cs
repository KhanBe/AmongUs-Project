using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager Instance;

    [SerializeField]
    private CustomizeUI customizeUI;

    public CustomizeUI CustomizeUI { get { return customizeUI; } }

    private void Awake()
    {
        Instance = this;
    }

}
