using AutoMapper;
using xLiAdProjectTemplate.Entities;
using xLiAdProjectTemplate.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xLiAdProjectTemplate.Services.Assets
{
    public class MappingHelper
    {
        private static IMapper MapperInstance;
        public static IMapper Mapper
        {
            get
            {
                if (MapperInstance == null)
                    MapperInstance = Configuration.CreateMapper();
                return MapperInstance;
            }
        }

        private static MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
        {

        });
    }
}
