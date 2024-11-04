using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFLTeams.Models;

namespace NFLTeams.Controllers
{
    public class FavoritesController : Controller
    {
        private TeamContext context;

        public FavoritesController(TeamContext ctx) => context = ctx;

        [HttpGet]
        public ViewResult Index()
        {
            // Create an instance of NFLSession to access session data
            var session = new NFLSession(HttpContext.Session);

            // Gather user information and favorite teams from session
            var model = new TeamsViewModel
            {
                ActiveConf = session.GetActiveConf(),
                ActiveDiv = session.GetActiveDiv(),
                Teams = session.GetMyTeams(),
                Username = session.GetName() // Add the username here
            };

            return View(model); // Return the populated view model to the view
        }

        [HttpPost]
        public RedirectToActionResult Add(Team team)
        {
            // Fetch the full team data from the database
            team = context.Teams
                 .Include(t => t.Conference)
                 .Include(t => t.Division)
                 .Where(t => t.TeamID == team.TeamID)
                 .FirstOrDefault() ?? new Team();

            // Add the team to the user's favorite teams in session
            var session = new NFLSession(HttpContext.Session);
            var teams = session.GetMyTeams();
            teams.Add(team);
            session.SetMyTeams(teams);

            // Set a message to indicate the addition
            TempData["message"] = $"{team.Name} added to your favorites";

            // Redirect to the Home page
            return RedirectToAction("Index", "Home",
                new
                {
                    ActiveConf = session.GetActiveConf(),
                    ActiveDiv = session.GetActiveDiv()
                });
        }

        [HttpPost]
        public RedirectToActionResult Delete()
        {
            // Remove favorite teams from session
            var session = new NFLSession(HttpContext.Session);
            session.RemoveMyTeams();

            // Set a message to indicate the clearance of favorites
            TempData["message"] = "Favorite teams cleared";

            // Redirect to the Home page
            return RedirectToAction("Index", "Home",
                new
                {
                    ActiveConf = session.GetActiveConf(),
                    ActiveDiv = session.GetActiveDiv()
                });
        }
    }
}
