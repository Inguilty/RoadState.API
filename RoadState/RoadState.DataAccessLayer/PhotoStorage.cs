using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RoadState.DataAccessLayer
{
    public interface IPhotoCreator
    {
        void CreatePhoto(Photo photo);
    }

    public interface IPhotoFinder
    {
        IEnumerable<Photo> GetPhotoes(Expression<Func<Photo, bool>> predicate);
    }

    public class PhotoStorage : IPhotoCreator, IPhotoFinder
    {
        private RoadStateContext _context;
        public PhotoStorage(RoadStateContext context)
        {
            this._context = context;
        }

        public void CreatePhoto(Photo photo)
        {
            this._context.AddAsync(photo);
            this._context.SaveChangesAsync();
        }

        public IEnumerable<Photo> GetPhotoes(Expression<Func<Photo, bool>> predicate)
        {
            return _context.Photos.Where(predicate);
        }
    }
}
