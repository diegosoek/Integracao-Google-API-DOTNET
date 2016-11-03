using Google.Apis.Classroom.v1;
using Google.Apis.Services;
using Integracao_Google_API_DOTNET.Util;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Integracao_Google_API_DOTNET.Controllers
{
    public class ClassroomController : Controller
    {
        // GET: Classroom
        public ActionResult Index()
        {

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

                var classroom = new ClassroomService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = google.userCredential,
                    ApplicationName = GoogleApi.aplicationName,
                });

                IList<Google.Apis.Classroom.v1.Data.Course> courses = classroom.Courses.List().Execute().Courses;

                foreach(var course in courses)
                {
                    IList<Google.Apis.Classroom.v1.Data.CourseWork> works = classroom.Courses.CourseWork.List(course.Id).Execute().CourseWork;
                    if(works != null && works.Count > 0)
                    {
                        foreach(var work in works)
                        {
                            IList<Google.Apis.Classroom.v1.Data.StudentSubmission> submissions = classroom.Courses.CourseWork.StudentSubmissions.List(course.Id, work.Id).Execute().StudentSubmissions;
                            if(submissions != null && submissions.Count > 0)
                            {
                                ViewBag.work = work;
                                ViewBag.submissions = submissions;
                                break;
                            }
                        }
                        if(ViewBag.work != null)
                        {
                            break;
                        }
                    }
                }

                ViewBag.courses = courses;


                /*
                Google.Apis.Classroom.v1.Data.UserProfile perfil = classroom.UserProfiles.Get("me").Execute();
                if(perfil.)

                */

                CoursesResource.ListRequest request = classroom.Courses.List();
                //Usuário logado como aluno
                request.StudentId = "me";

                courses = request.Execute().Courses;

                if(courses != null && courses.Count > 0)
                {
                    ViewBag.turmasAluno = courses;
                }

                request = classroom.Courses.List();
                //Usuário logado como aluno
                request.TeacherId = "me";

                courses = request.Execute().Courses;

                if (courses != null && courses.Count > 0)
                {
                    ViewBag.turmasProf = courses;
                }


            }

                return View();
        }
    }
}