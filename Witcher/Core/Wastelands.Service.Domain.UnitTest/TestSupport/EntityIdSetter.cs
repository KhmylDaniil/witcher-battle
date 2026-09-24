using System.Reflection;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Service.Domain.UnitTest.TestSupport
{
	/// <summary>
	/// Entity.Id только для EF — вне персистентности всегда 0, что делает разные экземпляры
	/// неразличимыми для кода, сопоставляющего участников по Id (например, Battle.RemoveParticipant).
	/// Тестам, где это важно (несколько персонажей/существ в одном бою), нужны разные Id без БД.
	/// </summary>
	internal static class EntityIdSetter
	{
		private static readonly PropertyInfo IdProperty = typeof(Entity).GetProperty(nameof(Entity.Id))!;

		public static T WithId<T>(this T entity, long id)
			where T : Entity
		{
			IdProperty.SetValue(entity, id);
			return entity;
		}
	}
}
