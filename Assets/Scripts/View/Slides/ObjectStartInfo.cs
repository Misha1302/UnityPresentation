namespace View.Slides
{
    using System;
    using UnityEngine;

    [Serializable]
    public class ObjectStartInfo
    {
        public Color defaultColor;
        public Vector3 defaultPosition;
        public Vector3 defaultRotation;
        public Vector3 defaultScale;

        public ObjectStartInfo(Color defaultColor, Vector3 defaultPosition, Vector3 defaultRotation, Vector3 defaultScale)
        {
            this.defaultColor = defaultColor;
            this.defaultPosition = defaultPosition;
            this.defaultRotation = defaultRotation;
            this.defaultScale = defaultScale;
        }
    }
}