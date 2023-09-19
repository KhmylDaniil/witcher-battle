using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.NotificationRequests;
using Witcher.Core.Entities;
using Witcher.Core.Notifications;
using Witcher.Core.Requests.NotificationRequests;

namespace Witcher.UnitTest.Core.Requests.NotificationRequests
{
	[TestClass]
	public sealed class GetNotificationByIdHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly User _user;
		private readonly Notification _notReadedNotification;
		private readonly Notification _readedNotification;

		public GetNotificationByIdHandlerTest() : base()
		{
			_user = User.CreateForTest(id: UserId);
			_readedNotification = new Notification() { User = _user, Message = "readed", IsReaded = true };
			_notReadedNotification = new Notification() { User = _user, Message = "unReaded", IsReaded = false };

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_user, _readedNotification, _notReadedNotification));
		}

		[TestMethod]
		public async Task Handle_GetNotificationById()
		{
			var request = new GetNotificationByIdQuery()
			{
				Id = _readedNotification.Id,
			};

			var newHandler = new GetNotificationByIdHandler(
				_dbContext, AuthorizationService.Object, UserContext.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			Assert.IsTrue(result.IsReaded);
			Assert.AreEqual("readed", result.Message);
		}
	}
}
