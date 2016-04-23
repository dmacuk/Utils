using System;
using System.Collections.Generic;
using Utils.File;

namespace Utils.Preference
{
    public class PreferenceException : Exception
    {
        public PreferenceException(string message) : base(message)
        {
        }
    }

    public sealed class PreferenceManager
    {
        private static string _fileName;
        private static PreferenceManager _instance;
        private static JsonUtils<Dictionary<string, string>> _jsonUtils;
        private static Dictionary<string, string> _preferences;

        private PreferenceManager(string fileName)
        {
            _fileName = fileName;
            _jsonUtils = new JsonUtils<Dictionary<string, string>>(_fileName);
        }

        public static PreferenceManager GetInstance(string fileName)
        {
            if (_instance == null)
            {
                _instance = new PreferenceManager(fileName);
                _preferences = LoadPreferences();
            }
            return _instance;
        }

        public static PreferenceManager GetInstance()
        {
            if (_fileName == null) throw new Exception("Preference manager not initialised");
            return _instance;
        }

        public static T GetPreference<T>(string name, T defaultValue)
        {
            if (!_preferences.ContainsKey(name)) return defaultValue;
            var value = _preferences[name];
            return JsonUtils<T>.ReadJsonString(value);
        }

        public static void SavePreferences()
        {
            _jsonUtils.WriteObject(_preferences);
        }

        public static void SetPreference<T>(string name, T value)
        {
            var json = JsonUtils<T>.WriteJsonString(value);
            _preferences[name] = json;
        }

        private static Dictionary<string, string> LoadPreferences()
        {
            if (System.IO.File.Exists(_fileName))
            {
                return _jsonUtils.ReadObject();
            }
            return new Dictionary<string, string>();
        }
    }
}