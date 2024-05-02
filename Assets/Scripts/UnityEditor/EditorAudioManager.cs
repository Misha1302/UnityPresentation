namespace View
{
    using Shared.Extensions;
    using UnityEngine;
    using View.Objects.Visualizers;

    public class EditorAudioManager : MonoBehaviour
    {
        [SerializeField] private bool changeMeToDisableAudio;
        [SerializeField] private bool changeMeToEnableAudio;

        private void OnValidate()
        {
            if (changeMeToDisableAudio) SetVolume(0);
            if (changeMeToEnableAudio) SetVolume(1);

            changeMeToDisableAudio = changeMeToEnableAudio = false;
        }

        private void SetVolume(float objVolume)
        {
            GetComponentsInChildren<AudioSource>(true).ForAll(x => x.volume = objVolume);
            GetComponentsInChildren<ObjectAudio>(true).ForAll(x => x.SetVolume(objVolume));
        }
    }
}