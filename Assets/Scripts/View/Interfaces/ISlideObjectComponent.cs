namespace View.Interfaces
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface ISlideObjectComponent
    {
        public List<Component> GetNecessaryComponents();
    }
}