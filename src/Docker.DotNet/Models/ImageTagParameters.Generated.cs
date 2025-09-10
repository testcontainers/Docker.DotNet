namespace Docker.DotNet.Models
{
    public class ImageTagParameters // (main.ImageTagParameters)
    {
        [QueryStringParameter("repo", false)]
        public string RepositoryName { get; set; }

        [QueryStringParameter("tag", false)]
        public string Tag { get; set; }
    }
}
