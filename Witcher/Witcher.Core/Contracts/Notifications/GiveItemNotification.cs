using MediatR;
using Witcher.Core.DAL.Entities;

namespace Witcher.Core.Contracts.Notifications
{
	public record GiveItemNotification(Item Item, Character Giver, Character Receiver) : INotification;
}
