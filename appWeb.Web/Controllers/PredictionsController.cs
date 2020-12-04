using appWeb.Common.Entities;
using appWeb.Web.Data;
using appWeb.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace appWeb.Web.Controllers
{
    public class PredictionsController : Controller
    {
        private readonly DataContext _context;
        private readonly IAIHelper _iAIHelper;

        public PredictionsController(DataContext context, IAIHelper aIHelper)
        {
            _context = context;
            _iAIHelper = aIHelper;
        }

        public async Task<IActionResult> Index()
        {
            List<History> histories = await _iAIHelper.GetBestAndWorst();
            return View(histories);
        }
    }
}
