using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Prank.DAL;
using PrankAWSServerlessApi.ApiSettings;
using System.IO;


namespace PrankAWSServerlessApi.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected readonly DataContext _dataContext;        
        protected readonly IWebHostEnvironment _env;
        protected readonly FileSettings _fileSettings;

        protected readonly IConfiguration _IConfiguration;

        protected string WebRootPath => _env?.WebRootPath;

        public BaseApiController(DataContext dataContext, IConfiguration config, FileSettings fileSettings = null, IWebHostEnvironment env = null) : base(config)
        {
            _dataContext = dataContext;
            _fileSettings = fileSettings;
            _env = env;
            _IConfiguration = config;
        }
        protected void EnsureFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        protected string MapPath(string path)
        {
            return Path.Combine(WebRootPath, path);
        }
        protected string GetDirPath(string entityType)
        {
            var directoryPathMapping = _fileSettings[entityType];
            if (directoryPathMapping == null)
            {
                return WebRootPath;
            }
            return directoryPathMapping.IsRelative ? MapPath(directoryPathMapping.Path) : directoryPathMapping.Path;
        }
        protected string GetFilePath(string entityType, string fileName)
        {
            return Path.Combine(GetDirPath(entityType), fileName);
        }
    }
}
