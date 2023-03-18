using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Witcher.Core.ExtensionMethods
{
	/// <summary>
	/// Перегруженный метод OrderBy
	/// </summary>
	public static class IOrderedQueryable
	{
		/// <summary>
		/// Перегруженный OrderBy
		/// </summary>
		/// <typeparam name="TSource">Тип параметра запроса</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="propertyName">Название поля</param>
		/// <param name="isAscending">Сортировать по возрастанию</param>
		/// <returns>новый запрос</returns>
		public static IQueryable<TSource> OrderBy<TSource>(
	   this IQueryable<TSource> query, string propertyName, bool isAscending = true)
		{
			if (query == null || propertyName == null)
				return query;

			var entityType = typeof(TSource);

			//Create x=>x.PropName
			var propertyInfo = entityType.GetProperty(propertyName);

			if (propertyInfo == null)
				throw new Exception("Не корректно заданo поле для сортировки");

			ParameterExpression arg = Expression.Parameter(entityType, "x");
			MemberExpression property = Expression.Property(arg, propertyName);
			var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

			//Get System.Linq.Queryable.OrderBy() method.
			var enumarableType = typeof(System.Linq.Queryable);
			var metodName = isAscending ? "OrderBy" : "OrderByDescending";
			var method = enumarableType.GetMethods()
				 .Where(m => m.Name == metodName && m.IsGenericMethodDefinition)
				 .Where(m =>
				 {
					 var parameters = m.GetParameters().ToList();
					 //Put more restriction here to ensure selecting the right overload                
					 return parameters.Count == 2;//overload that has 2 parameters
				 }).Single();

			//The linq's OrderBy<TSource, TKey> has two generic types, which provided here
			MethodInfo genericMethod = method
				 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

			/*Call query.OrderBy(selector), with query and selector: x=> x.PropName
			  Note that we pass the selector as Expression to the method and we don't compile it.
			  By doing so EF can extract "order by" columns and generate SQL for it.*/
			var newQuery = (IOrderedQueryable<TSource>)genericMethod
				 .Invoke(genericMethod, new object[] { query, selector });
			return newQuery;
		}
	}
}
