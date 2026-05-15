using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace EventBus.Editor
{
    public sealed class EventBusDebugWindow : EditorWindow
    {
        const int MaxEntries = 500;

        static readonly BindingFlags MemberFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        readonly List<EventEntry> entries = new List<EventEntry>();
        Vector2 scrollPosition;
        bool autoScroll = true;
        string filterText = string.Empty;

        [MenuItem("Window/Event Bus Logger")]
        public static void Open()
        {
            GetWindow<EventBusDebugWindow>("Event Bus Logger");
        }

        void OnEnable()
        {
            EventBusDebugHooks.EventRaised -= HandleEventRaised;
            EventBusDebugHooks.EventRaised += HandleEventRaised;
        }

        void OnDisable()
        {
            EventBusDebugHooks.EventRaised -= HandleEventRaised;
        }

        void HandleEventRaised(Type eventType, object eventData)
        {
            entries.Add(CreateEntry(eventType, eventData));
            if (entries.Count > MaxEntries)
            {
                entries.RemoveRange(0, entries.Count - MaxEntries);
            }

            Repaint();
        }

        void OnGUI()
        {
            DrawToolbar();
            EditorGUILayout.Space(4f);

            var filteredEntries = GetFilteredEntries();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < filteredEntries.Count; i++)
            {
                DrawEntry(filteredEntries[i]);
                if (i < filteredEntries.Count - 1)
                {
                    EditorGUILayout.Space(4f);
                }
            }
            EditorGUILayout.EndScrollView();

            if (autoScroll && Event.current.type == EventType.Repaint && filteredEntries.Count > 0)
            {
                scrollPosition.y = float.MaxValue;
            }
        }

        void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                autoScroll = GUILayout.Toggle(autoScroll, new GUIContent("Auto Scroll"), EditorStyles.toolbarButton, GUILayout.Width(85f));
                GUILayout.Space(8f);
                GUILayout.Label("Filter", GUILayout.Width(35f));
                filterText = GUILayout.TextField(filterText, EditorStyles.toolbarSearchField, GUILayout.MinWidth(120f));
                if (GUILayout.Button(GUIContent.none, EditorStyles.toolbarButton))
                {
                    filterText = string.Empty;
                    GUI.FocusControl(null);
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(60f)))
                {
                    entries.Clear();
                }
            }
        }

        List<EventEntry> GetFilteredEntries()
        {
            if (string.IsNullOrWhiteSpace(filterText))
            {
                return entries;
            }

            var term = filterText.Trim();
            return entries.Where(entry => entry.Matches(term)).ToList();
        }

        void DrawEntry(EventEntry entry)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField(entry.Title, EditorStyles.boldLabel);
                EditorGUILayout.LabelField(entry.Timestamp.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture), EditorStyles.miniLabel);

                for (int i = 0; i < entry.Lines.Count; i++)
                {
                    EditorGUILayout.SelectableLabel(entry.Lines[i], GUILayout.Height(EditorGUIUtility.singleLineHeight));
                }
            }
        }

        static EventEntry CreateEntry(Type eventType, object eventData)
        {
            var lines = new List<string>();
            if (eventData != null)
            {
                foreach (var member in GetInspectableMembers(eventData.GetType()))
                {
                    var value = GetMemberValue(member, eventData);
                    lines.Add($"{member.Name}: {FormatValue(value)}");
                }
            }

            return new EventEntry
            {
                Timestamp = DateTime.Now,
                Title = $"{eventType?.Name ?? "UnknownEvent"}",
                Lines = lines
            };
        }

        static IEnumerable<MemberInfo> GetInspectableMembers(Type type)
        {
            var fields = type.GetFields(MemberFlags)
            .Where(field => !field.IsDefined(typeof(CompilerGeneratedAttribute), true) && !field.Name.Contains("k__BackingField"));

            var properties = type.GetProperties(MemberFlags)
            .Where(property => property.CanRead && property.GetIndexParameters().Length == 0);

            return fields.Cast<MemberInfo>()
            .Concat(properties)
            .OrderBy(member => member.Name);
        }

        static object GetMemberValue(MemberInfo member, object instance)
        {
            switch (member)
            {
                case FieldInfo field:
                    return field.GetValue(instance);
                case PropertyInfo property:
                    try
                    {
                        return property.GetValue(instance, null);
                    }
                    catch
                    {
                        return "<unavailable>";
                    }
                default:
                    return null;
            }
        }

        static string FormatValue(object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is UnityEngine.Object unityObject)
            {
                return unityObject != null ? unityObject.name : "null";
            }

            if (value is string stringValue)
            {
                return stringValue;
            }

            if (value is IFormattable formattable)
            {
                return formattable.ToString(null, CultureInfo.InvariantCulture);
            }

            return value.ToString();
        }

        sealed class EventEntry
        {
            public DateTime Timestamp;
            public string Title;
            public List<string> Lines;

            public bool Matches(string term)
            {
                if (Title != null && Title.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }

                for (int i = 0; i < Lines.Count; i++)
                {
                    if (Lines[i].IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
