using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class TapsController : ControllerBase
{
    private readonly IMongoCollection<Tap> _tapsCollection;

    public TapsController(IMongoDatabase database)
    {
        _tapsCollection = database.GetCollection<Tap>("Taps");
    }

    // GET: api/taps
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var taps = await _tapsCollection.Find(_ => true).ToListAsync();
            return Ok(taps);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving data.", error = ex.Message });
        }
    }

    // GET: api/taps/{id}
    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var tap = await _tapsCollection.Find(w => w.Id == id).FirstOrDefaultAsync();
            if (tap == null)
                return NotFound(new { message = "Tap not found." });

            return Ok(tap);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the tap.", error = ex.Message });
        }
    }

    // POST: api/taps
    [HttpPost]
    public async Task<IActionResult> Create(Tap tap)
    {
        try
        {
            await _tapsCollection.InsertOneAsync(tap);
            return CreatedAtAction(nameof(GetById), new { id = tap.Id }, tap);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the tap.", error = ex.Message });
        }
    }

    // PUT: api/taps/{id}
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Tap updatedTap)
    {
        try
        {
            var result = await _tapsCollection.ReplaceOneAsync(w => w.Id == id, updatedTap);
            if (result.MatchedCount == 0)
                return NotFound(new { message = "Tap not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the tap.", error = ex.Message });
        }
    }

    // DELETE: api/taps/{id}
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var result = await _tapsCollection.DeleteOneAsync(w => w.Id == id);
            if (result.DeletedCount == 0)
                return NotFound(new { message = "Tap not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the tap.", error = ex.Message });
        }
    }
}
