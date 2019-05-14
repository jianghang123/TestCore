using Microsoft.AspNetCore.Http;

namespace TestCore.Framework.Strategy
{
    /// <summary>
    /// 上传图片策略接口
    /// </summary>
    public interface IUploadStrategy
    {
        string UploadImg(IFormFile file, string key = "", string nodeDir = "");
        string UploadProductImg(IFormFile file, string key = "");
        string DelImg(string key = "", string url = "");
    }
}
