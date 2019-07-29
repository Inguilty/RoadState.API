using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoadState.DataAccessLayer;
using System.Linq;
using System.Threading.Tasks;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPhotoFinder photoFinder;
        public PhotoController(
            IMapper mapper,
            IPhotoFinder photoFinder
            )
        {
            this._mapper = mapper;
            this.photoFinder = photoFinder;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhotoByIdAsync(int id)
        {
            var photos = await photoFinder.GetPhotoesAsync(x => x.Id == id);
            var photo = photos.FirstOrDefault();
            if(photo != null)
            {
                return Ok(File(photo.Blob, "image/jpeg"));
            }
            return NotFound("No photo found with provided id");
        }
    }
}