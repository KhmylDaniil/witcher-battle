using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;

namespace Wastelands.Service.Infrastructure.IntegrationTest.TestSupport
{
	/// <summary>
	/// Один контейнер Postgres на всю тестовую сборку (поднять контейнер и прогнать миграции один раз
	/// дорого — на каждый тест было бы слишком медленно). Изоляция между тестами — не транзакции, а то,
	/// что каждый тест создаёт собственных User/Game/Character с уникальными именами (тот же приём, что
	/// использовался в ручных curl-смоук-тестах этой сессии) — сценарии не пересекаются данными.
	/// </summary>
	[TestClass]
	public static class AssemblyFixture
	{
		private static PostgreSqlContainer? _container;
		private static NpgsqlDataSource? _dataSource;

		/// <summary>
		/// EnableDynamicJson() — как в продовой AddServiceDbContext (Wastelands.EfDataAccess/Extensions/
		/// ServiceCollectionExtensions.cs) — без него запись Dictionary-полей (Skills/DamageTypeModifiers/
		/// CriticalWounds — jsonb-колонки) в Npgsql падает с NotSupportedException.
		/// </summary>
		public static NpgsqlDataSource DataSource => _dataSource ?? throw new InvalidOperationException("AssemblyFixture not initialized.");

		[AssemblyInitialize]
		public static async Task InitializeAsync(TestContext context)
		{
			_container = new PostgreSqlBuilder("postgres:16").Build();
			await _container.StartAsync();
			_dataSource = new NpgsqlDataSourceBuilder(_container.GetConnectionString()).EnableDynamicJson().Build();

			var options = new DbContextOptionsBuilder<WastelandsDbContext>().UseNpgsql(_dataSource).Options;
			await using var dbContext = new WastelandsDbContext(options);
			await dbContext.Database.MigrateAsync();
		}

		[AssemblyCleanup]
		public static async Task CleanupAsync()
		{
			if (_dataSource is not null)
			{
				await _dataSource.DisposeAsync();
			}

			if (_container is not null)
			{
				await _container.DisposeAsync();
			}
		}
	}
}
