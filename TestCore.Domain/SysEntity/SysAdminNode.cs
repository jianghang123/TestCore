using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Common.Attributes;
using TestCore.Common.Helper;

namespace TestCore.Domain.SysEntity
{
    public class SysAdminNode
    {
        private List<int> rightIdList;

        [FieldInfo(IsAllowEdit = false)]
        public List<int> RightIdList
        {
            get
            {
                if (rightIdList == null)
                {
                    rightIdList = new List<int>();
                    foreach (var rid in this.Rights.Split(','))
                    {
                        rightIdList.Add(TypeHelper.TryParse(rid, 0));
                    }
                }
                return rightIdList;
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int SortIndex { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string RouteId { get; set; }
        public string QueryString { get; set; }
        public string Rights { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
