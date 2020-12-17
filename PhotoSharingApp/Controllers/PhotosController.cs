using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhotoSharingApp.Data;
using PhotoSharingApp.Models;

namespace PhotoSharingApp.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public PhotosController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Photos.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos
                .FirstOrDefaultAsync(m => m.PhotoId == id);
            if (photos == null)
            {
                return NotFound();
            }

            return View(photos);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(33554432)] // 32MB
        public async Task<IActionResult> Create(Photos photos)
        {
            if (ModelState.IsValid)
            {
                /*// Validates Photo
                IFormFile photo = photos.Photo;

                if(photo.Length > 0)
                {
                    return View();
                }

                string extension = Path.GetExtension(photo.FileName).ToLower();
                string[] permittedExtensions = { ".png", ".gif", ".jpg" };
                if (!permittedExtensions.Contains(extension))
                {
                    return View();
                }

                // Generate unqiue file name and Save photo to storage
                string acc = _config.GetSection("StorageAccountName").Value;
                string key = _config.GetSection("StorageAccountKey").Value;

                BlobServiceClient blobService = new BlobServiceClient("UseDevelopmentStorage=true");
                BlobContainerClient containerClient = blobService.GetBlobContainerClient("photos"); 
                
                if (!containerClient.Exists())
                {
                    await containerClient.CreateAsync();
                    await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);
                }


                // Add BLOV to container
                string newFileName = Guid.NewGuid().ToString() + extension;
                BlobClient blobClient = containerClient.GetBlobClient(newFileName);

                await blobClient.UploadAsync(photos.Photo.OpenReadStream());

                photos.PhotoUrl = newFileName;*/
                _context.Add(photos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(photos);
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos.FindAsync(id);
            if (photos == null)
            {
                return NotFound();
            }
            return View(photos);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoId,PhotoName,PhotoDescription,PhotoUrl")] Photos photos)
        {
            if (id != photos.PhotoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotosExists(photos.PhotoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(photos);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos
                .FirstOrDefaultAsync(m => m.PhotoId == id);
            if (photos == null)
            {
                return NotFound();
            }

            return View(photos);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photos = await _context.Photos.FindAsync(id);
            _context.Photos.Remove(photos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotosExists(int id)
        {
            return _context.Photos.Any(e => e.PhotoId == id);
        }
    }
}
