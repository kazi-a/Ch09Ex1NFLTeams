using Microsoft.AspNetCore.Mvc;
using NFLTeams.Models;

namespace NFLTeams.Controllers
{
    public class NameController : Controller
    {
        private NFLSession _nflSession;

        public NameController(NFLSession nflSession)
        {
            _nflSession = nflSession;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var model = new TeamsViewModel
            {
                ActiveConf = _nflSession.GetActiveConf(),
                ActiveDiv = _nflSession.GetActiveDiv(),
                Teams = _nflSession.GetMyTeams(),
                Username = _nflSession.GetName() 
            };

            return View(model);
        }

        // Change action method for POST requests
        [HttpPost]
        public RedirectToActionResult Change(TeamsViewModel model)
        {
            var session = new NFLSession(HttpContext.Session); 
            session.SetName(model.Username); 

            // Redirect to the Home page with current active conference and division
            return RedirectToAction("Index", "Home", new
            {
                ActiveConf = session.GetActiveConf(),
                ActiveDiv = session.GetActiveDiv()
            });
        }
    }
}
