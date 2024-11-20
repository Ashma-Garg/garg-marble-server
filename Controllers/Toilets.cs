using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class ToiletsController : ControllerBase
{
    private readonly IMongoCollection<Toilet> _toiletsCollection;

    public ToiletsController(IMongoDatabase database)
    {
        _toiletsCollection = database.GetCollection<Toilet>("Toilets");
    }

    // GET: api/toilets
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var toilets = await _toiletsCollection.Find(_ => true).ToListAsync();
            return Ok(toilets);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving data.", error = ex.Message });
        }
    }

    // GET: api/toilets/{id}
    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var toilet = await _toiletsCollection.Find(w => w.Id == id).FirstOrDefaultAsync();
            if (toilet == null)
                return NotFound(new { message = "Toilet not found." });

            return Ok(toilet);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the toilet.", error = ex.Message });
        }
    }

    // POST: api/toilets
    [HttpPost]
    public async Task<IActionResult> Create(Toilet toilet)
    {
        try
        {
            await _toiletsCollection.InsertOneAsync(toilet);
            return CreatedAtAction(nameof(GetById), new { id = toilet.Id }, toilet);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the toilet.", error = ex.Message });
        }
    }

    // PUT: api/toilets/{id}
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Toilet updatedToilet)
    {
        try
        {
            var result = await _toiletsCollection.ReplaceOneAsync(w => w.Id == id, updatedToilet);
            if (result.MatchedCount == 0)
                return NotFound(new { message = "Toilet not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the toilet.", error = ex.Message });
        }
    }

    // DELETE: api/toilets/{id}
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var result = await _toiletsCollection.DeleteOneAsync(w => w.Id == id);
            if (result.DeletedCount == 0)
                return NotFound(new { message = "Toilet not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the toilet.", error = ex.Message });
        }
    }
}
