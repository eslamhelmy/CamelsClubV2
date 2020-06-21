
using CamelsClub.API.Helpers;
using CamelsClub.Localization.Shared;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class UploadController : BaseController
    {

        private const string _TEMP_PATH = "uploads/temp";
        string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
        string hostName = HttpContext.Current.Request.Url.Authority.ToString();

        [HttpPost]
        public async Task<ResultViewModel<List<UploadedFile>>> Upload()
        {
            ResultViewModel<List<UploadedFile>> resultViewModel = new ResultViewModel<List<UploadedFile>>();
            List<UploadedFile> uploadedFiles = new List<UploadedFile>();
            try
            {
                if (Request.Content.IsMimeMultipartContent())
                {
                    string root = HttpContext.Current.Server.MapPath($"~/{_TEMP_PATH}");
                    if (!Directory.Exists(root))
                        Directory.CreateDirectory(root);
                    var httpRequest = HttpContext.Current.Request;
                    if (httpRequest.Files.Count > 0)
                    {
                        foreach (string file in httpRequest.Files)
                        {
                            var postedFile = httpRequest.Files[file];
                            string fileExtension = postedFile.FileName.Split('.').Last();
                            string fileName = Guid.NewGuid().ToString() + "." + fileExtension;
                            var filePath = HttpContext.Current.Server.MapPath($"/{_TEMP_PATH}/" + fileName);
                            postedFile.SaveAs(filePath);
                            string serverFilePath = protocol + "://" + hostName + $"/{_TEMP_PATH}/" + fileName;
                            uploadedFiles.Add(new UploadedFile
                            {
                                OriginalFileName = postedFile.FileName,
                                NewFileName = fileName,
                              //  Path = serverFilePath,
                                Extension = fileExtension
                            });
                        }
                    }
                    resultViewModel.Data = uploadedFiles;

                }
                else
                {
                    resultViewModel.Success = false;
                    resultViewModel.Message = "Error Ocure";
                    resultViewModel.Errors = "the content must be MimeMultipartContent";
                }
            }
            catch (Exception ex)
            {
                resultViewModel.Success = false;
                resultViewModel.Message = "Error Ocure";
                resultViewModel.Errors = ex.Message;
            }
            return resultViewModel;
        }


        [HttpGet]
        public ResultViewModel<bool> DeleteAttachment(string fileName)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
            try
            {
                string path = HttpContext.Current.Server.MapPath($"~/{_TEMP_PATH}/") + fileName;
                if (File.Exists(path))
                    File.Delete(path);
                resultViewModel.Message = "Successfully Deleted";//Resource.SuccessfullyDeleted;
                resultViewModel.Data = true;
            }
            catch (Exception ex)
            {
                resultViewModel.Success = false;
                resultViewModel.Message = "Successfully Deleted";//Resource.ErrorOccurred;
            }
            return resultViewModel;
        }
    }
}
