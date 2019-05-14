
namespace TestCore.Framework.Strategy
{ 
    public class FileViewModel
    {
        public FileViewModel() { }
        /// <summary>
        /// 上传文件查看的Model（包括guid、url、名称以及ICON前置图标）
        /// </summary>
        /// <param name="guid">guid</param>
        /// <param name="url">路径（包括文件名和扩展名）</param>
        /// <param name="name">文件名（包括路径和文件名）</param>
        /// <param name="icon">前置小图标</param>
        public FileViewModel(string guid, string url, string name, string icon)
        {
            this.Guid = guid;
            this.Url = url;
            this.Name = name;
            this.Icon = icon;
        }
        /// <summary>
        /// guid(通过Guid.NewGuid()生成）
        /// </summary>
        public string Guid { get; private set; }
        /// <summary>
        /// 路径（包括文件名和扩展名）
        /// </summary>
        public string Url { get; private set; }
        /// <summary>
        /// 文件名称（不包含路径）
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 显示前置图标
        /// </summary>
        public string Icon { get; private set; }
    }
}