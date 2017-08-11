using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Integracao_Google_API_DOTNET.Util;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Integracao_Google_API_DOTNET.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            GoogleService googleService = new GoogleService();

            var drive = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleService.ServiceAccountCredential,
                ApplicationName = GoogleApi.aplicationName,
            });

            var list = drive.Files.List();
            var listex = list.Execute();

            string GoogleAccessCode = String.Empty;
            string GoogleRefreshToken = String.Empty;

            try
            {
                GoogleAccessCode = System.Web.HttpContext.Current.Session["GoogleAccessCode"].ToString();
                GoogleRefreshToken = System.Web.HttpContext.Current.Session["GoogleRefreshToken"].ToString();
            }
            catch (Exception e)
            {

            }
            if (GoogleAccessCode != "" && GoogleRefreshToken != "")
            {

                GoogleApi google = new GoogleApi(GoogleAccessCode, GoogleRefreshToken);

                //Chamada Google Drive API
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = google.userCredential,
                    ApplicationName = GoogleApi.aplicationName,
                });

                // Define parameters of request.
                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = 10;
                listRequest.Fields = "nextPageToken, files(id, name)";

                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

                List<string> fileNames = new List<string>();

                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        fileNames.Add(file.Name);
                    }
                }

                ViewBag.files = fileNames;

            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}