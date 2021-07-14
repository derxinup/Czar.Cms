using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Dtos
{
    public class TouristRoutePictureDto
    {
        public int Id { get; set; }

        public string url { get; set; }

        public Guid TouristRouteId { get; set; }
    }
}
