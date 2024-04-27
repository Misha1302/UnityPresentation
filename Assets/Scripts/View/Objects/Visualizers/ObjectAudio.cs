namespace View.Objects.Visualizers
{
    using System.Collections.Generic;
    using Logic.DataSystem;
    using Shared.Extensions;
    using Shared.ResourceLoader;
    using UnityEngine;

    public class ObjectAudio : ObjectVisualizer
    {
        [Range(0f, 1f)] [SerializeField] private float volume = 1;

        public override void Init()
        {
            SetAudio();
        }

        public override void Show()
        {
            if (Application.isPlaying)
                GetComponent<AudioSource>().Play();
        }

        public override void Hide()
        {
            GetOrAddComponent<AudioSource>().Stop();
        }

        public override void PreShow()
        {
        }

        public override List<Component> GetNecessaryComponents() => new() { GetOrAddComponent<AudioSource>() };

        private void SetAudio()
        {
            ResourceLoader.LoadAudio(
                Data.Instance.GetAudioPath(key).PathToUrl(),
                clip =>
                {
                    var component = GetOrAddComponent<AudioSource>();
                    component.clip = clip;
                    component.volume = volume;
                });
        }
    }
}