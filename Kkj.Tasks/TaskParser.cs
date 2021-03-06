﻿using JetBrains.Annotations;
using System;
using System.Text.RegularExpressions;

namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a task parser that parses a serialized tasks.
    /// </summary>
    public class TaskParser : ITaskParser
    {
        /// <summary>
        /// Single line task pattern.
        /// </summary>
        internal readonly static Regex SingleLinePattern =
            // priority | status | name | [tags] //
            new Regex(@"^\s*([!?])?\s*\[([ X])\]\s*([^{]+)(?:{\s*([^}]+)})?$");

        /// <summary>
        /// Initializes a new <see cref="ParserResult"/> based on the
        /// serialized task.
        /// </summary>
        /// <remarks>
        /// Only one serialized task must be provided.
        /// </remarks>
        /// <param name="serializedTask">
        /// A serialized task.
        /// </param>
        /// <returns>
        /// A parser result or <c>null</c> if no task.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If serializedTask is <c>null</c>.
        /// </exception>
        public ParserResult Parse([NotNull] string serializedTask)
        {
            if (serializedTask == null)
            {
                throw new ArgumentNullException("serializedTask");
            }
            if (string.IsNullOrWhiteSpace(serializedTask))
            {
                return null;
            }
            var tokens = GetTokens(serializedTask);
            if (tokens == null)
            {
                return null;
            }
            var priority = ParsePriority(tokens.Item1);
            var status = ParseStatus(tokens.Item2);
            var name = ParseName(tokens.Item3);
            return new ParserResult
            {
                Priority = priority,
                Status = status,
                Name = name
            };
        }

        /// <summary>
        /// Get the task tokens.
        /// </summary>
        /// <param name="serializedTask">
        /// A serialized task.
        /// </param>
        /// <returns>
        /// The task tokens or <c>null</c> if not a task.
        /// </returns>
        // Atm: <priority, status, name>
        [CanBeNull]
        internal virtual Tuple<string, string, string>
            GetTokens(string serializedTask)
        {
            var matches = SingleLinePattern.Match(serializedTask);
            return matches.Success ?
                new Tuple<string, string, string>(
                    matches.Groups[1].Value,
                    matches.Groups[2].Value,
                    matches.Groups[3].Value
                ) :
                null;
        }

        /// <summary>
        /// Parses the specified priority string and finds the related task
        /// priority.
        /// </summary>
        /// <param name="priorityString">
        /// The priority string to parse.
        /// </param>
        /// <returns>
        /// The task priority.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If priorityString is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If not able to parse priorityString.
        /// </exception>
        internal virtual TaskPriority ParsePriority(
            [NotNull] string priorityString
        )
        {
            if (priorityString == null)
            {
                throw new ArgumentNullException("priorityString");
            }
            switch (priorityString)
            {
                case "":
                    return TaskPriority.Normal;
                case "!":
                    return TaskPriority.High;
                case "?":
                    return TaskPriority.Low;
            }
            throw new ArgumentException(
                "Invalid priority.",
                "priorityString"
            );
        }

        /// <summary>
        /// Parses the specified status string and finds the related task
        /// status.
        /// </summary>
        /// <param name="statusString">
        /// The status string to parse.
        /// </param>
        /// <returns>
        /// The task status.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If statuString is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If not able to parse statuString.
        /// </exception>
        internal virtual TaskStatus ParseStatus([NotNull] string statusString)
        {
            if (statusString == null)
            {
                throw new ArgumentNullException("statusString");
            }
            switch (statusString)
            {
                case " ":
                    return TaskStatus.Pending;
                case "X":
                    return TaskStatus.Done;
            }
            throw new ArgumentException(
                "Invalid status.",
                "statusString"
            );
        }

        /// <summary>
        /// Parses the specified name string and finds the related task name.
        /// </summary>
        /// <param name="nameString">
        /// The name string to parse.
        /// </param>
        /// <returns>
        /// The task name.
        /// </returns>
        [NotNull]
        internal virtual string ParseName([NotNull] string nameString)
        {
            if (nameString == null)
            {
                throw new ArgumentNullException("nameString");
            }
            return nameString.Trim();
        }
    }
}