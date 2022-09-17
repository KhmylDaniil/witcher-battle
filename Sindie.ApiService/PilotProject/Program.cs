
using PilotProject;
using PilotProject.DbContext;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Services.DateTimeProvider;
using Sindie.ApiService.Core.Services.Roll;

var filledContext = new FillDbContext();
var filledDb = filledContext.ReturnContext();
var authorizationService = filledContext.ReturnAuthorizationService();
IDateTimeProvider dateTimeProvider = new DateTimeProvider();
IRollService rollService = new RollService();

var app = new Application(filledDb, authorizationService, dateTimeProvider, rollService);
app.Run();
