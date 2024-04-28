namespace View
{
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Video;
    using View.Interfaces;

    public class SceneLighter : MonoBehaviour
    {
        [SerializeField] private bool changeMeToLightScene;
        [SerializeField] private bool changeMeToInitAll;

        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            if (changeMeToLightScene)
            {
                GetComponentsInChildren<VideoPlayer>(true).ForAll(x => x.targetTexture = null);
                GetComponentsInChildren<RawImage>(true).ForAll(x => x.texture = null);
                GetComponentsInChildren<Image>(true).ForAll(x => x.sprite = null);
                GetComponentsInChildren<AudioSource>(true).ForAll(x => x.clip = null);
            }

            if (changeMeToInitAll)
                GetComponentsInChildren<ISlideInitable>(true).ForAll(x => x.Init());


            changeMeToLightScene = changeMeToInitAll = false;
        }
    }
}