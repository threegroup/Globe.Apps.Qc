using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace Globe.QcApp.Common.Helpers.Themes
{
    public class ApplicationThemeManager
    {
        public const string DefaultThemeName = "Windows8";
        public const string DefaultThemeNameTouch = "Windows8Touch";

        private const string Office2013DefaultThemeName = "Office2013";
        private const string Office2013LightGrayThemeName = "Office2013_LightGray";
        private const string Office2013DarkGrayThemeName = "Office2013_DarkGray";

        private bool isOffice2013ColorPresetLoaded;
        private string currentOffice2013ThemeName;

        public event EventHandler ThemeChanged;

        private static readonly ApplicationThemeManager instance = new ApplicationThemeManager();
        
        // contains items like <"/Telerik.Windows.Themes.Windows8;component/telerik.windows.controls.xaml", ResourceDictionary>
        private static readonly Dictionary<string, ResourceDictionary> cachedResourceDictionaries = new Dictionary<string, ResourceDictionary>();
        
        private static readonly string[] allThemes = new string[]
        {
            "Office_Black",
            "Office_Blue",
            "Office_Silver",
            "Summer",
            "Vista",
            "Windows8",
            "Windows8Touch",
            "Transparent",
            "Windows7",
            "Expression_Dark",
            ApplicationThemeManager.Office2013DefaultThemeName,
            ApplicationThemeManager.Office2013LightGrayThemeName,
            ApplicationThemeManager.Office2013DarkGrayThemeName
        };

        private static readonly string[] defaultReferencesNamesForApplication = new string[]
        {
            "Telerik.Windows.Controls",
            //"Telerik.Windows.Controls.Input",
            //"Telerik.Windows.Controls.Navigation",
            //"Telerik.Windows.Documents",
            "System.Windows"
        };

        private string currentTheme;

        public string CurrentTheme
        {
            get
            {
                return this.currentTheme;
            }
        }

        private ApplicationThemeManager()
        {
        }

        public static ApplicationThemeManager GetInstance()
        {
            return instance;
        }

        public static string[] GetAllThemes()
        {
            return allThemes;
        }

        /// <summary>
        /// Adds ResourceDictionary items to application resources for certain theme using assembly names to form the ResourceDictionary's Source
        /// </summary>
        /// <param name="themeName">The theme for which resources to add</param>
        /// <param name="resourcesPaths">String array of assembly names used to form the Source of the ResourceDictionary items</param>
        public void EnsureResourcesForTheme(string themeName, string[] resourcesPaths = null)
        {
            if (resourcesPaths == null)
            {
                resourcesPaths = new string[] { };
            }

            // always include default resources
            resourcesPaths = resourcesPaths.Union(defaultReferencesNamesForApplication).ToArray();

            // temporarily save QSF's resources
            var telerikThemeDictionaries = from keyValuePair in cachedResourceDictionaries
                                           where keyValuePair.Key.Contains("Telerik.Windows.Themes.")
                                           select keyValuePair.Value;
            var qsfOnlyDictionaries = Application.Current.Resources.MergedDictionaries.Except(telerikThemeDictionaries).ToList();

            #if WPF
            Action resetApplicationResources = () =>
            {
                #endif
                Application.Current.Resources.MergedDictionaries.Clear();

                // add new resources
                foreach (string resourcePath in resourcesPaths)
                {
                    var name = resourcePath.Split(',')[0].ToLower(CultureInfo.InvariantCulture);

                    var xamlFile = string.Format("{0}.xaml", name);
                    var bamlFile = string.Format("{0}.baml", name);

                    if (themeName.Contains(ApplicationThemeManager.Office2013DefaultThemeName))
                    {
                        themeName = this.GetProperOffice2013ThemeName(themeName);
                    }

                    var uriStringToAdd = "/Telerik.Windows.Themes." + themeName + ";component/themes/" + xamlFile;

                    var assembly = System.AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName != null &&
                                                                                             a.FullName.Contains(themeName)).FirstOrDefault();

                    if (assembly == null)
                    {
                        assembly = System.Reflection.Assembly.Load(string.Format("Telerik.Windows.Themes.{0}", themeName));
                    }

                    if (assembly != null)
                    {
                        var paths = ResourceToStreamHelper.GetResourcePaths(assembly);
                        if (!paths.Contains(string.Format("themes/{0}", xamlFile)) && !paths.Contains(string.Format("themes/{0}", bamlFile)))
                        {
                            continue;
                        }
                    }

                    // this call may cause "Collection was modified" exception (WPF)
                    AddDictionaryToApplicationResources(uriStringToAdd);
                }

                //add back QSF's resources
                if (qsfOnlyDictionaries != null && qsfOnlyDictionaries.Count() > 0)
                {
                    foreach (var resourceDictionary in qsfOnlyDictionaries)
                    {
                        Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                    }
                }
                #if WPF
            };

            // retry to reset resources several times (hides the exception with "Collection was modified")
            int retries = 0;

            while (retries < 5)
            {
                try
                {
                    resetApplicationResources();
                    break;
                }
                catch
                {
                    retries++;
                }
            }
            #endif

				if(themeName.Contains(ApplicationThemeManager.Office2013DefaultThemeName))
				{
					this.currentTheme = this.currentOffice2013ThemeName;
				}
				else
				{
					this.currentTheme = themeName;
				}

				this.isOffice2013ColorPresetLoaded = false;

            this.OnThemeChanged();
        }
  
        private string GetProperOffice2013ThemeName(string themeName)
        {
            if (!this.isOffice2013ColorPresetLoaded)
            {
                this.currentOffice2013ThemeName = themeName;

                switch (themeName)
                {
                    case (ApplicationThemeManager.Office2013LightGrayThemeName):
                        {
                            Office2013Palette.LoadPreset(Office2013Palette.ColorVariation.LightGray);
                            themeName = ApplicationThemeManager.Office2013DefaultThemeName;
                        }
                        break;
                    case (ApplicationThemeManager.Office2013DarkGrayThemeName):
                        {
                            Office2013Palette.LoadPreset(Office2013Palette.ColorVariation.DarkGray);
                            themeName = ApplicationThemeManager.Office2013DefaultThemeName;
                        }
                        break;
                    default:
                        {
                            Office2013Palette.LoadPreset(Office2013Palette.ColorVariation.White);
                        }
                        break;
                }
                this.isOffice2013ColorPresetLoaded = true;
            }

            return themeName;
        }

        public static void EnsureFrameworkElementResourcesForTheme(FrameworkElement element, string themeName)
        {
            var resourcesPaths = new string[] { };

            // always include default resources
            resourcesPaths = defaultReferencesNamesForApplication.ToArray();

            // temporarily save QSF's resources
            var telerikThemeDictionaries = from keyValuePair in cachedResourceDictionaries
                                           where keyValuePair.Key.Contains("Telerik.Windows.Themes.")
                                           select keyValuePair.Value;
            var qsfOnlyDictionaries = Application.Current.Resources.MergedDictionaries.Except(telerikThemeDictionaries).ToList();

            element.Resources.MergedDictionaries.Clear();

            // add new resources
            foreach (string resourcePath in resourcesPaths)
            {
                var xamlFile = resourcePath.Split(',')[0].ToLower(CultureInfo.InvariantCulture) + ".xaml";
                var uriStringToAdd = "/Telerik.Windows.Themes." + themeName + ";component/themes/" + xamlFile;

                var uri = new Uri(uriStringToAdd, UriKind.RelativeOrAbsolute);
                var resourceDictionary = new ResourceDictionary() { Source = uri };
                element.Resources.MergedDictionaries.Add(resourceDictionary);
            }
        }
        
        public static string GetDefaultThemeName(bool isInTouchMode)
        {
            return isInTouchMode ? DefaultThemeNameTouch : DefaultThemeName;
        }

        private static void AddDictionaryToApplicationResources(string uriStringToAdd)
        {
            ResourceDictionary resourceDictionary = null;

            // load ResourceDictionary from cache, if exists there, otherwise try to create it and add it to cache
            if (cachedResourceDictionaries.ContainsKey(uriStringToAdd))
            {
                resourceDictionary = cachedResourceDictionaries[uriStringToAdd];
            }
            else
            {
                try
                {
                    var uri = new Uri(uriStringToAdd, UriKind.RelativeOrAbsolute);
                          
                    resourceDictionary = new ResourceDictionary() { Source = uri };
                    cachedResourceDictionaries.Add(uriStringToAdd, resourceDictionary);
                }
                catch
                {
                    resourceDictionary = null;
                    cachedResourceDictionaries.Add(uriStringToAdd, resourceDictionary);
                }
            }

            if (resourceDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
        }
        
        private void OnThemeChanged()
        {
            if (ThemeChanged != null)
            {
                this.ThemeChanged(this, EventArgs.Empty);
            }
        }
	}
}