using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public AssetReference _scene;
    public void LoadSceneIfNotLoaded()
    {
        Addressables.LoadResourceLocationsAsync(_scene).Completed += (loc) =>
        {
            var isSceneLoaded = SceneManager.GetSceneByPath(loc.Result[0].InternalId).isLoaded;
            if (!isSceneLoaded)
            {
                _scene.LoadSceneAsync();
            }
        };
    }
}
