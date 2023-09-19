using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.NotificationRequests;
using Witcher.Core.Entities;
using Witcher.Core.Notifications;
using Witcher.Core.Requests.NotificationRequests;

namespace Witcher.UnitTest.Core.Requests.NotificationRequests
{
	[TestClass]
	public sealed class GetNotificationsHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly User _user;
		private readonly Notification _notReadedNotification;
		private readonly Notification _readedNotification;

		public GetNotificationsHandlerTest() : base()
		{
			_user = User.CreateForTest(id: UserId);
			_readedNotification = new Notification() { User = _user, Message = "readed", IsReaded = true };
			_notReadedNotification = new Notification() { User = _user, Message = "unReaded", IsReaded = false };

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_user, _readedNotification, _notReadedNotification));
		}

		[TestMethod]
		public async Task Handle_GetAllNotifications()
		{
			var request = new GetNotificationsQuery();

			var newHandler = new GetNotificationsHandler(
				_dbContext, AuthorizationService.Object, UserContext.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			Assert.AreEqual(2, result.Count());
			Assert.IsNotNull(result.FirstOrDefault(x => x.Id == _readedNotification.Id));
			Assert.IsNotNull(result.FirstOrDefault(x => x.Id == _notReadedNotification.Id));
		}

		[TestMethod]
		public async Task Handle_GetNotReadedNotifications()
		{
			var request = new GetNotificationsQuery() { OnlyNotReaded = true };

			var newHandler = new GetNotificationsHandler(
				_dbContext, AuthorizationService.Object, UserContext.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			Assert.AreEqual(1, result.Count());
			Assert.IsNotNull(result.FirstOrDefault(x => x.Id == _notReadedNotification.Id));
		}
	}
}
