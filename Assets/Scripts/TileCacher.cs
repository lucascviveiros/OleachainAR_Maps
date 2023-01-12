using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UI;

public class TileCacher : MonoBehaviour
{
    public static TileCacher current;

    public enum Status
    {
        ALL_CACHED,
        SOME_CACHED,
        ALL_FALIED,
        NOTHING_TO_CAHCE
    }
    public delegate void TileCacherEvent(Status result, int FetchedTileCount);

    [Header("Area Data")]
    public List<string> Points;
    public string ImageMapId;
    public int ZoomLevel;

    [Header("Output")]
    public float Progress;
    [TextArea(10, 20)]
    public string Log;
    private ImageDataFetcher ImageFetcher;
    private int _tileCountToFetch;
    private int _failedTileCount;
    [SerializeField] private int _currentProgress;
    private Vector2 _anchor;
    [SerializeField] private Transform _canvas;
    [SerializeField] bool DoesLog = false;
    [SerializeField] bool DoesRender = false;
    [SerializeField] Image progressBarImage;
    public event TileCacherEvent OnTileCachingEnd;

    void Awake() { current = this; }
    private void Start()
    {
        //ImageFetcher = new ImageDataFetcher();
        ImageFetcher = ScriptableObject.CreateInstance<ImageDataFetcher>();
        ImageFetcher.DataRecieved += ImageDataReceived;
        ImageFetcher.FetchingError += ImageDataError;
    }

    public void CacheTiles(int _zoomLevel, string _topLeft, string _bottomRight)
    {
        ZoomLevel = _zoomLevel;
        Points = new List<string>();
        Points.Add(_topLeft);
        Points.Add(_bottomRight);
        PullTiles();
    }


    [ContextMenu("Download Tiles")]
    public void PullTiles()
    {
        Progress = 0;
        _tileCountToFetch = 0;
        _currentProgress = 0;
        _failedTileCount = 0;

        var pointMeters = new List<UnwrappedTileId>();
        foreach (var point in Points)
        {
            var pointVector = Conversions.StringToLatLon(point);
            var pointMeter = Conversions.LatitudeLongitudeToTileId(pointVector.x, pointVector.y, ZoomLevel);
            pointMeters.Add(pointMeter);
        }

        var minx = int.MaxValue;
        var maxx = int.MinValue;
        var miny = int.MaxValue;
        var maxy = int.MinValue;

        foreach (var meter in pointMeters)
        {
            if (meter.X < minx)
            {
                minx = meter.X;
            }

            if (meter.X > maxx)
            {
                maxx = meter.X;
            }

            if (meter.Y < miny)
            {
                miny = meter.Y;
            }

            if (meter.Y > maxy)
            {
                maxy = meter.Y;
            }
        }

        // If there is only one tile to fetch, this makes sure you fetch it
        if (maxx == minx)
        {
            maxx++;
            minx--;
        }

        if (maxy == miny)
        {
            maxy++;
            miny--;
        }

        _tileCountToFetch = (maxx - minx) * (maxy - miny);
        if (_tileCountToFetch == 0)
        {
            OnTileCachingEnd.Invoke(Status.NOTHING_TO_CAHCE, 0);
        }
        else
        {
            _anchor = new Vector2((maxx + minx) / 2, (maxy + miny) / 2);
            PrintLog(string.Format("{0}, {1}, {2}, {3}", minx, maxx, miny, maxy));
            StartCoroutine(StartPulling(minx, maxx, miny, maxy));
        }
    }

    private IEnumerator StartPulling(int minx, int maxx, int miny, int maxy)
    {

        for (int i = minx; i < maxx; i++)
        {
            for (int j = miny; j < maxy; j++)
            {

                ImageFetcher.FetchData(new ImageDataFetcherParameters()
                {
                    canonicalTileId = new CanonicalTileId(ZoomLevel, i, j),
                    mapid = ImageMapId,
                    tile = null
                });

                yield return null;
            }
        }
    }

    #region Fetcher Events

    private void ImageDataError(UnityTile arg1, RasterTile arg2, TileErrorEventArgs arg3)
    {
        PrintLog(string.Format("Image data fetching failed for {0}\r\n", arg2.Id));
        _failedTileCount++;
    }

    private void ImageDataReceived(UnityTile arg1, RasterTile arg2)
    {
        _currentProgress++;
        Progress = (float)_currentProgress / _tileCountToFetch * 100;
        if (progressBarImage != null && progressBarImage.gameObject.activeInHierarchy) progressBarImage.fillAmount = Progress / 100;
        RenderImagery(arg2);
        if (Progress == 100) CheckEnd();
    }
    #endregion

    #region Utility Functions
    private void CheckEnd()
    {
        if (OnTileCachingEnd != null)
        {
            if (_failedTileCount == 0)
            {
                OnTileCachingEnd.Invoke(Status.ALL_CACHED, _tileCountToFetch);
            }
            else if (_failedTileCount == _tileCountToFetch)
            {
                OnTileCachingEnd.Invoke(Status.ALL_FALIED, 0);
            }
            else if (_failedTileCount > 0 && _failedTileCount < _tileCountToFetch)
            {
                OnTileCachingEnd.Invoke(Status.SOME_CACHED, _tileCountToFetch - _failedTileCount);
            }
        }
    }
    private void RenderImagery(RasterTile rasterTile)
    {
        if (!DoesRender || _canvas == null || !_canvas.gameObject.activeInHierarchy) return;

        GameObject targetCanvas = GameObject.Find("canvas_" + ZoomLevel);
        if (targetCanvas == null)
        {
            targetCanvas = new GameObject("canvas_" + ZoomLevel);
            targetCanvas.transform.SetParent(_canvas);
        }

        var go = new GameObject("image");
        go.transform.SetParent(targetCanvas.transform);
        var img = go.AddComponent<RawImage>();
        img.rectTransform.sizeDelta = new Vector2(10, 10);
        var txt = new Texture2D(256, 256);
        txt.LoadImage(rasterTile.Data);
        img.texture = txt;
        (go.transform as RectTransform).anchoredPosition = new Vector2((float)(rasterTile.Id.X - _anchor.x) * 10, (float)-(rasterTile.Id.Y - _anchor.y) * 10);
    }
    private void PrintLog(string message)
    {
        if (!DoesLog) return;
        Log += message;
    }
    #endregion
}