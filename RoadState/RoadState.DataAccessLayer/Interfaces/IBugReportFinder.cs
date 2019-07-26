using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace RoadState.DataAccessLayer.Interfaces
{
    public interface IBugReportFinder
    {
        IEnumerable<BugReport> GetBugReports(Expression<Func<BugReport, bool>> predicate);
    }
}
