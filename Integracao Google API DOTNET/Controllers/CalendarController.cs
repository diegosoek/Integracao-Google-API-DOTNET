using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Integracao_Google_API_DOTNET.Util;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Integracao_Google_API_DOTNET.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
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

                var calendar = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = google.userCredential,
                    ApplicationName = GoogleApi.aplicationName,
                });

                EventsResource.ListRequest request = calendar.Events.List("primary");
                //request.TimeMin = DateTime.Now;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                IList<Google.Apis.Calendar.v3.Data.Event> events = request.Execute().Items;

                Event evento = new Event
                {
                    Summary = "Word of the Day",
                    Start = new EventDateTime
                    {
                        DateTime = DateTime.Now
                    },
                    End = new EventDateTime
                    {
                        DateTime = DateTime.Now
                    },
                    Gadget = new Event.GadgetData
                    {
                        Title = "Word of the Day",
                        Type = "application/x-google-gadgets+xml",
                        IconLink = "https://www.thefreedictionary.com/favicon.ico",
                        Link = "https://www.thefreedictionary.com/_/WoD/wod-module.xml",
                        Width = 300,
                        Height = 136
                    }
                };

                calendar.Events.Insert(evento, "primary").Execute();

                /*
                Event event = new Event();

        event.setSummary("Word of the Day");
        DateTime start = DateTime.parseRfc3339("2007-03-14");
        DateTime end = DateTime.parseRfc3339("2007-03-15");
event.setStart(new EventDateTime().setDate(start));
event.setEnd(new EventDateTime().setDate(end));

        Gadget gadget = new Gadget();

        gadget.setTitle("Word of the Day");
gadget.setType("application/x-google-gadgets+xml");
gadget.setIconLink("http://www.thefreedictionary.com/favicon.ico");
gadget.setLink("http://www.thefreedictionary.com/_/WoD/wod-module.xml");
gadget.setWidth(300);
gadget.setHeight(136);

Map<String, String> prefs = new HashMap<String, String>();
        prefs.put("Format", "0");
prefs.put("Days", "1");
gadget.setPreferences(prefs);

event.setGadget(gadget);

        service.events().insert("primary", event).execute();
        */

            }

            return View();

        }
    }
}