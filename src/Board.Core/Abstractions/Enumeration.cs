using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Board.Core.Abstractions
{
    public abstract class Enumeration : IComparable
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static T FromValue<T>(int value) where T : Enumeration
        {
            var @enum = GetAll<T>().FirstOrDefault(i => i.Id == value);

            if (@enum == null)
                throw new InvalidOperationException($"Unable to find {typeof(T)} with Id '{value}'");

            return @enum;
        }

        public static T FromName<T>(string name) where T : Enumeration
        {
            var @enum = GetAll<T>().FirstOrDefault(i => i.Name == name);

            if (@enum == null)
                throw new InvalidOperationException($"Unable to find {typeof(T)} with name '{name}'");

            return @enum;
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}
