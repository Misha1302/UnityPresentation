namespace View.Slides
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Shared.Extensions;
    using UnityEngine;
    using View.Interfaces;

    public class SlideSwitcher : MonoBehaviour
    {
        [SerializeField] private bool reverse = true;
        [SerializeField] private string startSlideName;

        private int _slideIndex = -1;
        private List<SlideBase> _slides;

        public int SlideIndex
        {
            get => _slideIndex;
            set
            {
                var prev = _slideIndex;
                _slideIndex = Math.Clamp(value, 0, _slides.Count - 1);

                if (prev != _slideIndex)
                    ReRender(prev, _slideIndex);
            }
        }

        private void Start()
        {
            InitSlides();
            DisableSlides();
            SlideIndex = _slides.FindIndex(x => x.name == startSlideName);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Prev();
            if (Input.GetKeyDown(KeyCode.RightArrow)) Next();
        }

        private void InitSlides()
        {
            _slides = GetComponentsInChildren<SlideBase>(true).ToList();
            if (reverse)
                _slides.Reverse();

            _slides.ForEach(slide => slide.GetComponentsInChildren<ISlideInitable>().ForAll(x => x.Init()));
        }

        private void DisableSlides()
        {
            foreach (var slide in _slides)
                slide.gameObject.SetActive(false);
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
            for (var index = 0; index < _slides.Count; index++)
            {
                if (except.Contains(index)) continue;

                _slides[index].HideImmediately();
            }
        }

        private void Next() => SlideIndex++;

        private void Prev() => SlideIndex--;
    }
}