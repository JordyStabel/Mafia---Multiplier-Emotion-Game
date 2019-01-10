using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(Text))]
[TrackClipType(typeof(SubtitleClip))]
[TrackColor(0.1394896f, 0.4411765f, 0.3413077f)]
public class SubtitleTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<SubtitleMixerBehaviour>.Create(graph, inputCount);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        Text trackBinding = director.GetGenericBinding(this) as Text;
        if (trackBinding == null)
            return;

        var serializedObject = new UnityEditor.SerializedObject(trackBinding);
        var iterator = serializedObject.GetIterator();
        while (iterator.NextVisible(true))
        {
            if (iterator.hasVisibleChildren)
                continue;

            driver.AddFromName<Text>(trackBinding.gameObject, iterator.propertyPath);
        }
#endif
        base.GatherProperties(director, driver);
    }

}

public class SubtitleMixerBehaviour : PlayableBehaviour
{
    Text m_TrackBinding;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_TrackBinding = playerData as Text;
        if (m_TrackBinding == null) { return; }

        int inputCount = playable.GetInputCount();

        float greatestWeight = 0f;
        int currentInputs = 0;

        for (int i = 0; i < inputCount; i++)
        {
            ScriptPlayable<SubtitleBehaviour> inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
            SubtitleBehaviour input = inputPlayable.GetBehaviour();

            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > greatestWeight)
            {
                m_TrackBinding.text = input.Text;
                greatestWeight = inputWeight;

                if (!Mathf.Approximately(inputWeight, 0f)) { currentInputs++; }
            }
        }

        if (currentInputs == 0)
        {
            m_TrackBinding.text = string.Empty;
        }
    }
}
