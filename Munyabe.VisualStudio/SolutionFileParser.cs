using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Munyabe.Common;

namespace Munyabe.VisualStudio
{
    /// <summary>
    /// ソリューションファイルを解析するクラスです。
    /// </summary>
    public static class SolutionFileParser
    {
        /// <summary>
        /// プロジェクトファイルの一覧を取得します。
        /// </summary>
        /// <param name="solutionFilePath">ソリューションファイル</param>
        /// <returns>プロジェクトファイルの一覧</returns>
        public static IEnumerable<string> GetProjectFiles(string solutionFilePath)
        {
            return GetProjectFiles(solutionFilePath, true);
        }

        /// <summary>
        /// プロジェクトファイルの一覧を取得します。
        /// </summary>
        /// <param name="solutionFilePath">ソリューションファイル</param>
        /// <param name="isAbsolutePath">絶対パスを取得する場合は<see langword="true"/></param>
        /// <returns>プロジェクトファイルの一覧</returns>
        public static IEnumerable<string> GetProjectFiles(string solutionFilePath, bool isAbsolutePath)
        {
            Guard.ArgumentNotNullOrEmpty(solutionFilePath, "projectFilePath");
            if (solutionFilePath.EndsWith(".sln") == false)
            {
                throw new ArgumentException(string.Format("The path [{0}] is not solution file path.", solutionFilePath));
            }

            var allLines = File.ReadAllLines(solutionFilePath);
            var firstLine = allLines.FirstOrDefault(line => string.IsNullOrEmpty(line) == false);
            if (firstLine != null && firstLine.StartsWith("Microsoft Visual Studio Solution File") == false)
            {
                throw new InvalidOperationException("This solution file is unknown format.");
            }

            var files = allLines
                .Where(line => line.StartsWith("Project"))
                .Select(line =>
                {
                    var matches = Regex.Matches(line, "\"[^$\"\"]*\"");
                    if (1 < matches.Count)
                    {
                        var projectName = matches[1].Value;
                        var candidate = matches[2].Value;

                        if (projectName != candidate)
                        {
                            return candidate.Substring(1, candidate.Length - 2);
                        }
                    }

                    return string.Empty;
                })
                .Where(path => string.IsNullOrEmpty(path) == false);

            if (isAbsolutePath)
            {
                var baseDir = new FileInfo(solutionFilePath).Directory.FullName;
                return files.Select(path => Path.Combine(baseDir, path));
            }
            else
            {
                return files;
            }
        }
    }
}
