using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IResourceManager : IManager
{
    T Load<T>(string path) where T : Object;
    IEnumerator<T> LoadParallel<T>(string path) where T : Object;
    Task<T> LoadAsync<T>(string path) where T : Object;
    void Unload(string packageName);
}
