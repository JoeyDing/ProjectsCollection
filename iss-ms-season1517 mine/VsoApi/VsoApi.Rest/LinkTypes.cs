namespace VsoApi.Rest
{
    public static class LinkTypes
    {
        public static LinkType Child;
        public static LinkType Related;

        static LinkTypes()
        {
            Child = new LinkType("System.LinkTypes.Hierarchy-Reverse");
            Related = new LinkType("System.LinkTypes.Related");
        }
    }
}