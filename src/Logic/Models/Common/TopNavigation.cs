namespace ScBootstrap.Logic.Models.Common
{
    public class TopNavigation
    {
        public string MenuTitle { get; set; }
        public string Url { get; set; }
        public bool HideNavi { get; set; }
        public Navigation[] Children { get; set; }
    }

}
