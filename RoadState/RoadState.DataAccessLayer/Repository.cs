
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
    public class Repository
    {
        protected RoadStateContext RoadStateContext { get; set; }

        public Repository(RoadStateContext roadStateContext)
        {
            this.RoadStateContext = roadStateContext;
        }

        public bool Contains<T>(T t) where T : class
        {
            return this.RoadStateContext.Set<T>().Contains(t);
        }

        public void AddUser(User user)
        {
            this.RoadStateContext.Users.Add(user);
            this.RoadStateContext.SaveChanges();
        }

        public void AddBugReport(BugReport bugreport)
        {
            this.RoadStateContext.BugReports.Add(bugreport);
            this.RoadStateContext.SaveChanges();
        }

        public void AddComment(Comment comment)
        {
            this.RoadStateContext.Comments.Add(comment);
            this.RoadStateContext.SaveChanges();
        }

        public void AddPhoto(Photo photo)
        {
            this.RoadStateContext.Photos.Add(photo);
            this.RoadStateContext.SaveChanges();
        }

        public void UpdateBugReport(BugReport bugreport)
        {
            this.RoadStateContext.BugReports.Update(bugreport);
            this.RoadStateContext.SaveChanges();
        }

        public void DeleteBugReport(int id)
        {
            var bugReport = this.RoadStateContext.BugReports.FirstOrDefault(b => b.Id == id);
            this.RoadStateContext.BugReports.Remove(bugReport);
            this.RoadStateContext.SaveChanges();
        }

        public void DeleteComment(int id)
        {
            var comment = this.RoadStateContext.Comments.FirstOrDefault(c => c.Id == id);
            this.RoadStateContext.Comments.Remove(comment);
            this.RoadStateContext.SaveChanges();
        }

        public void DeleteUser(string id)
        {
            var user = this.RoadStateContext.Users.FirstOrDefault(u => u.Id == id);
            this.RoadStateContext.Users.Remove(user);
            this.RoadStateContext.SaveChanges();
        }

        public User GetUser(string id)
        {
            return this.RoadStateContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public BugReport GetBugReport(int id)
        {
            return this.RoadStateContext.BugReports.FirstOrDefault(b => b.Id == id);
        }

        public Comment GetComment(int id)
        {
            return this.RoadStateContext.Comments.FirstOrDefault(c => c.Id == id);
        }

        public List<BugReport> GetBugReports()
        {
            return this.RoadStateContext.BugReports.Include(x => x.Author).Include(x => x.Comments).ToList();
        }

        public void RateBugReport(string userId, int bugReportId, bool hasAgreed)
        {
            this.RoadStateContext.BugReportRates.Add(new BugReportRate()
            {
                UserId = userId,
                User = GetUser(userId),
                BugReportId = bugReportId,
                BugReport = GetBugReport(bugReportId),
                HasAgreed = hasAgreed,
            });
            this.RoadStateContext.SaveChanges();
        }
    }
}
