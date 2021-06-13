using System.Diagnostics;
using System.Linq;
using DapperEFCorePerformanceBenchmarks.Models;
using DapperEFCorePerformanceBenchmarks.TestData;
using Microsoft.EntityFrameworkCore;

namespace DapperEFCorePerformanceBenchmarks.DataAccess
{
    public class EntityFrameworkRawCore : ITestSignature
    {

        public long GetPlayerByID(int id)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (SportContextEfCore context = new SportContextEfCore(Database.GetOptions()))
            {
                var player = context.Players.FromSqlRaw("SELECT Id, FirstName, LastName, DateOfBirth, TeamId FROM Player WHERE Id = @p0", id).AsNoTracking().FirstOrDefault(); 
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long GetRosterByTeamID(int teamId)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (SportContextEfCore context = new SportContextEfCore(Database.GetOptions()))
            {
                var teams = context.Teams.FromSqlRaw("SELECT Id, Name, SportID, FoundingDate FROM Team WHERE ID = @p0", teamId).AsNoTracking().FirstOrDefault();
                var players = context.Players.FromSqlRaw("SELECT Id, FirstName, LastName, DateOfBirth, TeamId FROM Player WHERE TeamId = @p0", teamId).AsNoTracking().ToList(); 
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long GetTeamRostersForSport(int sportId)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (SportContextEfCore context = new SportContextEfCore(Database.GetOptions()))
            {
                var players = context.Teams.Include(x => x.Players).Where(x => x.SportId == sportId).AsNoTracking().ToList(); 
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
