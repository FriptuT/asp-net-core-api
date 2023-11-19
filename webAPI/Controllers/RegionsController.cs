using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using webAPI.Data;
using webAPI.Models.Domain;
using webAPI.Models.DTOs;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ROWalksDbContext dbContext;

        public RegionsController(ROWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //-----------------------------------------------------

        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get Data From Database - Domain models
            var regionsDomain = await dbContext.Regions.ToListAsync();

            // Map Domain Models to DTOs
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            return Ok(regionsDto);
        }

        //-----------------------------------------------------

        //GET SINGLE REGION ( Get Region by ID)
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await dbContext.Regions.FindAsync(id);
           // var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map/Convert Region Domain Model to Region DTO

            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDomain);
        }

        //-----------------------------------------------------

        // POST To Create New Region
        // POST : https://localhost:portnumber/api/regions/
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map / Convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl= addRegionRequestDto.RegionImageUrl 
            };

            // Use Domain Model to create Region
            await dbContext.Regions.AddAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();

            // Map / Convert Domain Model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id}, regionDto);
        }

        //-----------------------------------------------------

        // PUT to Update a certain Region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto )
        {
            //Check if region exists
            //var regionDomainModel = dbContext.Regions.Find(id);
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // MAP/CONVERT DTO to Domain Model

            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;


            await dbContext.SaveChangesAsync();

            // MAP/CONVERT Domain Model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto); 

        }

        //-----------------------------------------------------

        // Delete Region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async  Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Delete Region
            dbContext.Regions.Remove(regionDomainModel);
            await dbContext.SaveChangesAsync();

            // return deleted Region back
            // map Domain Model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code= regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };


            return Ok(regionDto);
        }

        //-----------------------------------------------------
    }
}
