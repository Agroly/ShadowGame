using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using VContainer.Unity;

namespace _project.Scripts.Localization
{
    public class LocalizationService : IStartable
    {
        private const string LanguagePrefsKey = "selected-locale";
        
        private List<Locale> _availableLocales;
        
        public void Start()
        {
           _availableLocales = LocalizationSettings.AvailableLocales.Locales;
        }
        public void ToggleLanguage()
        {
            if (_availableLocales == null || _availableLocales.Count <= 1) return;

            int currentLocaleIndex = _availableLocales.IndexOf(LocalizationSettings.SelectedLocale);
            int nextLocaleIndex = (currentLocaleIndex + 1) % _availableLocales.Count;
            
            ApplyAndSave(_availableLocales[nextLocaleIndex]);
        }
        
        private void ApplyAndSave(Locale locale)
        {
            LocalizationSettings.SelectedLocale = locale;
            PlayerPrefs.SetString(LanguagePrefsKey, locale.Identifier.Code);
            PlayerPrefs.Save();
        }
    }
}