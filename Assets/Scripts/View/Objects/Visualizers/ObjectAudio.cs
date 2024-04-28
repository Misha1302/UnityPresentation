namespace View.Objects.Visualizers
{
    using System.Collections.Generic;
    using Logic.DataSystem;
    using Shared.Coroutines;
    using Shared.Extensions;
    using Shared.ResourceLoader;
    using UnityEngine;

    public class ObjectAudio : ObjectVisualizer
    {
        [Range(0f, 1f)] [SerializeField] private float volume = 1;
        [SerializeField] private float delay = 0.1f;

        private readonly CoroutineManager _cm = new();

        private void OnDisable()
        {
            _cm.StopCors();
        }

        public void SetVolume(float v)
        {
            volume = v;
            GetOrAddComponent<AudioSource>().volume = v;
        }

        public override void Init()
        {
            SetAudio();
        }

        public override void Show()
        {
            if (Application.isPlaying)
                _cm.StartCor(CoroutinesHelper.StartAfterCoroutine(GetComponent<AudioSource>().Play, delay));
        }

        public override void Hide()
        {
            GetOrAddComponent<AudioSource>().Stop();
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
                    component.playOnAwake = false;
                });
        }
    }
}