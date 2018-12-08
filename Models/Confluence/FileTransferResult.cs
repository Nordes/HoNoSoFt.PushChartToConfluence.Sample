using System;

namespace HoNoSoFt.PushChartToConfluence.Sample.Models.Confluence
{
    public partial class FileTransferResult
    {
        public Result[] Results { get; set; }
        public long Size { get; set; }
        public ResponseBodyLinks Links { get; set; }
    }

    public partial class ResponseBodyLinks
    {
        public Uri Base { get; set; }
        public string Context { get; set; }
    }

    public partial class Result
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public Version Version { get; set; }
        public Container Container { get; set; }
        public Metadata Metadata { get; set; }
        public ResultExtensions Extensions { get; set; }
        public ResultExpandable Expandable { get; set; }
        public ResultLinks Links { get; set; }
    }

    public partial class Container
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public ContainerExtensions Extensions { get; set; }
        public ContainerExpandable Expandable { get; set; }
        public ContainerLinks Links { get; set; }
    }

    public partial class ContainerExpandable
    {
        public string Container { get; set; }
        public string Metadata { get; set; }
        public string Operations { get; set; }
        public string Children { get; set; }
        public string Restrictions { get; set; }
        public string History { get; set; }
        public string Ancestors { get; set; }
        public string Body { get; set; }
        public string Version { get; set; }
        public string Descendants { get; set; }
        public string Space { get; set; }
    }

    public partial class ContainerExtensions
    {
        public string Position { get; set; }
    }

    public partial class ContainerLinks
    {
        public Uri Self { get; set; }
        public string Webui { get; set; }
        public string Edit { get; set; }
        public string Tinyui { get; set; }
    }

    public partial class ResultExpandable
    {
        public string Operations { get; set; }
        public string Children { get; set; }
        public string Restrictions { get; set; }
        public string History { get; set; }
        public string Ancestors { get; set; }
        public string Body { get; set; }
        public string Descendants { get; set; }
        public string Space { get; set; }
    }

    public partial class ResultExtensions
    {
        public string MediaType { get; set; }
        public long FileSize { get; set; }
    }

    public partial class ResultLinks
    {
        public Uri Self { get; set; }
        public Uri Base { get; set; }
        public string Context { get; set; }
        public string Collection { get; set; }
        public string Webui { get; set; }
        public string Download { get; set; }
    }

    public partial class Metadata
    {
        public string MediaType { get; set; }
        public MetadataExpandable Expandable { get; set; }
        public Labels Labels { get; set; }
    }

    public partial class Labels
    {
        public object[] Results { get; set; }
        public long Start { get; set; }
        public long Limit { get; set; }
        public long Size { get; set; }
        public VersionLinks Links { get; set; }
    }

    public partial class MetadataExpandable
    {
        public string Currentuser { get; set; }
        public string Properties { get; set; }
        public string Frontend { get; set; }
        public string EditorHtml { get; set; }
    }

    public partial class Version
    {
        public By By { get; set; }
        public DateTimeOffset When { get; set; }
        public long Number { get; set; }
        public bool MinorEdit { get; set; }
        public bool Hidden { get; set; }
        public VersionExpandable Expandable { get; set; }
        public VersionLinks Links { get; set; }
    }

    public partial class By
    {
        public string Type { get; set; }
        public string Username { get; set; }
        public string UserKey { get; set; }
        public ProfilePicture ProfilePicture { get; set; }
        public string DisplayName { get; set; }
        public ByExpandable Expandable { get; set; }
        public VersionLinks Links { get; set; }
    }

    public partial class ByExpandable
    {
        public string Status { get; set; }
    }

    public partial class VersionLinks
    {
        public Uri Self { get; set; }
    }

    public partial class ProfilePicture
    {
        public string Path { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        public bool IsDefault { get; set; }
    }

    public partial class VersionExpandable
    {
        public string Content { get; set; }
    }
}