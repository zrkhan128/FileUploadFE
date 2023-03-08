
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using UploadFileAPI.Entities;
using UploadFileAPI.Helpers;
using UploadFileAPI.Interfaces;
using UploadFileAPI.Requests;
using UploadFileAPI.Response;
using UploadFileAPI.Response.Models;

namespace FileUploadApi.Services
{
    public class PostService : IDocumentService
    {
        private readonly AppDbContext appDbContext;
        private readonly IWebHostEnvironment environment;

        public PostService(AppDbContext socialDbContext, IWebHostEnvironment environment)
        {
            this.appDbContext = socialDbContext;
            this.environment = environment;
        }

        public async Task<PostResponse> CreateFileAsync(DocumentPostRequest postRequest)
        {
            var post = new Document
            {
                
                Description = postRequest.Description,
                Imagepath = postRequest.ImagePath
            };

            var postEntry = await appDbContext.Documents.AddAsync(post);

            var saveResponse = await appDbContext.SaveChangesAsync();

            if (saveResponse < 0)
            {
                return new PostResponse { Success = false, Error = "Issue while saving the post", ErrorCode = "CP01" };
            }

            var postEntity = postEntry.Entity;
            var postModel = new DocumentPostModel
            {
                Id = postEntity.Id,
                Description = postEntity.Description,
               
                Imagepath = Path.Combine(postEntity.Imagepath),
                

            };

            return new PostResponse { Success = true, Post = postModel };

        }

        public async Task SaveFileAsync(DocumentPostRequest postRequest)
        {
            var uniqueFileName = FileHelper.GetUniqueFileName(postRequest.File.FileName);
            
            var uploads = Path.Combine(environment.WebRootPath, "Documents", "MyDocuments", DateTime.Now.ToString("dd-MM-yyyy-mm-ss"));
            
            var filePath = Path.Combine(uploads, uniqueFileName);
            
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            await postRequest.File.CopyToAsync(new FileStream(filePath, FileMode.Create));
            
            postRequest.ImagePath = filePath;

            return;
        }
    }
}
