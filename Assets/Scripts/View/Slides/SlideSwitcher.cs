namespace View.Slides
{
    using System;
    using System.Linq;
    using UnityEngine;

    public class SlideSwitcher : MonoBehaviour
    {
        private int _slideIndex = -1;
        private SlideBase[] _slides;

        public int SlideIndex
        {
            get => _slideIndex;
            set
            {
                var prev = _slideIndex;
                _slideIndex = Math.Clamp(value, 0, _slides.Length - 1);

                if (prev != _slideIndex)
                    ReRender(prev, _slideIndex);
            }
        }

        private void Start()
        {
            _slides = GetComponentsInChildren<SlideBase>(true);
            foreach (var slide in _slides)
                slide.gameObject.SetActive(false);

            SlideIndex = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Prev();
            if (Input.GetKeyDown(KeyCode.RightArrow)) Next();
        }

        private void ReRender(int prevInd, int curInd)
        {
            HideImmediatelyAllSlides();

            if (prevInd >= 0)
            {
                var prev = _slides[prevInd];
                if (prev.SlideState != SlideState.Hiding) prev.Hide();
            }

            var cur = _slides[curInd];
            if (cur.SlideState != SlideState.Showing) cur.Show();
        }

        private void HideImmediatelyAllSlides(params int[] except)
        {
            for (var index = 0; index < _slides.Length; index++)
            {
                if (except.Contains(index)) continue;

                _slides[index].HideImmediately();
            }
        }

        private void Next() => SlideIndex++;

        private void Prev() => SlideIndex--;
    }
}