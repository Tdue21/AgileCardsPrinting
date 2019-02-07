// ****************************************************************************
// * The MIT License(MIT)
// * Copyright © 2019 Thomas Due
// * 
// * Permission is hereby granted, free of charge, to any person obtaining a 
// * copy of this software and associated documentation files (the “Software”), 
// * to deal in the Software without restriction, including without limitation 
// * the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// * and/or sell copies of the Software, and to permit persons to whom the  
// * Software is furnished to do so, subject to the following conditions:
// * 
// * The above copyright notice and this permission notice shall be included in  
// * all copies or substantial portions of the Software.
// * 
// * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS  
// * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL  
// * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING  
// * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// * IN THE SOFTWARE.
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using AgileCardsPrinting.Common;
using AgileCardsPrinting.Interfaces;
using AgileCardsPrinting.Services;
using AgileCardsPrinting.ViewModels;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using SimpleInjector;

namespace AgileCardsPrinting
{
	public class DependencyInjector
	{
		private readonly Container _container;

		public DependencyInjector()
		{
			_container = new Container();

			_container.RegisterType<IFileSystemService, FileSystemService>(Lifestyle.Singleton)
			          .RegisterType<ISettingsHandler, SettingsHandler>(Lifestyle.Singleton)
			          .RegisterType<IJiraService, JiraService>(Lifestyle.Singleton)
			          .RegisterPocoType<MainViewModel>()
			          .RegisterPocoType<PreviewViewModel>()
			          .RegisterPocoType<SettingsViewModel>()
				;
		}

		public MainViewModel MainViewModel => _container.GetInstance<MainViewModel>();

		public PreviewViewModel PreviewViewModel => _container.GetInstance<PreviewViewModel>();

		public SettingsViewModel SettingsViewModel => _container.GetInstance<SettingsViewModel>();
	}


	public abstract class LocatorBase
	{
		static Assembly entryAssembly;
		readonly Dictionary<string, Type> fullNameToTypeMapping = new Dictionary<string, Type>();
		readonly Dictionary<string, Type> registeredTypes = new Dictionary<string, Type>();
		readonly Dictionary<string, Type> shortNameToTypeMapping = new Dictionary<string, Type>();
		IEnumerator<Type> enumerator;

		protected static Assembly EntryAssembly
		{
			get => entryAssembly ?? (entryAssembly = Assembly.GetEntryAssembly());
			set => entryAssembly = value;
		}

		protected abstract IEnumerable<Assembly> Assemblies { get; }

		public void RegisterType(string name, Type type)
		{
			registeredTypes.Add(name, type);
		}

		protected Type ResolveType(string name, out IDictionary<string, string> properties)
		{
			ResolveTypeProperties(ref name, out properties);
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}

			if (registeredTypes.TryGetValue(name, out var type))
			{
				return type;
			}

			if (shortNameToTypeMapping.TryGetValue(name, out type) || fullNameToTypeMapping.TryGetValue(name, out type))
			{
				return type;
			}

			if (enumerator == null)
			{
				enumerator = GetTypes();
			}

			while (enumerator.MoveNext())
			{
				if (!fullNameToTypeMapping.ContainsKey(enumerator.Current.FullName))
				{
					shortNameToTypeMapping[enumerator.Current.Name] = enumerator.Current;
					fullNameToTypeMapping[enumerator.Current.FullName] = enumerator.Current;
				}

				if (enumerator.Current.Name == name || enumerator.Current.FullName == name)
				{
					return enumerator.Current;
				}
			}

			return null;
		}

		protected string ResolveTypeName(Type type, IDictionary<string, string> properties)
		{
			if (type == null)
			{
				return null;
			}

			var props = CreateTypeProperties(properties);
			return props + type.FullName;
		}

		protected virtual IEnumerator<Type> GetTypes()
		{
			foreach (var asm in Assemblies)
			{
				Type[] types;
				try
				{
					types = asm.GetTypes();
				}
				catch (ReflectionTypeLoadException e)
				{
					types = e.Types;
				}

				foreach (var type in types)
				{
					if (type != null)
					{
						yield return type;
					}
				}
			}
		}

		protected static void ResolveTypeProperties(ref string name, out IDictionary<string, string> properties)
		{
			string GetPropName(string x)
			{
				var ind = x.IndexOf('=');
				return ind == -1 ? null : x.Substring(0, ind);
			}

			string GetPropValue(string x)
			{
				var ind1 = x.IndexOf('=');
				var ind2 = x.IndexOf(';');
				if (ind1 == -1 || ind2 == -1)
					return null;
				return x.Substring(ind1 + 1, ind2 - ind1 - 1);
			}

			string RemoveProp(string x)
			{
				var ind = x.IndexOf(';');
				return x.Remove(0, ind + 1);
			}

			properties = new Dictionary<string, string>();
			if (string.IsNullOrEmpty(name))
			{
				return;
			}

			while (true)
			{
				var propName = GetPropName(name);
				var propValue = GetPropValue(name);
				if (propName == null || propValue == null)
					break;
				properties.Add(propName, propValue);
				name = RemoveProp(name);
			}
		}

		protected static string CreateTypeProperties(IDictionary<string, string> properties)
		{
			if (properties == null)
			{
				return null;
			}

			var res = string.Empty;
			foreach (var item in properties)
			{
				res += item.Key;
				res += "=";
				res += item.Value;
				res += ";";
			}

			return res;
		}

		protected virtual object CreateInstance(Type type, string typeName)
		{
			if (type == null)
			{
				return null;
			}

			object res = null;
			try
			{
				var parameterlessCtor = type.GetConstructor(
					BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance,
					null,
					new Type[] { },
					null);
				if (parameterlessCtor != null)
				{
					res = parameterlessCtor.Invoke(null);
				}

				if (res == null)
				{
					res = Activator.CreateInstance(type,
					                               BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding,
					                               null,
					                               null,
					                               null);
				}
			}
			catch (Exception e)
			{
				throw new LocatorException(GetType().Name, typeName, e);
			}

			if (res == null)
			{
				throw new LocatorException(GetType().Name, typeName, null);
			}

			return res;
		}
	}

	public class LocatorException : Exception
	{
		public LocatorException(string locatorName, string type, Exception innerException)
			: base($"{locatorName} cannot resolve the {type} type. See the inner exception for details.", innerException) { }
	}

	public interface IViewModelLocator
	{
		object ResolveViewModel(string name);
		Type ResolveViewModelType(string name);
		string GetViewModelTypeName(Type type);
	}

	public class ViewModelLocator : LocatorBase, IViewModelLocator
	{
		static readonly IViewModelLocator _defaultInstance = new ViewModelLocator(Application.Current);
		static IViewModelLocator _default;

		public ViewModelLocator(Application application)
			: this(EntryAssembly != null && !ViewModelBase.IsInDesignMode ? new[] {EntryAssembly} : new Assembly[0]) { }

		public ViewModelLocator(params Assembly[] assemblies)
			: this((IEnumerable<Assembly>) assemblies) { }

		public ViewModelLocator(IEnumerable<Assembly> assemblies)
		{
			this.Assemblies = assemblies;
		}

		public static IViewModelLocator Default
		{
			get => _default ?? _defaultInstance;
			set => _default = value;
		}

		protected override IEnumerable<Assembly> Assemblies { get; }

		public virtual Type ResolveViewModelType(string name)
		{
			var res = ResolveType(name, out var properties);
			if (res == null)
			{
				return null;
			}

			var isPoco = GetIsPOCOViewModelType(res, properties);
			return isPoco ? ViewModelSource.GetPOCOType(res) : res;
		}

		public virtual string GetViewModelTypeName(Type type)
		{
			var properties = new Dictionary<string, string>();
			if (type.GetInterfaces().Any(x => x == typeof(IPOCOViewModel)))
			{
				SetIsPOCOViewModelType(properties, true);
				type = type.BaseType;
			}

			return ResolveTypeName(type, properties);
		}

		object IViewModelLocator.ResolveViewModel(string name)
		{
			Type type = ((IViewModelLocator) this).ResolveViewModelType(name);
			if (type == null)
				return null;
			return CreateInstance(type, name);
		}

		protected bool GetIsPOCOViewModelType(Type type, IDictionary<string, string> properties)
		{
			if (type.GetCustomAttributes(typeof(POCOViewModelAttribute), true).Any())
			{
				return true;
			}

			if (properties.TryGetValue("IsPOCOViewModel", out var isPOCO))
			{
				return bool.Parse(isPOCO);
			}

			return false;
		}

		protected void SetIsPOCOViewModelType(IDictionary<string, string> properties, bool value)
		{
			properties.Add("IsPOCOViewModel", value.ToString());
		}
	}
}
