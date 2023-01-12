using System.Collections;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ExampleCache : MonoBehaviour
{
    IEnumerator DownloadAndCacheAssetBundle(string uri, string manifestBundlePath)
    {
        //Load the manifest
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(manifestBundlePath);
        AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //Create new cache
        string today = DateTime.Today.ToLongDateString();
        Directory.CreateDirectory(today);
        Cache newCache = Caching.AddCache(today);

        //Set current cache for writing to the new cache if the cache is valid
        if (newCache.valid)
            Caching.currentCacheForWriting = newCache;

        //Download the bundle
        Hash128 hash = manifest.GetAssetBundleHash("bundleName");
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri, hash, 0);
        yield return request.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

        //Get all the cached versions
        List<Hash128> listOfCachedVersions = new List<Hash128>();
        Caching.GetCachedVersions(bundle.name, listOfCachedVersions);

        if (!AssetBundleContainsAssetIWantToLoad(bundle))     //Or any conditions you want to check on your new asset bundle
        {
            //If our criteria wasn't met, we can remove the new cache and revert back to the most recent one
            Caching.currentCacheForWriting = Caching.GetCacheAt(Caching.cacheCount);
            Caching.RemoveCache(newCache);

            for (int i = listOfCachedVersions.Count - 1; i > 0; i--)
            {
                //Load a different bundle from a different cache
                request = UnityWebRequestAssetBundle.GetAssetBundle(uri, listOfCachedVersions[i], 0);
                yield return request.SendWebRequest();
                bundle = DownloadHandlerAssetBundle.GetContent(request);

                //Check and see if the newly loaded bundle from the cache meets your criteria
                if (AssetBundleContainsAssetIWantToLoad(bundle))
                    break;
            }
        }
        else
        {
            //This is if we only want to keep 5 local caches at any time
            if (Caching.cacheCount > 5)
                Caching.RemoveCache(Caching.GetCacheAt(1));     //Removes the oldest user created cache
        }
    }

    bool AssetBundleContainsAssetIWantToLoad(AssetBundle bundle)
    {
        return (bundle.LoadAsset<GameObject>("MyAsset") != null);     //this could be any conditional
    }
}