using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Munyabe.Common;
using Munyabe.Common.Xml;

namespace Munyabe.VisualStudio
{
    /// <summary>
    /// プロジェクトファイルを解析するクラスです。
    /// </summary>
    public static class ProjectFileParser
    {
        /// <summary>
        /// 対象のプロジェクトファイルから指定したファイルのパスを取得します。
        /// </summary>
        /// <param name="projectFilePath">プロジェクトファイルのパス</param>
        /// <param name="fileType">ファイル種別</param>
        /// <returns>指定したファイル一覧</returns>
        /// <exception cref="ArgumentException"><paramref name="projectFilePath"/>はプロジェクトファイルではありません。</exception>
        /// <exception cref="ArgumentException">指定されたプロジェクトファイルは未知のフォーマットです。</exception>
        public static IEnumerable<string> GetFiles(string projectFilePath, FileTypeFlags fileType)
        {
            return GetFiles(projectFilePath, fileType, true);
        }

        /// <summary>
        /// 対象のプロジェクトファイルから指定したファイルのパスを取得します。
        /// </summary>
        /// <param name="projectFilePath">プロジェクトファイルのパス</param>
        /// <param name="fileType">ファイル種別</param>
        /// <param name="isAbsolutePath"></param>
        /// <returns>指定したファイル一覧</returns>
        /// <exception cref="ArgumentException"><paramref name="projectFilePath"/>はプロジェクトファイルではありません。</exception>
        /// <exception cref="ArgumentException">指定されたプロジェクトファイルは未知のフォーマットです。</exception>
        public static IEnumerable<string> GetFiles(string projectFilePath, FileTypeFlags fileType, bool isAbsolutePath)
        {
            Guard.ArgumentNotNullOrEmpty(projectFilePath, "projectFilePath");
            ArgumentNotProjectFilePath(projectFilePath);

            var document = XDocument.Load(projectFilePath, LoadOptions.SetBaseUri);
            var defaultNamespace = document.Root.GetDefaultNamespace();

            ArgumentKnownFormat(document, defaultNamespace);

            var attributes = Enumerable.Empty<string>();

            if ((fileType & FileTypeFlags.CSharp) != 0)
            {
                var csFiles = GetItemGroupAttributeValues(document, defaultNamespace, "Compile", "Include")
                    .Where(value => value.EndsWith(".cs"));
                attributes = attributes.Concat(csFiles);
            }
            if ((fileType & FileTypeFlags.Xaml) != 0)
            {
                var xamlFiles = GetItemGroupAttributeValues(document, defaultNamespace, "Page", "Include")
                    .Where(value => value.EndsWith(".xaml"));
                attributes = attributes.Concat(xamlFiles);
            }
            if ((fileType & FileTypeFlags.ResX) != 0)
            {
                var dependentUpontXName = defaultNamespace.GetName("DependentUpon");
                var resxFiles = GetItemGroupElements(document, defaultNamespace, "Compile", "EmbeddedResource")
                    .Select(element =>
                    {
                        var includeValue = element.GetAttributeValueOrDefault("Include");
                        return new
                        {
                            Element = element.Element(dependentUpontXName),
                            DirectoryPath = string.IsNullOrEmpty(includeValue) ?
                                string.Empty : Path.GetDirectoryName(includeValue)
                        };
                    })
                    .Where(each => each.Element != null)
                    .Select(each => Path.Combine(each.DirectoryPath, each.Element.Value))
                    .Where(value => value.EndsWith(".resx"))
                    .Distinct();
                attributes = attributes.Concat(resxFiles);
            }

            var baseDir = isAbsolutePath ? Path.GetDirectoryName(projectFilePath) : string.Empty;
            return attributes.Select(value => Path.Combine(baseDir, value));
        }

        /// <summary>
        /// 解析可能なプロジェクトファイルのフォーマットであることを示します。
        /// </summary>
        private static void ArgumentKnownFormat(XDocument document, XNamespace defaultNamespace)
        {
            var element = document.Elements().FirstOrDefault();
            if (element == null ||
                element.Name != defaultNamespace.GetName("Project") ||
                defaultNamespace.NamespaceName != "http://schemas.microsoft.com/developer/msbuild/2003")
            {
                throw new ArgumentException("This project file is unknown format.");
            }
        }

        /// <summary>
        /// <param name="projectFilePath" />のパスがプロジェクトファイルであることを示します。
        /// </summary>
        private static void ArgumentNotProjectFilePath(string projectFilePath)
        {
            if (projectFilePath.EndsWith("proj") == false)
            {
                throw new ArgumentException(string.Format("The path [{0}] is not project file path.", projectFilePath));
            }
        }

        /// <summary>
        /// ItemGroup 要素内の指定の要素の属性を取得します。
        /// </summary>
        private static IEnumerable<string> GetItemGroupAttributeValues(
            XContainer container, XNamespace defaultNamespace, string elementName, string attributeName)
        {
            var projectXName = defaultNamespace.GetName("Project");
            var itemGroupXName = defaultNamespace.GetName("ItemGroup");
            var elementXName = defaultNamespace.GetName(elementName);

            return container
                .Elements(projectXName)
                .Elements(itemGroupXName)
                .Elements(elementXName)
                .Where(element => element != null)
                .Attributes(attributeName)
                .Where(attribute => attribute != null)
                .Select(attribute => attribute.Value);
        }

        /// <summary>
        /// ItemGroup 要素内の指定の要素を取得します。
        /// </summary>
        private static IEnumerable<XElement> GetItemGroupElements(XContainer container, XNamespace defaultNamespace, params string[] elementNames)
        {
            var projectXName = defaultNamespace.GetName("Project");
            var itemGroupXName = defaultNamespace.GetName("ItemGroup");

            return elementNames.SelectMany(name =>
                {
                    var targetXName = defaultNamespace.GetName(name);
                    return container
                        .Elements(projectXName)
                        .Elements(itemGroupXName)
                        .Elements(targetXName)
                        .Where(element => element != null);
                });
        }
    }
}