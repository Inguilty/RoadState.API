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
    public interface IBugReportRater
    {
        Task RateBugReportAsync(BugReport bugReport, User user, bool hasAgreed);
    }

    public interface IBugReportFinder
    {
        Task<List<BugReport>> GetBugReportsAsync(Expression<Func<BugReport, bool>> predicate);
    }

    public interface IBugReportCreator
    {
        Task CreateBugReportAsync(BugReport bugReport);
    }

    public class BugReportStorage : IBugReportCreator, IBugReportFinder, IBugReportRater
    {
        private RoadStateContext _context;
        public BugReportStorage(RoadStateContext context)
        {
            this._context = context;
        }

        public async Task CreateBugReportAsync(BugReport bugReport)
        {
            await this._context.BugReports.AddAsync(bugReport);
            await this._context.SaveChangesAsync();
        }

        public async Task RateBugReportAsync(BugReport bugReport, User user, bool hasAgreed)
        {
            await this._context.BugReportRates.AddAsync(new BugReportRate
            {
                User = user,
                UserId = user.Id,
                BugReport = bugReport,
                BugReportId = bugReport.Id,
                HasAgreed = hasAgreed,
            });
            await this._context.SaveChangesAsync();
        }

        public async Task<List<BugReport>> GetBugReportsAsync(Expression<Func<BugReport, bool>> predicate)
        {
            return await this._context.BugReports.Include(x => x.Author).Include(x => x.BugReportRates).Where(predicate).ToListAsync();
        }
    }
}
