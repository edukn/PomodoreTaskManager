using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
using Google.Apis.Util.Store;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using System.Web.Mvc;

namespace TaskManagerV2.Controllers
{
    public class AppAuthFlowMetadata : FlowMetadata
    {
        private static readonly IAuthorizationCodeFlow flow =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = "273302838981-77cl7lcb6g8lq8uvu775f0fibeqc25hq.apps.googleusercontent.com",
                    ClientSecret = "mloeicE_QofFj9jqQsay9XE4"
                },
                Scopes = new[] { CalendarService.Scope.Calendar },
                DataStore = new FileDataStore("TaskManagerV2")
               
            });
            public override string GetUserId(Controller controller)
        {
            return controller.User.Identity.Name;
        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }
    }
}

