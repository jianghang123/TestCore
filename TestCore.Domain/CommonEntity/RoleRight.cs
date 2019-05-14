using System;
using System.Linq;
using System.Runtime.Serialization;

namespace TestCore.Domain.CommonEntity
{

    [Serializable]
    public class UserRight
    {

        [DataMember]
        public Int64 UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int RoleId { get; set; }

        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        public int OperId { get; set; }


        [DataMember]
        public bool IsSupper { get; set; }

        [DataMember]
        public RoleRightItem[] Rights { get; set; }

        public string SiteIds { get; set; }


        public int SiteId { get; set; }


        public string SessionId { get; set; }


        public string ParentName { get; set; }


        public bool IsChild { get; set; }


        private int[] siteIdList;

        public int[] SiteIdList
        {
            get
            {
                if (siteIdList == null)
                {
                    if (!string.IsNullOrEmpty(this.SiteIds))
                    {
                        siteIdList = this.SiteIds.Split(',').Where(c => !string.IsNullOrEmpty(c)).Select(sid => int.Parse(sid)).ToArray();
                    }
                    else
                    {
                        siteIdList = new int[0];
                    }
                }
                return siteIdList;
            }
        }

        public bool HasRight(int rightId,params int[] nodeIds)
        {
            if (IsSupper) return true;

            if (nodeIds == null || !nodeIds.Any()) return false;

            return Rights.Where(c => nodeIds.Any(n=>n == c.NodeId) && c.RightId == rightId).Any();
        }

    }

    [Serializable]
    public class RoleRightItem
    {

        [DataMember]
        public int NodeId { get; set; }

        [DataMember]
        public int RightId { get; set; }

    }



}
