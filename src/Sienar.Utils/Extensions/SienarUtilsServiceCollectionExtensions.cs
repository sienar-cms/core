using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Sienar.Email;
using Sienar.Hooks;
using Sienar.Infrastructure;
using Sienar.Processors;
using Sienar.Services;

namespace Sienar.Extensions;

/// <summary>
/// Contains <see cref="IServiceCollection"/> extension methods for the <c>Sienar.Utils</c> assembly
/// </summary>
public static class SienarUtilsServiceCollectionExtensions
{
	/// <summary>
	/// Adds universal Sienar utilities to the DI container
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <returns>the service collection</returns>
	[ExcludeFromCodeCoverage]
	public static IServiceCollection AddSienarCoreUtilities(this IServiceCollection self)
	{
		self.TryAddScoped(typeof(IEntityReader<>), typeof(EntityReader<>));
		self.TryAddScoped(typeof(IEntityWriter<>), typeof(EntityWriter<>));
		self.TryAddScoped(typeof(IEntityDeleter<>), typeof(EntityDeleter<>));
		self.TryAddScoped(typeof(IStatusService<>), typeof(StatusService<>));
		self.TryAddScoped(typeof(IService<,>), typeof(Service<,>));
		self.TryAddScoped(typeof(IResultService<>), typeof(ResultService<>));
		self.TryAddScoped(typeof(IAccessValidatorService<>), typeof(AccessValidatorService<>));
		self.TryAddScoped(typeof(IStateValidatorService<>), typeof(StateValidatorService<>));
		self.TryAddScoped(typeof(IBeforeProcessService<>), typeof(BeforeProcessService<>));
		self.TryAddScoped(typeof(IAfterProcessService<>), typeof(AfterProcessService<>));
		self.TryAddScoped<IBotDetector, BotDetector>();
		self.TryAddSingleton<IDashboardProvider, DashboardProvider>();
		self.TryAddScoped<IDashboardGenerator, DashboardGenerator>();
		self.TryAddSingleton<IMenuProvider, MenuProvider>();
		self.TryAddScoped<IMenuGenerator, MenuGenerator>();
		self.TryAddSingleton<IScriptProvider, ScriptProvider>();
		self.TryAddSingleton<IStyleProvider, StyleProvider>();
		self.TryAddScoped<IEmailSender, DefaultEmailSender>();

		return self;
	}

	/// <summary>
	/// Checks if a <c>TOptions</c> has already been configured, and if not, adds the supplied default configuration
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <param name="config">the default configuration to apply if no existing configuration was found</param>
	/// <typeparam name="TOptions">the type of the options class to configure</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection ApplyDefaultConfiguration<TOptions>(
		this IServiceCollection self,
		IConfiguration config)
		where TOptions : class
	{
		if (!self.Any(sd => sd.ServiceType == typeof(IConfigureOptions<TOptions>)))
		{
			self.Configure<TOptions>(config);
		}

		return self;
	}

	/// <summary>
	/// Adds a configurer of type <c>IConfigurer&lt;TOptions&gt;</c> for the given <c>TOptions</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TConfigurer">the type of the configurer</typeparam>
	/// <typeparam name="TOptions">the type of the options class to configure</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddConfigurer<TConfigurer, TOptions>(this IServiceCollection self)
		where TConfigurer : class, IConfigurer<TOptions>
		where TOptions : class
		=> self.AddScoped<IConfigurer<TOptions>, TConfigurer>();

	/// <summary>
	/// Adds a configurer of type <c>IConfigurer&lt;TOptions&gt;</c> for the given <c>TOptions</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TConfigurer">the type of the configurer</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddConfigurer<TConfigurer>(this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(TConfigurer),
			typeof(IConfigurer<>),
			ServiceLifetime.Scoped,
			false);

	/// <summary>
	/// Adds a configurer of type <c>IConfigurer&lt;TOptions&gt;</c> for the given <c>TOptions</c> if one hasn't already been registered
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TConfigurer">the type of the configurer</typeparam>
	/// <typeparam name="TOptions">the type of the options class to configure</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection TryAddConfigurer<TConfigurer, TOptions>(this IServiceCollection self)
		where TConfigurer : class, IConfigurer<TOptions>
		where TOptions : class
	{
		self.TryAddScoped<IConfigurer<TOptions>, TConfigurer>();
		return self;
	}

	/// <summary>
	/// Adds a configurer of type <c>IConfigurer&lt;TOptions&gt;</c> for the given <c>TOptions</c> if one hasn't already been registered
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TConfigurer">the type of the configurer</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection TryAddConfigurer<TConfigurer>(this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(TConfigurer),
			typeof(IConfigurer<>),
			ServiceLifetime.Scoped,
			true);

	/// <summary>
	/// Adds an access validator for the given <c>TRequest</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TRequest">the data type of the request</typeparam>
	/// <typeparam name="TValidator">the validator implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddAccessValidator<TRequest, TValidator>(
		this IServiceCollection self)
		where TValidator : class, IAccessValidator<TRequest>
		=> self.AddScoped<IAccessValidator<TRequest>, TValidator>();

	/// <summary>
	/// Adds an access validator for the given <c>TRequest</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TValidator">the validator implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddAccessValidator<TValidator>(
		this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(TValidator),
			typeof(IAccessValidator<>),
			ServiceLifetime.Scoped,
			false);

	/// <summary>
	/// Adds a state validator for the given <c>TRequest</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TRequest">the data type of the request</typeparam>
	/// <typeparam name="TValidator">the validator implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddStateValidator<TRequest, TValidator>(
		this IServiceCollection self)
		where TValidator : class, IStateValidator<TRequest>
		=> self.AddScoped<IStateValidator<TRequest>, TValidator>();

	/// <summary>
	/// Adds a state validator for the given <c>TRequest</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TValidator">the validator implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddStateValidator<TValidator>(
		this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(TValidator),
			typeof(IStateValidator<>),
			ServiceLifetime.Scoped,
			false);

	/// <summary>
	/// Adds a before-process hook for the given <c>TRequest</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TRequest">the data type of the request</typeparam>
	/// <typeparam name="THook">the hook implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddBeforeHook<TRequest, THook>(
		this IServiceCollection self)
		where THook : class, IBeforeProcess<TRequest>
		=> self.AddScoped<IBeforeProcess<TRequest>, THook>();

	/// <summary>
	/// Adds a before-process hook for the given <c>TRequest</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="THook">the hook implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddBeforeHook<THook>(
		this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(THook),
			typeof(IBeforeProcess<>),
			ServiceLifetime.Scoped,
			false);

	/// <summary>
	/// Adds an after-process hook for the given <c>TRequest</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TRequest">the data type of the request</typeparam>
	/// <typeparam name="THook">the hook implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddAfterHook<TRequest, THook>(
		this IServiceCollection self)
		where THook : class, IAfterProcess<TRequest>
		=> self.AddScoped<IAfterProcess<TRequest>, THook>();

	/// <summary>
	/// Adds an after-process hook for the given <c>TRequest</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="THook">the hook implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddAfterHook<THook>(
		this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(THook),
			typeof(IAfterProcess<>),
			ServiceLifetime.Scoped,
			false);

	/// <summary>
	/// Adds a processor
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TRequest">the data type of the request</typeparam>
	/// <typeparam name="TResult">the data type of the result</typeparam>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddProcessor<TRequest, TResult, TProcessor>(
		this IServiceCollection self)
		where TProcessor : class, IProcessor<TRequest, TResult>
		=> self.AddScoped<IProcessor<TRequest, TResult>, TProcessor>();

	/// <summary>
	/// Adds a processor
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddProcessor<TProcessor>(
		this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(TProcessor),
			typeof(IProcessor<,>),
			ServiceLifetime.Scoped,
			false);

	/// <summary>
	/// Adds a processor
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TRequest">the data type of the request</typeparam>
	/// <typeparam name="TResult">the data type of the result</typeparam>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection TryAddProcessor<TRequest, TResult, TProcessor>(this IServiceCollection self)
		where TProcessor : class, IProcessor<TRequest, TResult>
	{
		self.TryAddScoped<IProcessor<TRequest, TResult>, TProcessor>();
		return self;
	}

	/// <summary>
	/// Adds a processor
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection TryAddProcessor<TProcessor>(
		this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(TProcessor),
			typeof(IProcessor<,>),
			ServiceLifetime.Scoped,
			true);

	/// <summary>
	/// Adds a status processor (<c>IProcessor&lt;TRequest, bool&gt;</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TRequest">the data type of the request</typeparam>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddStatusProcessor<TRequest, TProcessor>(
		this IServiceCollection self)
		where TProcessor : class, IProcessor<TRequest, bool>
		=> AddProcessor<TRequest, bool, TProcessor>(self);

	/// <summary>
	/// Adds a status processor (<c>IProcessor&lt;TRequest, bool&gt;</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddStatusProcessor<TProcessor>(
		this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(TProcessor),
			typeof(IProcessor<,>),
			ServiceLifetime.Scoped,
			false);

	/// <summary>
	/// Adds a status processor (<c>IProcessor&lt;TRequest, bool&gt;</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TRequest">the data type of the request</typeparam>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection TryAddStatusProcessor<TRequest, TProcessor>(
		this IServiceCollection self)
		where TProcessor : class, IProcessor<TRequest, bool>
		=> TryAddProcessor<TRequest, bool, TProcessor>(self);

	/// <summary>
	/// Adds a status processor (<c>IProcessor&lt;TRequest, bool&gt;</c>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection TryAddStatusProcessor<TProcessor>(
		this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(TProcessor),
			typeof(IProcessor<,>),
			ServiceLifetime.Scoped,
			true);

	/// <summary>
	/// Adds a result processor (<c>IProcessor&lt;TRequest&gt;</c>)
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TResult">the data type of the result</typeparam>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddResultProcessor<TResult, TProcessor>(
		this IServiceCollection self)
		where TProcessor : class, IProcessor<TResult>
		=> self.AddScoped<IProcessor<TResult>, TProcessor>();

	/// <summary>
	/// Adds a result processor (<c>IProcessor&lt;TRequest&gt;</c>)
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddResultProcessor<TProcessor>(
		this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(TProcessor),
			typeof(IProcessor<>),
			ServiceLifetime.Scoped,
			false);

	/// <summary>
	/// Adds a result processor (<c>IProcessor&lt;TRequest&gt;</c>)
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TResult">the data type of the result</typeparam>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection TryAddResultProcessor<TResult, TProcessor>(
		this IServiceCollection self)
		where TProcessor : class, IProcessor<TResult>
	{
		self.TryAddScoped<IProcessor<TResult>, TProcessor>();
		return self;
	}

	/// <summary>
	/// Adds a result processor (<c>IProcessor&lt;TRequest&gt;</c>)
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TProcessor">the processor implementation</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection TryAddResultProcessor<TProcessor>(
		this IServiceCollection self)
		=> self.AddImplementationAsInterface(
			typeof(TProcessor),
			typeof(IProcessor<>),
			ServiceLifetime.Scoped,
			true);

	private static IServiceCollection AddImplementationAsInterface(
		this IServiceCollection self,
		Type implementationType,
		Type interfaceType,
		ServiceLifetime lifetime,
		bool tryAdd)
	{
		var concreteInterfaceTypes = implementationType
			.GetInterfaces()
			.Where(
				i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);
		var implementsSpecifiedInterface = false;

		foreach (var t in concreteInterfaceTypes)
		{
			implementsSpecifiedInterface = true;
			var descriptor = new ServiceDescriptor(t, implementationType, lifetime);
			if (tryAdd)
			{
				self.TryAdd(descriptor);
			}
			else
			{
				self.Add(descriptor);
			}
		}

		if (!implementsSpecifiedInterface)
		{
			throw new InvalidOperationException($"Type {implementationType} does not inherit from {interfaceType}.");
		}

		return self;
	}
}