using System.Threading.Tasks;
using UploadFileAPI.Requests;
using UploadFileAPI.Response;

namespace UploadFileAPI.Interfaces
{
    public interface IDocumentService
    {
        Task SaveFileAsync(DocumentPostRequest postRequest);
        Task<PostResponse> CreateFileAsync(DocumentPostRequest postRequest);
    }
}