namespace Logic.SaveSystem
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class EventListDto<T>
    {
        public List<T> list;
    }
}