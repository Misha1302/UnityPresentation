namespace View.Slides
{
    using System;
    using System.Linq;
    using UnityEngine;

    public class SlideSwitcher : MonoBehaviour
    {
        [SerializeField] private SlideBase[] slides;

        private int _slideIndex;

        public int SlideIndex
        {
            get => _slideIndex;
            set
            {
                var prev = _slideIndex;
                _slideIndex = Math.Clamp(value, 0, slides.Length - 1);

                if (prev != _slideIndex)
                    ReRender(prev, _slideIndex);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Prev();
            if (Input.GetKeyDown(KeyCode.RightArrow)) Next();
        }

        private void OnValidate()
        {
            if (slides == null || slides.Length == 0)
                slides = GetComponentsInChildren<SlideBase>(true);
        }

        private void ReRender(int prevInd, int curInd)
        {
            HideImmediatelyAllSlides();

            var prev = slides[prevInd];
            if (prev.SlideState != SlideState.Hiding) prev.Hide();

            var cur = slides[curInd];
            if (cur.SlideState != SlideState.Showing) cur.Show();
        }

        private void HideImmediatelyAllSlides()
        {
            foreach (var t in slides.Where(x => x.SlideState != SlideState.Hided))
                t.HideImmediately();
        }

        private void Next() => SlideIndex++;

        private void Prev() => SlideIndex--;
    }
}