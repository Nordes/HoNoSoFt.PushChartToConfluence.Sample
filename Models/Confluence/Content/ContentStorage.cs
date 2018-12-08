using System;

namespace HoNoSoFt.PushChartToConfluence.Sample.Models.Confluence.Content
{
    public partial class ContentStorage
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public Version Version { get; set; }
        public Body Body { get; set; }
        public Extensions Extensions { get; set; }
        public ContentStorageQueryExpandable Expandable { get; set; }
        public ContentStorageQueryLinks Links { get; set; }
    }

    public partial class Body
    {
        public Storage Storage { get; set; }
        public BodyExpandable Expandable { get; set; }
    }

    public partial class BodyExpandable
    {
        public string Editor { get; set; }
        public string View { get; set; }
        public string ExportView { get; set; }
        public string StyledView { get; set; }
        public string AnonymousExportView { get; set; }
    }

    public partial class Storage
    {
        public string Value { get; set; }
        public string Representation { get; set; }
        public StorageExpandable Expandable { get; set; }
    }

    public partial class StorageExpandable
    {
        public string Content { get; set; }
    }

    public partial class ContentStorageQueryExpandable
    {
        public string Container { get; set; }
        public string Metadata { get; set; }
        public string Operations { get; set; }
        public string Children { get; set; }
        public string Restrictions { get; set; }
        public string History { get; set; }
        public string Ancestors { get; set; }
        public string Descendants { get; set; }
        public string Space { get; set; }
    }

    public partial class Extensions
    {
        public string Position { get; set; }
    }

    public partial class ContentStorageQueryLinks
    {
        public Uri Self { get; set; }
        public Uri Base { get; set; }
        public string Context { get; set; }
        public string Collection { get; set; }
        public string Webui { get; set; }
        public string Edit { get; set; }
        public string Tinyui { get; set; }
    }

    public partial class Version
    {
        public By By { get; set; }
        public DateTimeOffset When { get; set; }
        public string Message { get; set; }
        public long Number { get; set; }
        public bool MinorEdit { get; set; }
        public bool Hidden { get; set; }
        public StorageExpandable Expandable { get; set; }
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
}