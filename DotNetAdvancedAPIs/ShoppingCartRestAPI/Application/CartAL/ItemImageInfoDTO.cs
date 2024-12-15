using AutoMapper;
using ProductDomain.Entities;

namespace Application.CartAL
{
    public class ItemImageInfoDTO
    {
        public string Url { get; set; }
        public string AltText { get; set; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<ItemImageInfo, ItemImageInfoDTO>();
                CreateMap<ItemImageInfoDTO, ItemImageInfo>();
            }
        }
    }
}
