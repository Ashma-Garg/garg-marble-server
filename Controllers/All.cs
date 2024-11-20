using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class AllController : ControllerBase
{
    private readonly IMongoCollection<Washbasin> _washbasinsCollection;
    private readonly IMongoCollection<Tap> _tapsCollection;
    private readonly IMongoCollection<Toilet> _toiletsCollection;

    public AllController(IMongoDatabase database)
    {
        _washbasinsCollection = database.GetCollection<Washbasin>("Washbasins");
        _tapsCollection = database.GetCollection<Tap>("Taps");
        _toiletsCollection = database.GetCollection<Toilet>("Toilets");
    }

    // Search API that handles search queries for all collections
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchQuery query)
    {
        var searchString = query.Search.ToLower();

        // Query each collection
        var washbasins = await _washbasinsCollection.Find(w => w.Name.ToLower().Contains(searchString)).ToListAsync();
        var taps = await _tapsCollection.Find(t => t.Name.ToLower().Contains(searchString)).ToListAsync();
        var toilets = await _toiletsCollection.Find(t => t.Name.ToLower().Contains(searchString)).ToListAsync();

        // Combine all results into one list
        var combinedResults = washbasins.Cast<object>()
                                        .Concat(taps.Cast<object>())
                                        .Concat(toilets.Cast<object>())
                                        .ToList();

        return Ok(combinedResults);
    }
}

public class SearchQuery
{
    public string Search { get; set; }
}
