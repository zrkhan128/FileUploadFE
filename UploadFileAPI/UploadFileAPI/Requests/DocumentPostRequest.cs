using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace UploadFileAPI.Requests
{
    public class DocumentPostRequest
    {
        public string Description { get; set; }
        public IFormFile File { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string? ImagePath { get; set; }
    }
}