using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.NotificationRequests;
using Witcher.Core.Entities;
using Witcher.Core.Notifications;
using Witcher.Core.Requests.NotificationRequests;

namespace Witcher.UnitTest.Core.Requests.NotificationRequests
{
	[TestClass]
	public class DeleteNotificationHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly User _user;
		private readonly Notification _notReadedNotification;
		private readonly Notification _readedNotification;

		public DeleteNotificationHandlerTest() : base()
		{
			_user = User.CreateForTest(id: UserId);
			_readedNotification = new Notification() { User = _user, Message = "readed", IsReaded = true };
			_notReadedNotification = new Notification() { User = _user, Message = "unReaded", IsReaded = false };

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_user, _readedNotification, _notReadedNotification));
		}

		[TestMethod]
		public async Task Handle_DeleteNotification_ShouldReturnUnit()
		{
			var request = new DeleteNotificationCommand()
			{
				Id = _readedNotification.Id,
			};

			var newHandler = new DeleteNotificationHandler(
				_dbContext, AuthorizationService.Object, UserContext.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var notifications = _dbContext.Users.FirstOrDefault().Notifications;

			Assert.IsNotNull(notifications);
			Assert.AreEqual(1, notifications.Count);
			Assert.IsFalse(notifications[0].IsReaded);
			Assert.AreEqual("unReaded", notifications[0].Message);


		}
	}
}
