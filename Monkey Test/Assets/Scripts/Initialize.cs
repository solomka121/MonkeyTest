using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initialize : MonoBehaviour
{
    private List<Iinitialize> _initializableList = new List<Iinitialize>();

    private void Awake()
    {
        var scene = SceneManager.GetActiveScene();
        var rootObjects = scene.GetRootGameObjects(); ;

        foreach (var obj in rootObjects)
            _initializableList.AddRange(obj.GetComponentsInChildren<Iinitialize>(true));
        foreach (Iinitialize Iinitializable in _initializableList)
            Iinitializable.Init();
    }
}
