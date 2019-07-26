using RoadState.Data;
using RoadState.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RoadState.DataAccessLayer
{
    public class PhotoStorage : IPhotoCreator, IPhotoFinder
    {
        private RoadStateContext _context;
        public PhotoStorage(RoadStateContext context)
        {
            this._context = context;
        }

        public void CreatePhoto(Photo photo)
        {
            this._context.Add(photo);
            this._context.SaveChanges();
        }

        public IEnumerable<Photo> GetPhotoes(Expression<Func<Photo, bool>> predicate)
        {
            return _context.Photos.Where(predicate);
        }
    }
}
