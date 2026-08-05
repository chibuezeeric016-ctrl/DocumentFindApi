using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocumentFindApi.Data;
using DocumentFindApi.Models;

namespace DocumentFindApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
        {
            return await _context.Documents.ToListAsync();
        }

        // GET: api/documents/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            var document = await _context.Documents.FindAsync(id);

            if (document == null)
                return NotFound();

            return document;
        }

        // POST: api/documents
        [HttpPost]
        public async Task<ActionResult<Document>> PostDocument(Document document)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
        }
        // GET: api/documents/search?fileName=report&fileType=pdf&tag=invoice
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Document>>> SearchDocuments(
            [FromQuery] string? fileName,
            [FromQuery] string? fileType,
            [FromQuery] string? tag)
        {
            var query = _context.Documents.AsQueryable();

            if (!string.IsNullOrEmpty(fileName))
                query = query.Where(d => d.FileName.Contains(fileName));

            if (!string.IsNullOrEmpty(fileType))
                query = query.Where(d => d.FileType == fileType);

            if (!string.IsNullOrEmpty(tag))
                query = query.Where(d => d.Tags != null && d.Tags.Contains(tag));

            return await query.ToListAsync();
        }
        // POST: api/documents/upload
        [HttpPost("upload")]
        public async Task<ActionResult<Document>> UploadDocument(IFormFile file, [FromForm] string? tags)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new Document
            {
                FileName = file.FileName,
                FileType = Path.GetExtension(file.FileName).TrimStart('.'),
                FileSizeBytes = file.Length,
                FilePath = filePath,
                Tags = tags
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
        }
    }
}