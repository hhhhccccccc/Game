using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using YooAsset;
using Object = UnityEngine.Object;

public class ResourceManager : ManagerBase, IResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return YooAssets.LoadAssetSync<T>(path).GetAssetObject<T>();
    }

    public IEnumerator<T> LoadParallel<T>(string path) where T : Object
    {
        var handle = YooAssets.LoadAssetAsync<T>(path);
        yield return handle.AssetObject as T;
    }

    public async Task<T> LoadAsync<T>(string path) where T : Object
    {
        var handle = YooAssets.LoadAssetAsync<T>(path);
        await handle.Task;
        return handle.AssetObject as T;
    }

    public void Unload(string packageName)
    {
        var package = YooAssets.TryGetPackage(packageName);
        package?.UnloadUnusedAssetsAsync();
    }

    protected override IEnumerator OnInit()
    {
        return base.OnInit();
    }
}
