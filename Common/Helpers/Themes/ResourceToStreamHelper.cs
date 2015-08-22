using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Globe.QcApp.Common.Helpers.Themes
{
	public class ResourceToStreamHelper
	{
		public static bool ResourceExists(Assembly assembly, string resourcePath)
		{
			var resources = GetResourcePaths(assembly);
            var resourceExists = GetResourcePaths(assembly).Contains(resourcePath.ToLowerInvariant());
			return resourceExists;
		}

		public static IEnumerable<object> GetResourcePaths(Assembly assembly)
		{
			var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var resourceName = new System.Reflection.AssemblyName(assembly.FullName).Name + ".g";
			var resourceManager = new ResourceManager(resourceName, assembly);

			try
			{
				var resourceSet = resourceManager.GetResourceSet(culture, true, true);

				foreach (System.Collections.DictionaryEntry resource in resourceSet)
				{
					yield return resource.Key;
				}
			}
			finally
			{
				resourceManager.ReleaseAllResources();
			}
		}


		public static string ExtractSourceCodeFromSource(Uri uriSource)
		{
			Stream resourceFileStream = null;
            //if (uriSource.OriginalString.IsXaml())
            //{
            //    GetManifestResourceStream(uriSource, ref resourceFileStream);
            //}
            //else
            //{
            //    TryGetResource(uriSource, ref resourceFileStream);
            //}

            TryGetResource(uriSource, ref resourceFileStream);

			if (resourceFileStream != null)
			{
				using (StreamReader streamReader = new StreamReader(resourceFileStream))
				{
					return streamReader.ReadToEnd();
				}
			}

			return string.Empty;
		}

		public static bool TryGetResource(Uri uriSource, ref Stream resourceFileStream)
		{
			try
			{
				resourceFileStream = System.Windows.Application.GetResourceStream(uriSource).Stream;
				return true;
			}
			catch (IOException)
			{
				Debug.WriteLine("Please check the resource file: {0}", uriSource.OriginalString);
				return false;
			}
		}

//        private static void GetManifestResourceStream(Uri uriSource, ref Stream resourceFileStream)
//        {
//#if WPF
//            string executingAssembly = uriSource.OriginalString.Split(';')[0];
            
//            var targetAssembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => executingAssembly.Contains(a.GetName().Name)).FirstOrDefault();
//            var exampleString = uriSource.OriginalString.Split('/')[2];
//            var targetSource = string.Format("Telerik.Windows.Examples.{0}.{1}", targetAssembly.GetName().Name, exampleString.Replace('\\', '.'));
//#else
//            var targetAssembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => uriSource.OriginalString.Contains(a.FullName.Split(',')[0])).FirstOrDefault();
//            var exampleString = uriSource.OriginalString.Split('/')[2];
//            var targetSource = string.Format("Telerik.Windows.Examples.{0}.{1}", targetAssembly.FullName.Split(',')[0], exampleString.Replace('\\', '.'));
//#endif
//            resourceFileStream = targetAssembly.GetManifestResourceStream(targetSource);
//        }
	}
}
