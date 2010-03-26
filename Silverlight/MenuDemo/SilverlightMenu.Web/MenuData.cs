using System.Collections.Generic;

namespace SilverlightMenu.Web
{
    public class MenuData
    {
        public int Id { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 菜单项目
        /// </summary>
        public List<MenuItem> MenuItems { get; set; }
        /// <summary>
        /// 是否为置顶菜单
        /// </summary>
        public bool IsTop { get; set; }
    }

    public class MenuItem
    {
        public int Id { get; set; }
        /// <summary>
        /// 菜单标题
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// 菜单链接
        /// </summary>
        public string Url { get; set; }
    }
}