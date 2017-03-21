using System;

namespace ASummonersTale.Components.Settings
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class IniSection : Attribute
    {
        private string section;

        private object defaultValue;

        private Type type;

        public Type Type => type;

        public object MinimumValue, MaximumValue;

        public IniSection(string sectionName, object defaultValue, Type valueType)
        {
            section = sectionName;
            this.defaultValue = defaultValue;
            type = valueType;
        }

        public string Section => section;

        public dynamic DefaultValue => Convert.ChangeType(defaultValue, type);
    }
}
