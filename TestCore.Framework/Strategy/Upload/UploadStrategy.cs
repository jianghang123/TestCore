using TestCore.Common.Encryption;
using TestCore.Common.Extensions;
using TestCore.Common.Helper;
using TestCore.Common.Infrastructure;
using TestCore.Common.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using TestCore.Common.ResponseResult;
using AS = TestCore.IService.Singleton.AttachmentSettingsSingleton;

namespace TestCore.Framework.Strategy
{
    /// <summary>
    /// 上传图片策略
    /// </summary>
    public class UploadStrategy : IUploadStrategy
    {
        #region Fields 

        private readonly IHostingEnvironment hostingEnv;
        private readonly DESEncrypt desEncrypt;
        private readonly ITestCoreFileProvider fileProvider;

        #endregion

        #region Ctor

        public UploadStrategy(IHostingEnvironment hostingEnv, 
            DESEncrypt desEncrypt,
            ITestCoreFileProvider fileProvider)
        {
            this.hostingEnv = hostingEnv;
            this.desEncrypt = desEncrypt;
            this.fileProvider = fileProvider;
        }

        #endregion

        #region 上传普通图片,不生成相关大小配置图片
        /// <summary>
        /// 上传图片,不生成相关大小配置图片
        /// </summary>
        /// <param name="key">秘钥</param>
        /// <param name="nodeDir">目录</param>
        /// <returns></returns>
        public string UploadImg(IFormFile file, string key = "", string nodeDir = "")
        {
            MessagesData<FileViewModel> r = new MessagesData<FileViewModel>()
            {
                Success = false,
                Msg = "服务无响应"
            };
            try
            {
                if (key == AS.Singleton.FileServerMD5Key)
                { 
                    if (file != null)
                    {
                        long fileSize = file.Length;
                        if (fileSize > AS.Singleton.UploadImgMaxSize * 1024)
                        {
                            r.Msg = "上传图片大小不能超过" + AS.Singleton.UploadImgMaxSize + "KB";
                        }
                        else
                        {
                            //获取文件名
                            string uploadFileName = file.FileName;
                            //获取文件扩展名
                            string extension = Path.GetExtension(uploadFileName).ToLowerInvariant();
                            if (AS.Singleton.UploadImgExt.Contains(extension.TrimStart('.')))
                            {
                                //路径格式化
                                string path = PathFormatter.Format(AS.Singleton.UploadFilePathRule, nodeDir);
                                //文件名格式化
                                string fileName = FileNameFormatter.Format(uploadFileName, AS.Singleton.FileNameRule); 
                                //保存路径
                                string savePath = AS.Singleton.UploadDir + "\\" + path.Replace("/", "\\");
                                //访问地址
                                string url = $"{AS.Singleton.UploadUrl}/";
                                //若设置为上传至共享目录，否则上传至当前服务目录wwwroot中
                                if (AS.Singleton.EnabledUploadShare == "false")  
                                {
                                    savePath = fileProvider.Combine(hostingEnv.WebRootPath, savePath);
                                    url += $"{AS.Singleton.UploadDir}/";
                                }
                                //创建保存目录
                                fileProvider.CreateDirectory(savePath);
                                //文件全名（包括路径和文件名）
                                string fileFullName = fileProvider.Combine(savePath, fileName);
                                using (FileStream fs = File.Create(fileFullName))
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();
                                }
                                url += $"{path}/{fileName}";
                                r.Success = true;
                                r.Msg = "上传成功";
                                //将图片URL加密成guid，用于上传时可删除图片操作
                                r.Data = new FileViewModel(desEncrypt.Encrypt(url), url, null, null);
                            }
                            else
                            {
                                r.Msg = "上传图片扩展名只允许为" + AS.Singleton.UploadImgExt;
                            }
                        }
                    }
                    else
                    {
                        r.Msg = "上传图片大小为0";
                    }
                }
                else
                {
                    r.Msg = "无上传图片权限";
                }
            }
            catch (UnauthorizedAccessException)
            {
                r.Msg = "文件系统权限不足";
            }
            catch (DirectoryNotFoundException)
            {
                r.Msg = "路径不存在";
            }
            catch (IOException)
            {
                r.Msg = "文件系统读取错误";
            }
            catch (Exception)
            {
                r.Msg = "上传出错";
            }
            return r.ToJson();
        }

        #endregion

        #region 上传商品图片，完全按上传商品图片系统配置要求

        /// <summary>
        /// 上传商品图片
        /// </summary>
        /// <param name="key">秘钥</param> 
        /// <returns></returns>
        public string UploadProductImg(IFormFile file, string key = "")
        {
            MessagesData<FileViewModel> r = new MessagesData<FileViewModel>()
            {
                Success = false,
                Msg = "服务无响应"
            };
            try
            {
                if (key == AS.Singleton.FileServerMD5Key)
                { 
                    if (file != null)
                    {
                        long fileSize = file.Length;
                        if (fileSize > AS.Singleton.UploadImgMaxSize * 1024)
                        {
                            r.Msg = "上传图片大小不能超过" + AS.Singleton.UploadImgMaxSize + "KB";
                        }
                        else
                        {
                            //获取上传文件名
                            string uploadFileName = file.FileName;
                            //获取文件扩展名
                            string extension = Path.GetExtension(uploadFileName).ToLowerInvariant();
                            if (AS.Singleton.UploadImgExt.Contains(extension.TrimStart('.')))
                            {
                                //路径格式化
                                string dirPath = PathFormatter.Format(AS.Singleton.UploadFilePathRule, "product");
                                //原图路径
                                string sourceDirPath = dirPath + "/source";
                                //文件名格式化
                                string fileName = FileNameFormatter.Format(uploadFileName, AS.Singleton.FileNameRule); 
                                //定义原图保存路径
                                string saveSourcePath = AS.Singleton.UploadDir + "\\" + sourceDirPath.Replace("/", "\\"); 
                                //访问地址
                                string url = $"{AS.Singleton.UploadUrl}/";
                                //若设置为上传至共享目录，否则上传至当前服务目录wwwroot中
                                if (AS.Singleton.EnabledUploadShare == "false")  
                                {
                                    saveSourcePath = fileProvider.Combine(hostingEnv.WebRootPath, saveSourcePath);
                                    url += $"{AS.Singleton.UploadDir}/";
                                }
                                //创建保存原图目录   
                                fileProvider.CreateDirectory(saveSourcePath);
                                //原文件全名
                                string fileFullName = fileProvider.Combine(saveSourcePath, fileName);
                                using (FileStream fs = File.Create(fileFullName))
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();
                                }  
                                if (AS.Singleton.WatermarkType == 1)//文字水印
                                {
                                    string path = string.Format("{0}\\{1}_text{2}", saveSourcePath, fileName.Substring(0, fileName.LastIndexOf('.')), extension);
                                    IOHelper.GenerateTextWatermark(fileFullName, path, AS.Singleton.WatermarkText, AS.Singleton.WatermarkTextSize, AS.Singleton.WatermarkTextFont, AS.Singleton.WatermarkPosition, AS.Singleton.WatermarkQuality);
                                    fileFullName = path;
                                }
                                else if (AS.Singleton.WatermarkType == 2)//图片水印
                                {
                                    string path = string.Format("{0}\\{1}_img{2}", saveSourcePath, fileName.Substring(0, fileName.LastIndexOf('.')), extension);
                                    //放在本应用图片目录下的水印图片路径
                                    string watermarkPath = hostingEnv.WebRootPath + @"\images\" + AS.Singleton.WatermarkImg;
                                    //创建水印图片
                                    IOHelper.GenerateImageWatermark(fileFullName, watermarkPath, path, AS.Singleton.WatermarkPosition, AS.Singleton.WatermarkImgOpacity, AS.Singleton.WatermarkQuality);
                                    fileFullName = path;
                                } 
                                string[] sizeList = StringHelper.SplitString(AS.Singleton.ProductShowThumbSize);
                                foreach (string size in sizeList)
                                {
                                    string thumbDirPath = string.Format("{0}/thumb{1}/", dirPath, size);
                                    thumbDirPath = AS.Singleton.UploadDir + "\\" + thumbDirPath.Replace("/", "\\");
                                    //若设置为上传至共享目录，否则上传至当前服务目录wwwroot中
                                    if (AS.Singleton.EnabledUploadShare == "false")  
                                    {
                                        thumbDirPath = fileProvider.Combine(hostingEnv.WebRootPath, thumbDirPath);
                                    }
                                    //创建缩略图目录   
                                    fileProvider.CreateDirectory(thumbDirPath); 
                                    string[] widthAndHeight = StringHelper.SplitString(size, "_");
                                    IOHelper.GenerateThumb(fileFullName,
                                                           thumbDirPath + fileName,
                                                           widthAndHeight[0].AsInt(),
                                                           widthAndHeight[1].AsInt(),
                                                           "H");
                                }
                                url += $"{sourceDirPath}/{fileName}";
                                r.Success = true;
                                r.Msg = "上传成功";
                                r.Data = new FileViewModel(desEncrypt.Encrypt(url), url, null, null);
                            }
                            else
                            {
                                r.Msg = "上传图片扩展名只允许为" + AS.Singleton.UploadImgExt;
                            }
                        }
                    }
                    else
                    {
                        r.Msg = "上传图片大小为0";
                    }
                }
                else
                {
                    r.Msg = "无上传图片权限";
                }
            }
            catch (UnauthorizedAccessException)
            {
                r.Msg = "文件系统权限不足";
            }
            catch (DirectoryNotFoundException)
            {
                r.Msg = "路径不存在";
            }
            catch (IOException)
            {
                r.Msg = "文件系统读取错误";
            }
            catch (Exception)
            {
                r.Msg = "上传出错";
            }
            return r.ToJson();
        }

        #endregion

        #region 删除图片

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="key">秘钥</param>
        /// <param name="url">加密的URL地址（guid）</param>
        /// <returns></returns>
        public string DelImg(string key = "", string url = "")
        {
            //TODO：由于图片跨度太大，之前上传的图片无法访问，为了允许可以重新上传，直接设置为删除成功
            Messages r = new Messages()
            {
                Success = true,
                Msg = "参数错误"
            };
            try
            {
                string mekey = key;
                string syskey = AS.Singleton.FileServerMD5Key;
                if (key == AS.Singleton.FileServerMD5Key)
                {
                    if (url.Length > 0)
                    {
                        string fileName = desEncrypt.Decrypt(url);
                        string file = fileName.Substring(fileName.Replace("//", "||").IndexOf("/"));
                        string path = string.Empty;
                        if (AS.Singleton.EnabledUploadShare == "true")
                        {
                            path = AS.Singleton.UploadDir + file.Replace("/", "\\");
                        }
                        else
                        {
                            path = fileProvider.Combine(hostingEnv.WebRootPath, file);
                        }
                        fileProvider.DeleteFile(path);
                        r.Msg = "删除成功!";
                    }
                    else
                    {
                        r.Msg = "无效参数";
                    }
                }
                else
                {
                    r.Msg = "无删除权限";
                }
            }
            catch (UnauthorizedAccessException)
            {
                r.Msg = "文件系统权限不足";
            }
            catch (DirectoryNotFoundException)
            {
                r.Msg = "路径不存在";
            }
            catch (IOException)
            {
                r.Msg = "文件系统读取错误";
            }
            catch (Exception)
            {
                r.Msg = "删除出错";
            }
            return r.ToJson();
        }

        #endregion

    }
}
