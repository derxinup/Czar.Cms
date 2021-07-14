using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.RegularExpressions;
using FakeXiecheng.API.ResourceParameters;
using FakeXiecheng.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using FakeXiecheng.API.Helper;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository,
            IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public IActionResult GetTouristRoutes([FromQuery] TouristRouteRescourceParameters parameters) 
        {
           var touristRoutesFromRepo =  _touristRouteRepository.GetTouristRoutes(parameters.Keyword, parameters.RatingOperator, parameters.RatingValue);
           if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0) {
                return NotFound("没有旅游路线");            
           }
           var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);
           return Ok(touristRoutesDto);
        }

        [HttpGet("{touristRouteId}",Name = "GetTouristRouteById")]
        [HttpHead]
        public IActionResult GetTouristRouteById(Guid touristRouteId) {
            var touristrouteFromRepo=_touristRouteRepository.GetTouristRoute(touristRouteId);
            if (touristrouteFromRepo == null) {
                return NotFound($"旅游路线{touristRouteId}找不到");
            }
            //var touristRouteDto = new TouristRouteDto()
            //{
            //    Id = touristrouteFromRepo.Id,
            //    Title = touristrouteFromRepo.Title,
            //    Description = touristrouteFromRepo.Description,
            //    Price = touristrouteFromRepo.OriginalPrice * (decimal)(touristrouteFromRepo.DiscountPrecent ?? 1),
            //    CreteTime = touristrouteFromRepo.CreteTime,
            //    UpdateTime= touristrouteFromRepo.UpdateTime,
            //    Features= touristrouteFromRepo.Features,
            //    Fees= touristrouteFromRepo.Fees,
            //    Notes= touristrouteFromRepo.Notes,
            //    Rating= touristrouteFromRepo.Rating,
            //    TravelDays= touristrouteFromRepo.TravelDays.ToString(),
            //    TripType= touristrouteFromRepo.TripType.ToString(),
            //    DepartureCity= touristrouteFromRepo.DepartureCity.ToString()
            //};
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristrouteFromRepo);
            return Ok(touristRouteDto);
        }

        [HttpPost]
        public IActionResult CreateTouristRoute([FromBody] TouristForCreationDto touristForCreationDto) {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            _touristRouteRepository.Save();
            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId = touristRouteToReturn.Id },
                touristRouteToReturn
            );
        }

        [HttpPut("{touristRouteId}")]
        public IActionResult UpdateTouristRoute(
            [FromRoute]Guid touristRouteId,
            [FromBody]TouristRouteForUpdateDto touristRouteForUpdateDto
        ) 
        {
            var touristrouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if (touristrouteFromRepo == null)
            {
                return NotFound($"旅游路线{touristRouteId}找不到");
            }
            _mapper.Map(touristRouteForUpdateDto, touristrouteFromRepo);

            _touristRouteRepository.Save();

            return NoContent();
        }

        [HttpPatch("{touristRouteId}")]
        public IActionResult PartiallyUpdateTouristRoute(
            [FromRoute]Guid touristRouteId,
            [FromBody]JsonPatchDocument<TouristRouteForUpdateDto> patchDocument
        )
        {
            var touristrouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if (touristrouteFromRepo == null)
            {
                return NotFound($"旅游路线{touristRouteId}找不到");
            }
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristrouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch,ModelState);

            if (!TryValidateModel(touristRouteToPatch)) {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(touristRouteToPatch, touristrouteFromRepo);
            _touristRouteRepository.Save();

            return NoContent();
        }
        [HttpDelete("{touristRouteId}")]
        public IActionResult DeleteTouristRoute([FromRoute]Guid touristRouteId) {
            var touristrouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if (touristrouteFromRepo == null)
            {
                return NotFound($"旅游路线{touristRouteId}找不到");
            }

            _touristRouteRepository.DeleteTouristRoute(touristrouteFromRepo);
            _touristRouteRepository.Save();

            return NoContent();

        }


        [HttpDelete("({touristIDs})")]
        public IActionResult DeleteByIDs(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute]IEnumerable<Guid> touristIDs
        )
        {
            if (touristIDs == null) {
                return BadRequest();
            }
            var touristFromRepo = _touristRouteRepository.GetTouristRoutesByIDList(touristIDs);
            _touristRouteRepository.DeleteTouristRoutes(touristFromRepo);
            _touristRouteRepository.Save();

            return NoContent();

        }

    }
}
