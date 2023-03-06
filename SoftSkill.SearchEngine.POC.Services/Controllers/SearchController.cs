using Microsoft.AspNetCore.Mvc;
using SoftSkill.SearchEngine.POC.Services.Services;

namespace SoftSkill.SearchEngine.POC.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        public readonly ISearchableModel<T> _searchableModel;

        public SearchController(ISearchableModel searchableModel)
        {
            _searchableModel = searchableModel;
        }

        [HttpGet]
        public IActionResult Get(string query)
        {
            _searchableModel.GetSearchableContent(query);

        }
    }
}
