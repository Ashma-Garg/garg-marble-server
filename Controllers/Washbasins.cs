using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class WashbasinsController : ControllerBase
{
    private readonly IMongoCollection<Washbasin> _washbasinsCollection;

    public WashbasinsController(IMongoDatabase database)
    {
        _washbasinsCollection = database.GetCollection<Washbasin>("Washbasins");
    }

    // GET: api/washbasins
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var washbasins = await _washbasinsCollection.Find(_ => true).ToListAsync();
            return Ok(washbasins);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving data.", error = ex.Message });
        }
    }

    // GET: api/washbasins/{id}
    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var washbasin = await _washbasinsCollection.Find(w => w.Id == id).FirstOrDefaultAsync();
            if (washbasin == null)
                return NotFound(new { message = "Washbasin not found." });

            return Ok(washbasin);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the washbasin.", error = ex.Message });
        }
    }

    // POST: api/washbasins
    [HttpPost]
    public async Task<IActionResult> Create(Washbasin washbasin)
    {
        try
        {
            await _washbasinsCollection.InsertOneAsync(washbasin);
            return CreatedAtAction(nameof(GetById), new { id = washbasin.Id }, washbasin);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the washbasin.", error = ex.Message });
        }
    }

    // PUT: api/washbasins/{id}
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Washbasin updatedWashbasin)
    {
        try
        {
            var result = await _washbasinsCollection.ReplaceOneAsync(w => w.Id == id, updatedWashbasin);
            if (result.MatchedCount == 0)
                return NotFound(new { message = "Washbasin not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the washbasin.", error = ex.Message });
        }
    }

    // DELETE: api/washbasins/{id}
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var result = await _washbasinsCollection.DeleteOneAsync(w => w.Id == id);
            if (result.DeletedCount == 0)
                return NotFound(new { message = "Washbasin not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the washbasin.", error = ex.Message });
        }
    }
}
