using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xavier.Framework.Db.Models;

namespace Xavier.Framework.Db.Commons
{
	public static class ExtLinq
	{
		private class ParameterRebinder : ExpressionVisitor
		{
			private readonly Dictionary<ParameterExpression, ParameterExpression> map;

			private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
			{
				this.map = (map ?? new Dictionary<ParameterExpression, ParameterExpression>());
			}

			public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
			{
				return new ParameterRebinder(map).Visit(exp);
			}

			protected override Expression VisitParameter(ParameterExpression p)
			{
				if (map.TryGetValue(p, out ParameterExpression value))
				{
					p = value;
				}
				return base.VisitParameter(p);
			}
		}

		public static Expression Property(this Expression expression, string propertyName)
		{
			return Expression.Property(expression, propertyName);
		}

		public static Expression AndAlso(this Expression left, Expression right)
		{
			return Expression.AndAlso(left, right);
		}

		public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
		{
			return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
		}

		public static Expression GreaterThan(this Expression left, Expression right)
		{
			return Expression.GreaterThan(left, right);
		}

		public static Expression<T> ToLambda<T>(this Expression body, params ParameterExpression[] parameters)
		{
			return Expression.Lambda<T>(body, parameters);
		}

		public static Expression<Func<T, bool>> True<T>()
		{
			return (T param) => true;
		}

		public static Expression<Func<T, bool>> False<T>()
		{
			return (T param) => false;
		}

		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			return first.Compose(second, Expression.AndAlso);
		}

		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			return first.Compose(second, Expression.OrElse);
		}

		public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
		{
			Dictionary<ParameterExpression, ParameterExpression> map = first.Parameters.Select((ParameterExpression f, int i) => new
			{
				f = f,
				s = second.Parameters[i]
			}).ToDictionary(p => p.s, p => p.f);
			Expression arg = ParameterRebinder.ReplaceParameters(map, second.Body);
			return Expression.Lambda<T>(merge(first.Body, arg), first.Parameters);
		}

		public static IOrderedQueryable<TEntity> SortBy<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, dynamic>> sortPredicate) where TEntity : class, new()
		{
			return InvokeSortBy(query, sortPredicate, SortOrder.Ascending);
		}

		public static IOrderedQueryable<TEntity> SortByDescending<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, dynamic>> sortPredicate) where TEntity : class, new()
		{
			return InvokeSortBy(query, sortPredicate, SortOrder.Descending);
		}

		private static IOrderedQueryable<TEntity> InvokeSortBy<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder) where TEntity : class, new()
		{
			ParameterExpression parameterExpression = sortPredicate.Parameters[0];
			string text = null;
			Type type = null;
			Expression expression = null;
			if (sortPredicate.Body is UnaryExpression)
			{
				UnaryExpression unaryExpression = sortPredicate.Body as UnaryExpression;
				expression = unaryExpression.Operand;
			}
			else
			{
				if (!(sortPredicate.Body is MemberExpression))
				{
					throw new ArgumentException("The body of the sort predicate expression should be \r\n                either UnaryExpression or MemberExpression.", "sortPredicate");
				}
				expression = sortPredicate.Body;
			}
			MemberExpression memberExpression = (MemberExpression)expression;
			text = memberExpression.Member.Name;
			if (memberExpression.Member.MemberType == MemberTypes.Property)
			{
				PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
				if (propertyInfo != null)
				{
					type = propertyInfo.PropertyType;
				}
				Type delegateType = typeof(Func<, >).MakeGenericType(typeof(TEntity), type);
				LambdaExpression lambdaExpression = Expression.Lambda(delegateType, Expression.Convert(Expression.Property(parameterExpression, text), type), parameterExpression);
				MethodInfo[] methods = typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public);
				string sortingMethodName = GetSortingMethodName(sortOrder);
				MethodInfo methodInfo = methods.First((MethodInfo sm) => sm.Name == sortingMethodName && sm.GetParameters().Length == 2);
				return (IOrderedQueryable<TEntity>)methodInfo.MakeGenericMethod(typeof(TEntity), type).Invoke(null, new object[2]
				{
					query,
					lambdaExpression
				});
			}
			throw new InvalidOperationException("Cannot evaluate the type of property since the member expression \r\n                represented by the sort predicate expression does not contain a PropertyInfo object.");
		}

		private static string GetSortingMethodName(SortOrder sortOrder)
		{
			switch (sortOrder)
			{
			case SortOrder.Ascending:
				return "OrderBy";
			case SortOrder.Descending:
				return "OrderByDescending";
			default:
				throw new ArgumentException("Sort Order must be specified as either Ascending or Descending.", "sortOrder");
			}
		}
	}
}
