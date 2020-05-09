using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using SynchronizerEX.Contracts;
using SynchronizerEX.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizerEX.Services
{
    public class GoogleDriveService : IGoogleDriveService
    {
        private string[] _scopes = { DriveService.Scope.Drive };
        private string _applivationName = "SynchronizerEX";

        public UserCredential GetUserCredential()
        {
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string creadPath = Path.Combine(StaticHelper.WorkingDirectory, "drive-credentials.json");

                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "User",
                    CancellationToken.None,
                    new FileDataStore(creadPath, true)).Result;
            }
        }


    }
}
