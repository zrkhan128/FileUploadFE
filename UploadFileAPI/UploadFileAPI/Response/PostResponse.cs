using UploadFileAPI.Response.Models;

namespace UploadFileAPI.Response
{
    public class PostResponse : BaseResponse
    {
        public DocumentPostModel Post { get; set; }
    }
}
