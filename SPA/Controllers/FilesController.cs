using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SPA.Models;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace SPA.Controllers
{


    [ApiController]
    [Route("api/modfiles")]
    public class FilesController : ControllerBase
    {
        FilesContext _context;

        public FilesController(FilesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<ModFile> Get()
        {
            IEnumerable<ModFile> modFiles = _context.Files.Where(f => f.User == User.Identity.Name);
            return modFiles;
        }

        [HttpPost]
        public async Task<IActionResult> UploadToDatabase([FromForm] IFormFile file)
        {
            byte[] Blob;
            byte[] HashCode;
            using (var dataStream = new MemoryStream())
            {
                await file.CopyToAsync(dataStream);
                Blob = dataStream.ToArray();
            }
            using (SHA256 sha256 = SHA256.Create())
            {
                HashCode = sha256.ComputeHash(Blob);
            }
            var existModBlob = await _context.ModBlobs.FirstOrDefaultAsync(f => f.HashCode == HashCode);
            var fileName = file.FileName;
            var extension = file.ContentType;
            if (existModBlob != null)
            {

                var existModFile = await _context.Files.FirstOrDefaultAsync(f => f.ModBlobId == existModBlob.Id && f.User == User.Identity.Name);
                if(existModFile != null)
                {
                    return Ok();
                }
                var fileM = new ModFile
                {
                    ModBlob = existModBlob,
                    Date = DateTime.Now.ToString("yyyy/MM/dd"),
                    ContentType = extension,
                    Name = fileName,
                    User = User.Identity.Name
                };
                _context.Files.Add(fileM);
                await _context.SaveChangesAsync();
                return Ok(file);
            }
                var FileBlob = new ModBlob
                {
                    Blob = Blob,
                    HashCode = HashCode
                };
                _context.ModBlobs.Add(FileBlob);
            var fileModel = new ModFile
            {
                ModBlob = FileBlob,
                Date = DateTime.Now.ToString("yyyy/MM/dd"),
                ContentType = extension,
                Name = fileName,
                User = User.Identity.Name
            };
            _context.Files.Add(fileModel);
            await _context.SaveChangesAsync();
            return Ok(file);
        }

        [HttpGet]
        [Route("download/{id}")]
        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {
            var file = await _context.Files.Include(f => f.ModBlob).FirstOrDefaultAsync(x => x.Id == id);
            if (file == null) return null;
            return File(file.ModBlob.Blob, file.ContentType, file.Name);
        }
        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFileFromDatabase([FromRoute] int id)
        {
            var file = await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
            int BlobId = file.ModBlobId;
            _context.Files.Remove(file);
            int FilesCount = _context.Files.Count(f => f.ModBlobId == id);
            if(FilesCount == 1)
            {
                _context.ModBlobs.Remove(_context.ModBlobs.FirstOrDefault(m => m.Id == BlobId));
            }
            _context.SaveChanges();
            return Ok();
        }
    }
}