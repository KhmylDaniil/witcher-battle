using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Service.Domain.Entities
{
	public class Character : Entity
	{
		public long UserId { get; private set; }

		public string Name { get; private set; }

		public int Int {  get; private set; }

		public int Str { get; private set; }

		public int Rea { get; private set; }

		public int Dex { get; private set; }

		public int Cra { get; private set; }

		public int Emp { get; private set; }

		public int Wil { get; private set; }

		private Character()
		{
		}

		public Character(long userId, string name, int @int, int str, int rea, int dex, int cra, int emp, int wil)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(userId, nameof(userId));
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(@int, nameof(@int));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(str, nameof(str));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(rea, nameof(rea));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(dex, nameof(dex));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(cra, nameof(cra));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(emp, nameof(emp));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(wil, nameof(wil));

			UserId = userId;
			Name = name;
			Int = @int;
			Str = str;
			Rea = rea;
			Dex = dex;
			Cra = cra;
			Emp = emp;
			Wil = wil;
		}

		public void UpdateCharacter(string name, int @int, int str, int rea, int dex, int cra, int emp, int wil)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(@int, nameof(@int));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(str, nameof(str));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(rea, nameof(rea));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(dex, nameof(dex));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(cra, nameof(cra));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(emp, nameof(emp));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(wil, nameof(wil));

			Name = name;
			Int = @int;
			Str = str;
			Rea = rea;
			Dex = dex;
			Cra = cra;
			Emp = emp;
			Wil = wil;
		}
	}
}
