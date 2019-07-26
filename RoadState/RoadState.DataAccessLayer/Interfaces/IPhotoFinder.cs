using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace RoadState.DataAccessLayer.Interfaces
{
    public interface IPhotoFinder
    {
        IEnumerable<Photo> GetPhotoes(Expression<Func<Photo, bool>> predicate);
    }
}
