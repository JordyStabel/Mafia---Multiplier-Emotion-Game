using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SubtitleClip : PlayableAsset, ITimelineClipAsset
{
    public SubtitleBehaviour Template = new SubtitleBehaviour();

    public ClipCaps clipCaps { get { return ClipCaps.None; } }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<SubtitleBehaviour>.Create(graph, Template);
    }
}

[Serializable]
public class SubtitleBehaviour : PlayableBehaviour
{
    public string Text;
}
