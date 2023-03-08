using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UploadFileAPI.Interfaces;
using UploadFileAPI.Requests;
using UploadFileAPI.Response;

namespace UploadFileAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> logger;
        private readonly IDocumentService postService;

        public DocumentController(ILogger<DocumentController> logger, IDocumentService postService)
        {
            this.logger = logger;
            this.postService = postService;
        }

        [HttpPost]
        [Route("")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> SubmitDocument([FromForm] DocumentPostRequest postRequest)
        {
            if (postRequest == null)
            {
                return BadRequest(new PostResponse { Success = false, ErrorCode = "001", Error = "Invalid post request" });
            }

            if (string.IsNullOrEmpty(Request.GetMultipartBoundary()))
            {
                return BadRequest(new PostResponse { Success = false, ErrorCode = "002", Error = "Invalid post header" });
            }

            if (postRequest.File != null)
            {
                await postService.SaveFileAsync(postRequest);
            }

            var postResponse = await postService.CreateFileAsync(postRequest);

            if (!postResponse.Success)
            {
                return NotFound(postResponse);
            }

            return Ok(postResponse.Post);

        }
    }
}
