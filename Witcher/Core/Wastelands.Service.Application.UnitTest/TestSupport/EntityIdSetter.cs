using System.Reflection;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Service.Application.UnitTest.TestSupport
{
	/// <summary>
	/// Entity.Id только для EF — вне персистентности всегда 0, что делает разные экземпляры
	/// неразличимыми для кода, сопоставляющего по Id (например, BattleAttack.TargetedCreaturePartId).
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
