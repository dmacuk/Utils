using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils.File;

namespace Utils.Preference
{
    public sealed class PreferenceManager
    {
        private static Dictionary<string, string> _preferences;
        private static JsonUtils<Dictionary<string, string>> _jsonUtils;
        private static string _fileName;
        private static PreferenceManager _instance;

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
                LoadPreferences();
            }
            return _instance;
        }

        public static PreferenceManager GetInstance()
        {
            if (_fileName == null) throw new Exception("Preference manager not initialised");
            return _instance;
        }


        private static async void LoadPreferences()
        {
            if (System.IO.File.Exists(_fileName))
            {
                _preferences = await _jsonUtils.ReadObjectAsync();

            }
            else
            {
                _preferences = new Dictionary<string, string>();
            }

        }

        public async void SavePreferences()
        {
            await _jsonUtils.WriteObjectAsync(_preferences);
        }

        public static T GetPreference<T>(string name, T defaultValue)
        {
            if (_preferences == null) throw new PreferenceException("Preferences not loaded");
            if (!_preferences.ContainsKey(name)) return defaultValue;
            var value = _preferences[name];
            return JsonUtils<T>.ReadJsonString(value);
        }

        public static void SetPreference<T>(string name, T value)
        {
            var json = JsonUtils<T>.WriteJsonString(value);
            _preferences[name] = json;
        }
    }

    public class PreferenceException : Exception
    {
        public PreferenceException(string message):base(message)
        {
        }
    }
}
