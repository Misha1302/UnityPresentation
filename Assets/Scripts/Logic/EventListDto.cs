using System.Collections.Generic;

namespace Logic
{
    using System;

    [Serializable]
    public class EventListDto<T>
    {
        public List<T> list;
    }
}