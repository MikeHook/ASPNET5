namespace TheWorld.Controllers.Api
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using AutoMapper;
    using Microsoft.AspNet.Mvc;
    using Models;
    using ViewModels;

    [Route("api/trips")]
    public class TripController : Controller
    {
        private readonly IWorldRepository _worldRepository;

        public TripController(IWorldRepository worldRepository)
        {
            _worldRepository = worldRepository;
        }

        [HttpGet("")]
        public JsonResult Get()
        {
            var results = Mapper.Map<IEnumerable<TripViewModel>>(_worldRepository.GetAllTripsWithStops());

            return Json(results);
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]TripViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTrip = Mapper.Map<Trip>(vm);

                    //Save to DB
                    _worldRepository.AddTrip(newTrip);

                    if (_worldRepository.SaveAll())
                    {
                        Response.StatusCode = (int) HttpStatusCode.Created;
                        return Json(Mapper.Map<TripViewModel>(newTrip));
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message});
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new {Message = "Failed", ModelState = ModelState});
        }
    }
}
