using Microsoft.EntityFrameworkCore;
using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoadState.DataAccessLayer
{
    public interface IPhotoCreator
    {
        Task CreatePhotoAsync(Photo photo);
    }

    public interface IPhotoFinder
    {
        Task<List<Photo>> GetPhotoesAsync(Expression<Func<Photo, bool>> predicate);
    }

    public class PhotoStorage : IPhotoCreator, IPhotoFinder
    {
        private RoadStateContext _context;
        public PhotoStorage(RoadStateContext context)
        {
            this._context = context;
        }

        public async Task CreatePhotoAsync(Photo photo)
        {
            await this._context.AddAsync(photo);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<Photo>> GetPhotoesAsync(Expression<Func<Photo, bool>> predicate)
        {
            return await _context.Photos.Where(predicate).ToListAsync();
        }
    }
}
