using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todo_api.Models;

namespace todo_api.Controllers
{
    [Produces("application/json")]
    [Route("api/Notes")]
    public class NotesController : Controller
    {
        private readonly TodoApiContext _context;

        public NotesController(TodoApiContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<IEnumerable<Note>> GetNote()
        {
            var z = await _context.Note.Include(s=>s.Labels).Include(y=>y.CheckLists).ToListAsync();
            return z;
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {   
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(s => s.Labels).Include(y => y.CheckLists).SingleOrDefaultAsync(m => m.ID == id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // GET: api/Notes/pin/pinned
        [HttpGet("pin/{pinned}")]
        public async Task<IActionResult> GetNote([FromRoute] bool pinned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(s => s.Labels).Include(y => y.CheckLists).FirstOrDefaultAsync(k => k.Pinned == pinned);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // GET: api/Notes/lbl/label
        [HttpGet("lbl/{label}")]
        public IActionResult GetResult([FromRoute] string label)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = _context.Note.Include(s => s.Labels).Include(y => y.CheckLists).Where(x => x.Labels!=null);
            var result = note.Where(x => x.Labels.Any(c => c.LabelData == label));


            if (note == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // SEARCH: api/notes/title
        [HttpGet("search/{title}")]
        public async Task<IActionResult> GetByTitle([FromRoute] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(s => s.Labels).Include(y => y.CheckLists).FirstOrDefaultAsync(s => s.Title == title);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != note.ID)
            {
                return BadRequest();
            }

            _context.Note.Update(note);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.ID }, note);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(s => s.Labels).Include(y => y.CheckLists).SingleOrDefaultAsync(m => m.ID == id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Note.Remove(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        // DELETE: api/Notes/dlt/title
        [HttpDelete("dlt/{title}")]
        public IActionResult DeleteNote([FromRoute] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = _context.Note.Include(s => s.Labels).Include(y => y.CheckLists).Where(c => c.Title == title);

            if (note == null)
            {
                return NotFound();
            }

            _context.Note.RemoveRange(note);
            _context.SaveChangesAsync();

            return Ok(note);
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.ID == id);
        }
    }
}