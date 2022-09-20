using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this._regionRepository = regionRepository;
            this._mapper = mapper;
        }

        

        [HttpGet]        
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllAsync();

            //return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();

            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };

            //    regionsDTO.Add(regionDTO);
            //});
            var regionsDTO = _mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await _regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = _mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<ActionResult> AddRegionAsync(AddRegionRequest addRegionRequest)
        {
            //Request(DTO) to Domain Model
            var region = new Models.Domain.Region()
            {
                Name = addRegionRequest.Name,
                Code = addRegionRequest.Code,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population,
                Area = addRegionRequest.Area
            };


            //Pass details to Repository
            region = await _regionRepository.AddAsync(region);

            //Convert back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
                Area = region.Area
            };

            return CreatedAtAction(nameof(GetRegionAsync), new {id = regionDTO.Id },regionDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult> DeleteRegionAsync(Guid id)
        {
            //Get Region from DB

            var region = await _regionRepository.DeleteAsync(id);

            //If null Notfound
            if (region == null) return NotFound();

            //Convert response back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
                Area = region.Area
            };

            //return ok response
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Id = id,
                Name = updateRegionRequest.Name,
                Code = updateRegionRequest.Code,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population,
                Area = updateRegionRequest.Area

            };

            //Update Region using Repository

            region = await _regionRepository.UpdateAsync(id, region);

            //If Null then NotFound
            if (region == null) return NotFound();


            //Convert back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = id,
                Name = region.Name,
                Code = region.Code,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
                Area = region.Area

            };

            //Return ok response
            return Ok(regionDTO);
        }
    }
}
