using System;
using System.Linq;
using Microsoft.Extensions.Options;
using PrankAWSWebApp.Utility;
using PrankAWSWebApp.Factory;
using Microsoft.AspNetCore.Identity;
using PrankAWSWebApp.Areas.Admin.Data;
using Microsoft.Extensions.Configuration;
using PrankAWSWebApp.Common;
using Microsoft.AspNetCore.Mvc;
using PrankAWSWebApp.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using PrankAWSWebApp.Areas.Admin.Models;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Prank.Model;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using PrankAWSWebApp.AwsS3;
using Microsoft.AspNetCore.Authorization;

namespace PrankAWSWebApp.Areas.Admin.Controllers
{
  [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class PrankController : Controller
    {
        IConfiguration _iconfiguration;
        private readonly IOptions<MySettingsModel> appSettings;
        private readonly SignInManager<PrankIdentityUser> loginManager;
        private readonly UserManager<PrankIdentityUser> userManager;
        private readonly IHostingEnvironment hostEnvironment;

        private Task<PrankIdentityUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);
        public string EmployeeId
        {
            get
            {
                var task = Task.Run(async () => await GetCurrentUserAsync());
                return task.Result.Id;
            }
        }

        private readonly IAwsS3HelperService _awsS3Service;

        private AmazonS3Client _s3Client = new AmazonS3Client(RegionEndpoint.APSouth1);
        private string _bucketName = "prankapibucket";// "mis-pdf-library";//this is my Amazon Bucket name
        private static string _bucketSubdirectory = "Images";// String.Empty;



        private const string filePath = null;
        // Specify your bucket region (an example region is shown).  
        private static readonly string bucketName = "prankapibucket";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;
        private static readonly string accesskey = "AKIAWAXVQOY4K5MNJGGH";
        private static readonly string secretkey = "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";


        public PrankController(IHostingEnvironment environment, IOptions<MySettingsModel> app, SignInManager<PrankIdentityUser> loginManager, IConfiguration iconfiguration, UserManager<PrankIdentityUser> userManager, IAwsS3HelperService awsS3Service)
        {
            appSettings = app;
            ApplicationSettings.WebApiUrl = appSettings.Value.WebApiBaseUrl;
            this.loginManager = loginManager;
            this.userManager = userManager;
            _iconfiguration = iconfiguration;
            hostEnvironment = environment;
            _awsS3Service = awsS3Service;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PartialPrankInfoLst(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            string token = string.Empty;
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
            if (userTokenClaim != null)
            {
                token = userTokenClaim.Value;
            }
            try
            {
                string sortCol = string.Empty;
                switch (iSortCol_0)
                {

                    case 0:
                        {
                            sortCol = "PrankName";
                            break;
                        }
                    case 1:
                        {
                            sortCol = "PrankDesc";
                            break;
                        }
                    case 2:
                        {
                            sortCol = "IsActive";
                            break;
                        }
                    case 3:
                        {
                            sortCol = "AddedDate";
                            break;
                        }
                    case 4:
                        {
                            sortCol = "ModifiedDate";
                            break;
                        }

                    default:
                        {
                            sortCol = "PrankId";
                            break;
                        }
                }
                var data = await ApiClientFactory.Instance.GetPrankInfo(token, iDisplayStart, iDisplayLength, sSearch, sortCol, sSortDir_0);
                
                var result = new
                {
                    iTotalRecords = data.Any() ? data.FirstOrDefault().TotalCount : 0,
                    iTotalDisplayRecords = data.Any() ? data.FirstOrDefault().TotalCount : 0,
                    aaData = data
                };
                return Json(result);
            }
            catch (Exception e)
            {
                throw;
            }
        }
 
        public async Task<IActionResult> PartialAddEditPrankInfo(int prankId)
        {
            var model = new PrankAddEditModel();
            if (prankId > 0)
            {
                try
                {
                    string token = string.Empty;
                    var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                    var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                    if (userTokenClaim != null)
                    {
                        token = userTokenClaim.Value;
                    }
                    var result = await ApiClientFactory.Instance.GetPrankInfoById(token, prankId);
                    if (result != null & result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            model.PrankId = item.PrankId;
                            model.PrankName = item.PrankName;
                            model.PrankDesc = item.PrankDesc;
                            model.PrankImage = item.PrankImage;
                            model.PreviewAudioFile = item.PreviewAudioFile;
                            model.MainAudioFile = item.MainAudioFile;
                            model.PlivoAudioXmlUrl = item.PlivoAudioXmlUrl;
                            model.IsActive = item.IsActive;
                            model.AddedDate = item.AddedDate;
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
            string viewFromCurrentController = await this.RenderViewToStringAsync("PartialAddEditPrankInfo", model);
            return Json(new { html = viewFromCurrentController });
        }

        public ActionResult SavePrankImageRequest()
        {
            bool isCorrectFile = false;
            var StrMessage = SaveImage(ref isCorrectFile);
            return Json(new { response = StrMessage, isSuccess = isCorrectFile });
        }
        public ActionResult SavePrankAudioRequest()
        {
            bool isCorrectFile = false;
            var StrMessage = SaveAudio(ref isCorrectFile);
            return Json(new { response = StrMessage, isSuccess = isCorrectFile });
        }

        public async Task<bool> uploadToBucket(IFormFile file)
        {
            var fileName = ContentDispositionHeaderValue
               .Parse(file.ContentDisposition)
               .FileName
               .TrimStart().ToString();
            var folderName = "Images";// Request.Form.ContainsKey("folder") ? Request.Form["folder"].ToString() : null;
            bool status = false;
            using (var fileStream = file.OpenReadStream())
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(ms);
                        status = await _awsS3Service.UploadFileAsync(ms, fileName, folderName);
                    }
                }
                catch (Exception ex)
                {
                    string str = ex.ToString();
                }
            return status;
        }
        public void uploadToS3(string filePath)
        {
            try
            {
                TransferUtility fileTransferUtility = new
                    TransferUtility(new AmazonS3Client(Amazon.RegionEndpoint.APSouth1));

                string bucketName;


                if (_bucketSubdirectory == "" || _bucketSubdirectory == null)
                {
                    bucketName = _bucketName; //no subdirectory just bucket name  
                }
                else
                {   // subdirectory and bucket name  
                    bucketName = _bucketName + @"/" + _bucketSubdirectory;
                }


                // 1. Upload a file, file name is used as the object key name.
                fileTransferUtility.Upload(filePath, bucketName);
                Console.WriteLine("Upload 1 completed");


            }
            catch (Exception ex)
            {
                string str = ex.ToString();
            }
            //catch (AmazonS3Exception s3Exception)
            //{
            //    Console.WriteLine(s3Exception.Message,
            //                      s3Exception.InnerException);
            //}
        }

        public bool sendMyFileToS3(System.IO.Stream localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3)
        {
            //IAmazonS3 client = new AmazonS3Client(RegionEndpoint.APSouth1);
            try
            {
                IAmazonS3 client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

                TransferUtility utility = new TransferUtility(client);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                if (subDirectoryInBucket == "" || subDirectoryInBucket == null)
                {
                    request.BucketName = bucketName; //no subdirectory just bucket name  
                }
                else
                {   // subdirectory and bucket name  
                    request.BucketName = bucketName + @"/" + subDirectoryInBucket;
                }
                request.Key = fileNameInS3; //file name up in S3  
                request.InputStream = localFilePath;
                request.CannedACL = S3CannedACL.PublicReadWrite;
                utility.Upload(request); //commensing the transfer  

                return true; //indicate that the file was sent  
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
                return false;
            }
        }

        public void UploadToBucket(IFormFile file, string keyName)
        {
            var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

            var fileTransferUtility = new TransferUtility(s3Client);
            try
            {
                //if (file.ContentLength > 0)
                //{
                var filePath = "~/Images";// Path.Combine("~/Images", Path.GetFileName(file.FileName)); //Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(file.FileName));
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    FilePath = filePath,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456, // 6 MB.  
                    Key = keyName,
                    CannedACL = S3CannedACL.PublicRead
                };
                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");
                fileTransferUtility.Upload(fileTransferUtilityRequest);
                fileTransferUtility.Dispose();
                //}
                ViewBag.Message = "File Uploaded Successfully!!";
            }

            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    ViewBag.Message = "Check the provided AWS Credentials.";
                }
                else
                {
                    ViewBag.Message = "Error occurred: " + amazonS3Exception.Message;
                }
            }
        }
        public string SaveImage(ref bool isCorrectFile)
        {
            try
            {
                string fileName = string.Empty;
                var httpRequest = HttpContext.Request.Form;

                if (httpRequest.Files.Count > 0)
                {
                    foreach (var formFile in httpRequest.Files)
                    {
                        if (formFile.Length > 0)
                        {
                            fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                            var validExtensions = new List<string>() { ".jpeg", ".jpg", ".gif", ".png" };

                            if (!validExtensions.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase))
                            {
                                isCorrectFile = false;
                                return "File upload process failed due to invalid file format.";
                            }
                            else
                            {
                                Stream st = formFile.OpenReadStream();
                                string myBucketName = "prankapibucket";
                                string s3DirectoryName = "Images";
                                isCorrectFile = sendMyFileToS3(st, myBucketName, s3DirectoryName, fileName);
                            }
                        }
                    }
                }               
                return fileName;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string SaveAudio(ref bool isCorrectFile)
        {
            string fileName = string.Empty;
            var httpRequest = HttpContext.Request.Form;

            if (httpRequest.Files.Count > 0)
            {
                foreach (var formFile in httpRequest.Files)
                {
                    if (formFile.Length > 0)
                    {
                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                        //    var filePath = Path.Combine(hostEnvironment.WebRootPath + "\\Images\\PrankImages", fileName);//Path.GetTempFileName();

                        var filePath = Path.Combine("prankapibucket.s3.ap-south-1.amazonaws.com" + "\\Audio", fileName);

                        Stream st = formFile.OpenReadStream();// FileUpload1.PostedFile.InputStream;
                        string name = fileName;// Path.GetFileName(FileUpload1.FileName);
                        string myBucketName = "prankapibucket"; //your s3 bucket name goes here  
                        string s3DirectoryName = "Audio";
                        string s3FileName = @name;
                        isCorrectFile = sendMyFileToS3(st, myBucketName, s3DirectoryName, fileName);

                        var validExtensions = new List<string>() { ".mp3", ".wav" };

                        if (!validExtensions.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase))
                        {
                            isCorrectFile = false;
                            return "File upload process failed due to invalid file format.";
                        }
                        //if (!Directory.Exists(filePath))
                        //{
                        //    using (var stream = System.IO.File.Create(filePath))
                        //    {
                        //        formFile.CopyTo(stream);
                        //        isCorrectFile = true;
                        //    }
                        //}
                    }
                }
            }
            //  isCorrectFile = true;
            return fileName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveUpdate(PrankAddEditModel prankModel)
        {
            var data = new ResponseModel();
            var objResponse = new SaveResponse();
            string viewFromCurrentController = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    string token = string.Empty;
                    var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                    var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                    if (userTokenClaim != null)
                    {
                        token = userTokenClaim.Value;
                    }
                    var model = new PrankInfoModel()
                    {
                        PrankId = prankModel.PrankId,
                        PrankName = prankModel.PrankName,
                        PrankDesc = prankModel.PrankDesc,
                        PrankImage = prankModel.PrankImage,
                        PreviewAudioFile = prankModel.PreviewAudioFile,
                        MainAudioFile = prankModel.MainAudioFile,
                        PlivoAudioXmlUrl = prankModel.PlivoAudioXmlUrl,
                        IsActive = prankModel.IsActive,
                    };
                    if (model.PrankId > 0)
                    {
                        data = await ApiClientFactory.Instance.UpdatePrankInfo(token, model);
                    }
                    else
                    {
                        data = await ApiClientFactory.Instance.SavePrankInfo(token, model);
                    }

                   
                    objResponse.StatusCode = Convert.ToInt32(data.StatusCode);
              
                    if (Convert.ToInt32(data.StatusCode) == 200)
                    {
                        objResponse.Message = model.PrankId > 0 ? "Record updated successfully" : "Record saved successfully";
                        await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, prankModel.PrankName,
                           prankModel.PrankId, prankModel.PrankId > 0 ? "Update" : "Save");
                    }
                    else
                    {
                        objResponse.Message = model.PrankId > 0 ? "Record not updated successfully" : "Record not saved successfully";
                    }

                    return new JsonResult(new
                    {
                        objResponse
                    });
                }
                catch (Exception ex)
                {
                    string exep = ex.ToString();
                }
            }
            return new JsonResult(new List<string>());
        }

        [HttpGet]
        public async Task<JsonResult> DeletePrankInfo(int id)
        {
            var objResponse = new SaveResponse();
            string viewFromCurrentController = string.Empty;
            if (id > 0)
            {
                string token = string.Empty;
                var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                if (userTokenClaim != null)
                {
                    token = userTokenClaim.Value;
                }
                bool isDeleted = await ApiClientFactory.Instance.DeletePrankinfo(token, id);
                if (isDeleted)
                {
                    objResponse.StatusCode = 200;
                    objResponse.Message = "Record deleted successfully";
                    await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, id.ToString(), id, "Delete");
                }
                else
                {
                    objResponse.Message = "Record not deleted successfully";
                }
            }
            else
            {
                objResponse.Message = "Please click on package";
            }

            return new JsonResult(new
            {
                objResponse
            });


        }

    }
}