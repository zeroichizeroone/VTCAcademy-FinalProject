using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    public List<TimelineData> timelines; // Danh sách timeline được set từ Inspector

    private Dictionary<string, PlayableDirector> timelineDict;

    void Awake()
    {
        // Đảm bảo Singleton duy nhất
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển Scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Chuyển danh sách sang Dictionary để truy cập nhanh
        timelineDict = new Dictionary<string, PlayableDirector>();
        foreach (var timeline in timelines)
        {
            if (timeline != null && !string.IsNullOrEmpty(timeline.timelineName))
            {
                timelineDict[timeline.timelineName] = timeline.director;
            }
        }
    }

    public void PlayTimeline(string timelineName)
    {
        if (timelineDict.TryGetValue(timelineName, out PlayableDirector director))
        {
            director.Play();
        }
        else
        {
            Debug.LogWarning($"Timeline '{timelineName}' không tồn tại!");
        }
    }

    public void StopTimeline(string timelineName)
    {
        if (timelineDict.TryGetValue(timelineName, out PlayableDirector director))
        {
            director.Stop();
        }
        else
        {
            Debug.LogWarning($"Timeline '{timelineName}' không tồn tại!");
        }
    }
}

[System.Serializable]
public class TimelineData
{
    public string timelineName;
    public PlayableDirector director;
}