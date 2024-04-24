namespace Logic.DataSystem
{
    using System;

    [Serializable]
    public class DataManagerDto
    {
        public EventListDto<Pair<string, string>> audio;
        public EventListDto<Pair<string, string>> texts;
        public EventListDto<Pair<string, string>> videos;
        public EventListDto<Pair<string, string>> images;
    }
}