using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Realtorist.DataAccess.Abstractions;
using Realtorist.Models.Settings;
using Realtorist.Services.Abstractions.Providers;
using Realtorist.Web.Install.Application.Models;

namespace Realtorist.Web.Install.Application.Controllers
{
    [Route("api/install")]
    public class InstallApiController : Controller
    {
        private readonly ISettingsDataAccess _settingsDataAccess;
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly ILogger _logger;

        public InstallApiController(ISettingsDataAccess settingsDataAccess, IEncryptionProvider encryptionProvider, ILogger<InstallApiController> logger)
        {
            _settingsDataAccess = settingsDataAccess;
            _logger = logger;
            _encryptionProvider = encryptionProvider;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Install([FromBody] InstallModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("Starting installation...");

            _logger.LogInformation("Writing website settings...");
            var websiteSettings = new WebsiteSettings 
            {
                WebsiteAddress = model.WebsiteAddress,
                WebsiteName = model.WebsiteName,
                WebsiteTitle = model.WebsiteTitle,
                Timezone = model.Timezone
            };

            await _settingsDataAccess.UpdateSettingsAsync(SettingTypes.Website, websiteSettings);

            _logger.LogInformation("Writing profile settings...");
            var profileSettings = new ProfileSettings
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.EmailAddress,
                AdminTheme = "default",
                Phone = string.Empty,
                ShortDescription = string.Empty,
                Address = string.Empty,
                Avatar = string.Empty
            };

            await _settingsDataAccess.UpdateSettingsAsync(SettingTypes.Profile, profileSettings);

            _logger.LogInformation("Writing password settings");
            var passwordSettings = new PasswordSettings
            {
                Guid = System.Guid.NewGuid(),
                Password = _encryptionProvider.EncryptOneWay(model.Password)
            };

            await _settingsDataAccess.UpdateSettingsAsync(SettingTypes.Password, passwordSettings);

            _logger.LogInformation("Success! Installation has completed successfully.");

            return Ok();
        }
    }
}