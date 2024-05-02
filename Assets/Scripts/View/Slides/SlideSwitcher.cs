namespace View.Slides
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Shared.Extensions;
    using UnityEngine;

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
            SetSlide();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Prev();
            if (Input.GetKeyDown(KeyCode.RightArrow)) Next();
        }

        private void SetSlide()
        {
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (Application.isEditor)
                SlideIndex = _slides.FindLastIndex(x => x.gameObject.activeSelf);
            else SlideIndex = _slides.FindIndex(x => x.name == startSlideName);
        }

        private void InitSlides()
        {
            _slides = GetComponentsInChildren<SlideBase>(true).ToList();
            if (reverse)
                _slides.Reverse();

            if (!Application.isEditor)
                _slides.ForEach(slide => slide.Init());
        }

        private void ReRender(int prevInd, int curInd)
        {
            HideImmediatelyAllSlides(prevInd);

            HandleCurrentSlide(curInd);
            HandlePrevSlide(prevInd, curInd);
        }

        private void HandlePrevSlide(int prevInd, int curInd)
        {
            if (prevInd < 0) return;

            var cur = _slides[curInd];
            var prev = _slides[prevInd];

            if (curInd > prevInd)
                CarryObjectsToNextSlide(prev, cur);

            prev.HideImmediately();
            HideSlide(prev);
        }

        private void HandleCurrentSlide(int curInd)
        {
            ShowSlide(_slides[curInd]);
        }

        private static void HideSlide(SlideBase prev)
        {
            if (prev.SlideState != SlideState.Hiding)
                prev.Hide();
        }

        private static void CarryObjectsToNextSlide(SlideBase prev, Component cur)
        {
            prev.SaveToNextSlide.ForAll(x => x.SetParent(cur.transform));
        }

        private static void ShowSlide(SlideBase cur)
        {
            if (Application.isEditor) cur.Init();
            if (cur.SlideState != SlideState.Showing) cur.Show();
        }

        private void HideImmediatelyAllSlides(params int[] except)
        {
            _slides.Except(except).ForAll(x => x.HideImmediately());
        }

        private void Next() => SlideIndex++;

        private void Prev() => SlideIndex--;
    }
}