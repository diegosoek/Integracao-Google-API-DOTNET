using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Classroom.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using System;

namespace Integracao_Google_API_DOTNET.Util
{
    public class GoogleApi
    {

        public const string clientId = "992304351793-kq028c4l13lg6tvhvt640vn0ti3t5dm6.apps.googleusercontent.com";
        public const string clientSecret = "";
        public const string aplicationName = "Teste API";
        public static string[] scopes = { "openid", "email", DriveService.Scope.DriveReadonly, CalendarService.Scope.Calendar, ClassroomService.Scope.ClassroomCourses, ClassroomService.Scope.ClassroomCourseworkMe, ClassroomService.Scope.ClassroomCourseworkStudents, ClassroomService.Scope.ClassroomStudentSubmissionsMeReadonly };

        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public UserCredential userCredential { get; set; }

        public GoogleApi(string accessToken, string refreshToken)
        {
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
            CreateUserCredential();
        }

        public void CreateUserCredential()
        {

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                Scopes = scopes,
                DataStore = new FileDataStore("Store")
            });

            var token = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            var credential = new UserCredential(flow, Environment.UserName, token);

            userCredential = credential;

        }

    }
}