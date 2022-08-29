
using PilotProject;
using PilotProject.Controllers;
using PilotProject.DbContext;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests;
using Sindie.ApiService.Core.Services.DateTimeProvider;
using Sindie.ApiService.Core.Services.Roll;
using Sindie.ApiService.Storage.Postgresql;

var filledContext = new FillDbContext();
var filledDb = filledContext.ReturnContext();
var authorizationService = filledContext.ReturnAuthorizationService();
IDateTimeProvider dateTimeProvider = new DateTimeProvider();
IRollService rollService = new RollService();

var app = new Application(filledDb, authorizationService, dateTimeProvider, rollService);
app.Run();
