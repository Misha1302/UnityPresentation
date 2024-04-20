using System.Linq;

namespace Logic
{
    public static class DtoExtensions
    {
        public static DataManagerDto ToDto(this DataManager dataManager) =>
            new()
            {
                audio = new EventListDto<Pair<string, string>> { list = dataManager.Audio.ToList() },
                videos = new EventListDto<Pair<string, string>> { list = dataManager.Videos.ToList() },
                texts = new EventListDto<Pair<string, string>> { list = dataManager.Texts.ToList() },
                images = new EventListDto<Pair<string, string>> { list = dataManager.Images.ToList() },
            };

        
        public static DataManager FromDto(this DataManagerDto dto) =>
            new(
                new EventList<Pair<string, string>>(dto.audio.list),
                new EventList<Pair<string, string>>(dto.texts.list),
                new EventList<Pair<string, string>>(dto.videos.list),
                new EventList<Pair<string, string>>(dto.images.list)
            );
    }
}