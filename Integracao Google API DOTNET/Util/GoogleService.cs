using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Json;
using System;
using System.IO;

namespace Integracao_Google_API_DOTNET.Util
{
    public class GoogleService
    {

        public static string[] scopes = { DriveService.Scope.Drive, ClassroomService.Scope.ClassroomCourses };

        public ServiceAccountCredential ServiceAccountCredential { get; set; }

        public GoogleService()
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/service-account-credentials.json");
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var credentialParameters =
                    NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(fs);
                if (credentialParameters.Type != "service_account"
                    || string.IsNullOrEmpty(credentialParameters.ClientEmail)
                    || string.IsNullOrEmpty(credentialParameters.PrivateKey))
                    throw new InvalidOperationException("JSON data does not represent a valid service account credential.");
                this.ServiceAccountCredential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(credentialParameters.ClientEmail)
                    {
                        Scopes = scopes,
                        User = "diego@gedu.demo.mestra.org" //the user to be impersonated
                    }.FromPrivateKey(credentialParameters.PrivateKey));
            }
        }


    }
}