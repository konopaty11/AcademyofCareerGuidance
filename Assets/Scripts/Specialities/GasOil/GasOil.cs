using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// игра трубопроводров
/// </summary>
public class GasOil : Speciality
{
    [SerializeField] GameObject gameWindow;
    [SerializeField] CaptureScreen captureScreen;
    [SerializeField] RectTransform captureArea;
    [SerializeField] GameObject readyBtn;

    Pipe[,] grid = new Pipe[7, 4];

    List<GameObject> pipes = new();
    List<Vector2> hashPipes = new();

    float sizeCell = 300f;

    void OnEnable()
    {
        Saves.SavesLoad += RestoreSettings;
    }

    void OnDisable()
    {
        Saves.SavesLoad -= RestoreSettings;
    }

    void Start()
    {
        StartCoroutine(CheckCorutine());

    }

    /// <summary>
    /// загрузка настроек
    /// </summary>
    void RestoreSettings()
    {
        IsComplete = SpecialityManager.Instance.Saves.SavesData.IsPipelineComplite;
    }

    /// <summary>
    /// начало игры
    /// </summary>
    public void StartGame()
    {
        if (IsComplete) return;
        IsComplete = false;
        gameWindow.SetActive(true);
        readyBtn.SetActive(false);

        for (int i = 0; i < grid.GetLength(0); i++)
            for (int j = 0; j < grid.GetLength(1); j++)
                grid[i, j] = null;

        for (int i = 0; i < pipes.Count; i++)
        {
            Destroy(pipes[i]);
            pipes[i] = null;
        }

        hashPipes = new();
    }

    /// <summary>
    /// удаление трубы из сетки
    /// </summary>
    /// <param name="prefPosition"></param>
    /// <param name="position"></param>
    /// <param name="pipe"></param>
    public void DeleteFromGrid(Vector2 prefPosition , Vector2 position, Pipe pipe)
    {
        int prefX = Mathf.RoundToInt((prefPosition.x - 150) / sizeCell);
        int prefY = Mathf.RoundToInt((prefPosition.y - 150) / sizeCell);

        if (prefX > grid.GetLength(1) - 1) prefX = grid.GetLength(1) - 1;
        else if (prefX < 0) prefX = 0;

        if (prefY > grid.GetLength(0) - 1) prefY = grid.GetLength(0) - 1;
        else if (prefY < 0) prefY = 0;

        int x = Mathf.RoundToInt((position.x - 150) / sizeCell);
        int y = Mathf.RoundToInt((position.y - 150) / sizeCell);

        if (x > grid.GetLength(1) - 1) x = grid.GetLength(1) - 1;
        else if (x < 0) x = 0;

        if (y > grid.GetLength(0) - 1) y = grid.GetLength(0) - 1;
        else if (y < 0) y = 0;

        if (grid[prefY, prefX] == pipe && (prefX != x || prefY != y))
        {
            grid[prefY, prefX] = null;
        }
    }

    /// <summary>
    /// получить координаты сетки для трубы
    /// </summary>
    /// <param name="position"></param>
    /// <param name="pipe"></param>
    /// <returns></returns>
    public Vector2 SetGridCoord(Vector2 position, Pipe pipe)
    {
        int x = Mathf.RoundToInt((position.x - 150) / sizeCell);
        int y = Mathf.RoundToInt((position.y - 150) / sizeCell);

        if (x > grid.GetLength(1) - 1) x = grid.GetLength(1) - 1;
        else if (x < 0) x = 0;

        if (y > grid.GetLength(0) - 1) y = grid.GetLength(0) - 1;
        else if (y < 0) y = 0;

        if (grid[y, x] == null || grid[y, x] == pipe)
        {
            grid[y, x] = pipe;
            return new(x * sizeCell + 150, y * sizeCell + 150);
        }

        return position;
    }

    /// <summary>
    /// корутина проверки правильности трубопровода
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckCorutine()
    {
        while (true)
        {
            if (!IsComplete)
            {
                CheckPipeline();
                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }
    }

    /// <summary>
    /// проверка трубопровода
    /// </summary>
    public void CheckPipeline()
    {
        hashPipes = new();

        Vector2Int start = new(1, 1);
        Vector2Int finish = new(2, 6);
        Vector2Int current = start;

        if (grid[start.y, start.x] == null || !grid[start.y, start.x].Sides.HasFlag(AvailableSide.Bottom) ||
            grid[finish.y, finish.x] == null || !grid[finish.y, finish.x].Sides.HasFlag(AvailableSide.Right))
        {
            return;
        }

        AvailableSide prefSide = AvailableSide.Top;
        if (IsSuccesfulPipeline(current, finish, prefSide))
        {
            IsComplete = true;
            SpecialityManager.Instance.Saves.SavesData.IsPipelineComplite = IsComplete;
            SpecialityManager.Instance.Saves.Save();

            readyBtn.SetActive(true);
            CapturePipeline();
        }
    }

    /// <summary>
    /// скриншот трубопровода
    /// </summary>
    void CapturePipeline()
    {

        Vector3[] corners = new Vector3[4];
        captureArea.GetWorldCorners(corners);

        Canvas canvas = captureArea.GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Camera camera = canvas.worldCamera ?? Camera.main;
            if (camera != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corners[i]);
                    corners[i] = camera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, camera.nearClipPlane));
                }
            }
        }

        int width = (int)(corners[2].x - corners[0].x);
        int height = (int)(corners[2].y - corners[0].y);
        int x = (int)corners[0].x;
        int y = (int)corners[0].y;

        captureScreen.CaptureAreaAsSprite(width, height, x, y);

    }

    /// <summary>
    /// возвращает правильность трубопровода
    /// </summary>
    /// <param name="current"></param>
    /// <param name="finish"></param>
    /// <param name="prefSide"></param>
    /// <returns></returns>
    bool IsSuccesfulPipeline(Vector2Int current, Vector2 finish, AvailableSide prefSide)
    {
        bool isReached = false;
        foreach (AvailableSide side in grid[current.y, current.x].GetAvailableSides())
        {
            if (side == ShiftDigits(prefSide, 2)) continue;

            hashPipes.Add(current);

            Vector2Int next = current;
            switch (side)
            {
                case AvailableSide.Left:
                    next += Vector2Int.left;
                    break;
                case AvailableSide.Right:
                    next += Vector2Int.right;
                    break;
                case AvailableSide.Top:
                    next += Vector2Int.up;
                    break;
                case AvailableSide.Bottom:
                    next += Vector2Int.down;
                    break;
            }

            if (next == finish)
            {
                isReached = true;
                continue;
            }

            if (next.y < 0 || next.x < 0 || next.x >= grid.GetLength(1)  || next.y >= grid.GetLength(0) || 
                grid[next.y, next.x] == null || hashPipes.Contains(next))
            {
                return false;
            }

            if (current != next)
                return IsSuccesfulPipeline(next, finish, side);
        }

        if (isReached) return true;

        return false;
    }

    /// <summary>
    /// переместить разряды числа
    /// </summary>
    /// <param name="side"></param>
    /// <param name="countShifts"></param>
    /// <returns></returns>
    AvailableSide ShiftDigits(AvailableSide side, int countShifts)
    {
        int num = (int)side;
        for (int i = 0; i < countShifts; i++)
        {
            num >>= 1;
            if ((AvailableSide)num == AvailableSide.None) num = (int)AvailableSide.Bottom;
        }

        return (AvailableSide)num;
    }

    /// <summary>
    /// добавить трубу в список труб
    /// </summary>
    /// <param name="pipe"></param>
    public void AddPipe(GameObject pipe)
    {
        pipes.Add(pipe);
    }

    /// <summary>
    /// удалить трубу из списка труб
    /// </summary>
    public void DeletePipe()
    {
        if (pipes.Count > 0)
        {
            Destroy(pipes[pipes.Count - 1]);
            pipes.RemoveAt(pipes.Count - 1);
        }
    }
}
