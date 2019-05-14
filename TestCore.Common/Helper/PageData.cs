using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCore.Common.Helper
{

    #region 分页

    /// <summary>
    /// 分页查询结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageData<T> 
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 查询结果
        /// </summary>
        public IEnumerable<T> Result { get; set; }

    }

    /// <summary>
    /// 条件比较
    /// </summary>
    public enum CompareType
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equals,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEquals,

        /// <summary>
        /// 包含,相当于 Like '%xxx%'
        /// </summary>
        Like,

        /// <summary>
        /// 在.... 之中，相当于 In， 值是数组类型
        /// </summary>
        In,

        /// <summary>
        /// 在.... 之中，相当于 not In， 值是数组类型
        /// </summary>
        NotIn,

        /// <summary>
        ///  相当于 is null, 不用值
        /// </summary>
        IsNull,

        /// <summary>
        ///  相当于 is not null, 不用值
        /// </summary>
        IsNotNull,

        /// <summary>
        /// 相当于 Like '%xxx'
        /// </summary>
        StartsWith,

        /// <summary>
        /// 相当于 Like 'xxx%'
        /// </summary>
        EndsWith,

        /// <summary>
        /// 大于,相当于 > 
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 小于, 相当于 <
        /// </summary>
        LessThan,

        /// <summary>
        /// 大于等于，相当于 >=
        /// </summary>
        GreaterThanEquals,


        /// <summary>
        /// 小于等于，相当于 <=
        /// </summary>
        LessThanEquals

    }


    public enum LogicType
    {
        AND,
        OR
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConditionItem
    {
        /// <summary>
        /// 成员名称
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        public string ParamName { get; set; }

        private CompareType compareType = CompareType.Equals;

        public CompareType CompareType
        {
            get {
                if(this.FieldValue is Array)
                {
                    if(compareType != CompareType.In && compareType != CompareType.NotIn)
                    {
                        compareType = CompareType.In;
                    }
                }
                return compareType;
            }
            set
            {
                compareType = value;
            }
        }

        public LogicType LogicType { get; set; } = LogicType.AND;

        /// <summary>
        /// 字段的值
        /// </summary>
        public object FieldValue {  get; set; }


        public ConditionItem(string fieldName, object val, CompareType compareType = CompareType.Equals, LogicType logicType = LogicType.AND)
        {
            this.FieldName = fieldName;
            this.ParamName = fieldName;
            this.CompareType = compareType;
            this.LogicType = logicType;
            this.FieldValue = val;
        }

        public ConditionItem()
        {

        }

        /// <summary>
        /// 参数的值
        /// </summary>
        public object ParamValue
        {
            get {
                if (FieldValue is DateTime)
                {
                    return Convert.ToDateTime(FieldValue).ToString("yyyy-MM-dd HH:mm:ssss");
                }
                switch (CompareType)
                {
                    case CompareType.Equals:
                    case CompareType.NotEquals:

                    case CompareType.In:
                    case CompareType.NotIn:

                    case CompareType.LessThan:
                    case CompareType.GreaterThan:

                    case CompareType.LessThanEquals:
                    case CompareType.GreaterThanEquals:
                        return FieldValue;

                    case CompareType.IsNull:
                    case CompareType.IsNotNull:
                        return null;
                    case CompareType.Like:
                        return string.Format("%{0}%", FieldValue);
                    case CompareType.StartsWith:
                        return string.Format("{0}%", FieldValue);
                    case CompareType.EndsWith:
                        return string.Format("%{0}", FieldValue);
                    default: return FieldValue;
                }
            }
        }

        public string ConditionString
        {
            get
            {
                switch (CompareType)
                {
                    case CompareType.Equals:
                        return string.Format("[{0}] = @{1}", FieldName, ParamName);
                    case CompareType.Like:
                        return string.Format("[{0}] LIKE @{1}", FieldName, ParamName);
                    case CompareType.StartsWith:
                        return string.Format("[{0}] LIKE @{1}", FieldName, ParamName);
                    case CompareType.EndsWith:
                        return string.Format("[{0}] LIKE @{1}", FieldName, ParamName);

                    case CompareType.In:
                        return string.Format("[{0}] IN @{1}", FieldName, ParamName);
                    case CompareType.NotIn:
                        return string.Format("[{0}] NOT IN @{1}", FieldName, ParamName);

                    case CompareType.LessThan:
                        return string.Format("[{0}] < @{1}", FieldName, ParamName);
                    case CompareType.GreaterThan:
                        return string.Format("[{0}] > @{1}", FieldName, ParamName);

                    case CompareType.LessThanEquals:
                        return string.Format("[{0}] <= @{1}", FieldName, ParamName);
                    case CompareType.GreaterThanEquals:
                        return string.Format("[{0}] >= @{1}", FieldName, ParamName);

                    case CompareType.IsNull:
                        return string.Format("[{0}] IS NULL", FieldName);
                    case CompareType.IsNotNull:
                        return string.Format("[{0}] IS NOT NULL", FieldName);

                    case CompareType.NotEquals:
                        return string.Format("[{0}] <> @{1}", FieldName, ParamName);
                    default: return string.Format("[{0}]=@{1}", FieldName, ParamName);
                }
            }
        }
    }

    /// <summary>
    /// 构建条件参数的实体类
    /// </summary>
    public class Condition
    {
        public LogicType LogicType { get; set; } = LogicType.AND;

        private List<Condition> ConditionList = new List<Condition>();

        private bool IsGroup = false;

        private List<ConditionItem> ConditionItems = new List<ConditionItem>();

        public Condition( )
        {
             
        }


        public Condition(object param, CompareType compareType = CompareType.Equals, LogicType logicType = LogicType.AND)
        {
            var items = GetListItems(param, compareType, logicType);

            if (items.Count > 0)
            {
                this.ConditionItems.AddRange(items);
            }
        }

        public static Condition Create(object param, CompareType compareType = CompareType.Equals, LogicType logicType = LogicType.AND)
        {
            Condition cond = new Condition( );

            var items = GetListItems(param, compareType, logicType);

            if (items.Count > 0)
            {
                cond.ConditionItems.AddRange(items);
            }
            return cond;
        }


        public static Condition Create( )
        {
            return  new Condition( );
        }

        private static List<ConditionItem> GetListItems(object param, CompareType compareType = CompareType.Equals, LogicType logicType = LogicType.AND )
        {
            if (param == null) return new List<ConditionItem>();
            return  param.GetType().GetProperties().Select(p => new ConditionItem
            {
                FieldName = p.Name,
                ParamName = p.Name,
                CompareType = compareType,
                LogicType = logicType,
                FieldValue = p.GetValue(param, null),
            }).ToList();
        }


        /// <summary>
        /// where 一个条件
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="paramValue">字段值</param>
        /// <param name="compareType">比较运算符</param>
        /// <returns></returns>
        public Condition Where(string fieldName, object paramValue, CompareType compareType = CompareType.Equals)
        {
            return this.BuildCondition(fieldName, paramValue, compareType, LogicType.AND);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param">参数实体类</param>
        /// <param name="compareType">比较运算符</param>
        /// <param name="logicType">组内逻辑运算符</param>
        /// <returns></returns>
        public Condition Where(object param, CompareType compareType = CompareType.Equals, LogicType logicType = LogicType.AND)
        {
            return this.BuildCondition(param, compareType, logicType, LogicType.AND);
        }


        #region private BuildCondition

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param">参数实体类</param>
        /// <param name="compareType">比较运算符</param>
        /// <param name="innerLogicType">组内逻辑运算符</param>
        /// <param name="outerLogicType">组外逻辑运算符</param>
        /// <returns></returns>
        private Condition BuildCondition(object param, CompareType compareType = CompareType.Equals, LogicType innerLogicType = LogicType.AND, LogicType outerLogicType = LogicType.AND)
        {
            this.ConditionStatus = ConditionStatus.Default;

            Condition cond = Create(param, compareType, innerLogicType);

            cond.LogicType = outerLogicType;

            this.ConditionList.Add(cond);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="compareType">比较运算符</param>
        /// <param name="outLogicType">逻辑运算符</param>
        /// <returns></returns>
        private Condition BuildCondition(string fieldName, object paramValue, CompareType compareType = CompareType.Equals, LogicType outLogicType = LogicType.AND)
        {
            Condition cond = new Condition();

            cond.LogicType = outLogicType;

            this.ConditionStatus = ConditionStatus.Default;

            cond.ConditionItems.Add(new ConditionItem
            {
                CompareType = compareType,
                FieldName = fieldName,
                ParamName = fieldName,
                FieldValue = paramValue,
                LogicType = outLogicType
            });
            this.ConditionList.Add(cond);

            return this;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="compareType">比较运算符</param>
        /// <returns></returns>
        public Condition Or(string fieldName, object paramValue, CompareType compareType = CompareType.Equals)
        {
            return this.BuildCondition(fieldName, paramValue, compareType, LogicType.OR);
        }


        /// <summary>
        /// 或一个条件
        /// </summary>
        /// <param name="param">参数实体类</param>
        /// <param name="compareType">比较运算符</param>
        /// <param name="logicType">组内逻辑运算符</param>
        /// <returns></returns>
        public Condition Or(object param, CompareType compareType = CompareType.Equals, LogicType logicType = LogicType.AND)
        {
            return this.BuildCondition(param, compareType, logicType, LogicType.OR);
        }

        /// <summary>
        /// and 一个条件
        /// </summary>
        /// <param name="param">参数实体类</param>
        /// <param name="compareType">比较运算符</param>
        /// <param name="logicType">组内逻辑运算符</param>
        /// <returns></returns>
        public Condition And(object param, CompareType compareType = CompareType.Equals, LogicType logicType = LogicType.AND)
        {
            return Where(param, compareType, logicType);
        }


        #region Group

        /// <summary>
        /// and 一个条件组  
        /// </summary>
        /// <param name="param"></param>
        /// <param name="compareType"></param>
        /// <param name="logicType"></param>
        /// <returns></returns>
        public Condition AndGroup(object param, CompareType compareType = CompareType.Equals, LogicType logicType = LogicType.AND)
        {
            var cond = Create(param, compareType, logicType);

            return AndGroup(cond);
        }


        /// <summary>
        ///  or 一个添加组
        /// </summary>
        /// <param name="param"></param>
        /// <param name="compareType"></param>
        /// <param name="logicType"></param>
        /// <returns></returns>
        public Condition OrGroup(object param, CompareType compareType = CompareType.Equals, LogicType logicType = LogicType.AND)
        {
            var cond = Create(param, compareType, logicType);

            return OrGroup(cond);
        }

        public Condition AndGroup(Condition cond)
        {
            cond.LogicType = LogicType.AND;

            cond.IsGroup = true;

            this.ConditionStatus = ConditionStatus.Default;
 
            this.ConditionList.Add(cond);

            return this;
        }

        public Condition OrGroup(Condition cond)
        {
            cond.LogicType = LogicType.OR;

            cond.IsGroup = true;

            this.ConditionStatus = ConditionStatus.Default;

            this.ConditionList.Add(cond);

            return this;
        }

        #endregion

        /// <summary>
        /// 相当于 IN
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition In(object param)
        {
            return Where(param, CompareType.In);
        }

       

        /// <summary>
        /// 相当于  Not IN
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition NotIn(object param)
        {
            return Where(param, CompareType.NotIn);
        }


        /// <summary>
        /// 相当于 Is  Null
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public Condition IsNull(string fieldName)
        {
            return this.Where(fieldName, null, CompareType.IsNull);
        }

        /// <summary>
        ///  相当于 Is Not Null
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public Condition IsNotNull(string fieldName)
        {
            return this.Where(fieldName, null, CompareType.IsNotNull );
        }


        /// <summary>
        /// 不等于 相当于 <>
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition NotEquals(object param)
        {
            return Where(param, CompareType.NotEquals);
        }

        /// <summary>
        /// 模糊查询 相当于 Like %XX%
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition Like(object param, LogicType  logicType = LogicType.AND)
        {
            return Where(param, CompareType.Like, logicType);
        }

        /// <summary>
        /// 模糊查询 相当于 Like XX%
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition StartsWith(object param, LogicType logicType = LogicType.AND)
        {
            return Where(param, CompareType.StartsWith,logicType);
        }

        /// <summary>
        /// 模糊查询 相当于 Like %XX
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition EndsWith(object param, LogicType logicType = LogicType.AND)
        {
            return Where(param, CompareType.EndsWith, logicType);
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="param"></param>
        /// <param name="logicType"></param>
        /// <returns></returns>
        public Condition GreaterThan(object param)
        {
            return Where(param, CompareType.GreaterThan);
        }

        /// <summary>
        /// 大于或者等于
        /// </summary>
        /// <param name="param"></param>
        /// <param name="logicType"></param>
        /// <returns></returns>
        public Condition GreaterThanEquals(object param)
        {
            return Where(param, CompareType.GreaterThanEquals);
        }

        /// <summary>
        /// 小于或者等于
        /// </summary>
        /// <param name="param"></param>
        /// <param name="logicType"></param>
        /// <returns></returns>
        public Condition LessThanEquals(object param)
        {
            return Where(param, CompareType.LessThanEquals);
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="param"></param>
        /// <param name="logicType"></param>
        /// <returns></returns>
        public Condition LessThan(object param)
        {
            return Where(param, CompareType.LessThan);
        }

        #region OR

        /// <summary>
        /// 相当于 或 IN
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition OrIn(object param)
        {
            return Or(param, CompareType.In);
        }

        /// <summary>
        /// 相当于 或  Not IN
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition OrNotIn(object param)
        {
            return Or(param, CompareType.NotIn);
        }

        /// <summary>
        /// 或 IS  NULL
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public Condition OrIsNull(string fieldName)
        {
            return this.Or(fieldName, null, CompareType.IsNull);
        }

        /// <summary>
        /// 或 IS NOT NULL
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public Condition OrIsNotNull(string fieldName)
        {
            return this.Or(fieldName, null, CompareType.IsNotNull);
        }

        /// <summary>
        /// 或 不等于
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition OrNotEquals(object param)
        {
            return Or(param, CompareType.NotEquals);
        }

        /// <summary>
        /// 或  Like
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition OrLike(object param)
        {
            return Or(param, CompareType.Like);
        }

        /// <summary>
        /// 或  Like XX%
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition OrStartsWith(object param)
        {
            return Or(param, CompareType.StartsWith);
        }


        /// <summary>
        /// 或  Like %XX
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Condition OrEndsWith(object param)
        {
            return Or(param, CompareType.EndsWith);
        }

        /// <summary>
        /// 或 大于
        /// </summary>
        /// <param name="param"></param>
        /// <param name="logicType"></param>
        /// <returns></returns>
        public Condition OrGreaterThan(object param)
        {
            return Or(param, CompareType.GreaterThan);
        }

        /// <summary>
        /// 或 大于等于
        /// </summary>
        /// <param name="param"></param>
        /// <param name="logicType"></param>
        /// <returns></returns>
        public Condition OrGreaterThanEquals(object param)
        {
            return Or(param, CompareType.GreaterThanEquals);
        }

        /// <summary>
        /// 或 小于等于
        /// </summary>
        /// <param name="param"></param>
        /// <param name="logicType"></param>
        /// <returns></returns>
        public Condition OrLessThanEquals(object param)
        {
            return Or(param, CompareType.LessThanEquals);
        }

        /// <summary>
        /// 或 小于
        /// </summary>
        /// <param name="param"></param>
        /// <param name="logicType"></param>
        /// <returns></returns>
        public Condition OrLessThan(object param)
        {
            return Or(param, CompareType.LessThan);
        }

        #endregion

        #region WhereString

        private string whereString;

        private int ParamNameIndex = 0;

        private ConditionStatus ConditionStatus = ConditionStatus.Default;

        public string WhereString
        {
            get
            {
                if (!this.ConditionList.Any()  )
                {
                    this.whereString = "1=1";
                    return this.whereString;
                }
                if (ConditionStatus == ConditionStatus.Default)
                {
                    this.BuildParamList();
                }
                if(ConditionStatus == ConditionStatus.BuildParam || this.whereString == null)
                {
                    this.whereString = this.GetWhereString(this);
  
                    ConditionStatus = ConditionStatus.BuildWhere;
                }
                return this.whereString;
            }
        }

        private string GetWhereString(Condition condition)
        {
            var itemWhere = GetCondString(condition.ConditionItems);

            if (!condition.ConditionList.Any()) return itemWhere;

            var condList = condition.ConditionList.Where(c => !c.IsGroup).ToList();

            var condWhere = new StringBuilder(itemWhere);

            if (condList.Any())
            {
                foreach (var cond in condList)
                {
                    var item = GetWhereString(cond);
 
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        if(condWhere.Length > 0)
                        {
                            condWhere.Append(" " + cond.LogicType.ToString() + " ");
                        }
                        condWhere.Append(item);
                    }
                }
            }

            if (condList.Count() > 1 || (!string.IsNullOrWhiteSpace(itemWhere) && condList.Count() == 1))
            {
                condWhere.Insert(0, "(").Append(")");
            }

            var where = new StringBuilder();

            where.Append(condWhere);


            var groupList = condition.ConditionList.Where(c => c.IsGroup).ToList();

            if (groupList.Any())
            {
                var groupWhere = new StringBuilder();

                foreach (var cond in groupList)
                {
                    var item = this.GetWhereString(cond);

                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        if(groupWhere.Length > 0)
                        {
                            groupWhere.Append( string.Format(" {0} ", cond.LogicType.ToString()));
                        }
                        groupWhere.Append(item);
                    }
                }
                if (groupList.Count() > 1 )
                {
                    groupWhere.Insert(0, "(").Append(")");
                }

                if(condWhere.Length > 0 &&  groupWhere.Length  > 0)
                {
                    where.Append(string.Format(" {0} {1}", groupList.Select(c => c.LogicType).FirstOrDefault() + "", groupWhere.ToString()));
                }
            }
            return where.ToString();
        }

        private string GetCondString(List<ConditionItem> items)
        {
            if (!items.Any()) return  string.Empty ;

            string logicString = null;

            int itemIndex = 0;

            StringBuilder whereItem = new StringBuilder();

            foreach (var item in items)
            {
                logicString = string.Format(" {0} ", item.LogicType.ToString());
 
                if (itemIndex == 0)
                {
                    whereItem.Append(item.ConditionString);
                }
                else
                {
                    whereItem.Append(logicString + item.ConditionString);
                }
                itemIndex++;
            }
            whereItem.Insert(0, "(").Append(")");

            return whereItem.ToString();
        }

        private string GetCondString(Condition group, bool isGroup)
        {
            var itemWhere = GetCondString(group.ConditionItems);

            if (!group.ConditionList.Any()) return itemWhere.ToString();

            var str = new StringBuilder();

            var condList = group.ConditionList.Where(c => !c.IsGroup).ToList();

            var condWhere = new StringBuilder();

            if (condList.Count > 0)
            {
                foreach (var cond in condList)
                {
                    if (!string.IsNullOrWhiteSpace(itemWhere) || condWhere.Length > 0)
                    {
                        condWhere.Append(" " + cond.LogicType.ToString() + " ");
                    }
                    condWhere.Append(GetCondString(cond, true));
                }
                if (condList.Count() > 1)
                {
                    condWhere.Insert(0, "(").Append(")");
                }

                str.Append(itemWhere).Append(condWhere);

                if (isGroup)
                {
                    if (!string.IsNullOrWhiteSpace(itemWhere) && condWhere.Length > 0)
                    {
                        str.Insert(0, "(").Append(")");
                    }
                }
            }
            var groupList = group.ConditionList.Where(c => c.IsGroup).ToList();

            if (groupList.Count > 0)
            {
                var groupWhere = new StringBuilder();

                foreach (var cond in groupList)
                {
                    if (str.Length > 0)
                    {
                        groupWhere.Append(" " + cond.LogicType.ToString() + " ");
                    }
                    groupWhere.Append(GetCondString(cond, true));
                }
                str.Append(groupWhere);

                if (isGroup)
                {
                    if ((!string.IsNullOrWhiteSpace(itemWhere) || condWhere.Length > 0 ) && groupWhere.Length > 0)
                    {
                        str.Insert(0, "(").Append(")");
                    }
                }
            }
            return str.ToString();
        }

        #endregion

        //#region CheckParamNames

        //private bool CheckedParamNames = false;

        //private void CheckParamNames( )
        //{
        //    if (CheckedParamNames) return;

        //    this.CheckParamNames(this.ConditionList);

        //    CheckedParamNames = true;
        //}

        //private void CheckParamNames(List<Condition> conds)
        //{
        //    foreach (var cond in conds)
        //    {
        //        this.CheckParamNames(cond.ConditionItems);

        //        if (cond.ConditionList.Any())
        //        {
        //            this.CheckParamNames(cond.ConditionList);
        //        }
        //    }
        //}
        
        //private void CheckParamNames(List<ConditionItem> items)
        //{
        //    foreach (var item in items)
        //    {
        //        if (OldParamNemes.Where(c => c.IsEquals(item.ParamName)).Count() > 1)
        //        {
        //            item.ParamName = string.Format("{0}_{1}", item.ParamName, ParamNameIndex);
        //        }
        //        ParamNameIndex++;
        //    }
        //}


        //#endregion

        //#region OldParamNemes

        //private List<string> oldParamNemes;

        //private List<string> OldParamNemes
        //{
        //    get
        //    {
        //        if (oldParamNemes == null)
        //        {
        //            oldParamNemes = new List<string>();  
        //        }
        //        if(!CheckedParamNames)
        //        {
        //            oldParamNemes.Clear();
        //            this.BuildParamNames(oldParamNemes, this.ConditionList);
        //        }

        //        return oldParamNemes;
        //    }
        //}

        //private void BuildParamNames(List<string> names, List<Condition> conds)
        //{
        //    if (!conds.Any()) return;

        //    foreach (var cond in conds)
        //    {
        //        var temp = cond.ConditionItems.Select(c=>c.ParamName);
                
        //        if(temp.Any())
        //        {
        //            names.AddRange(temp);
        //        }
        //        if (cond.ConditionList.Any())
        //        {
        //            BuildParamNames(names, cond.ConditionList);
        //        }
        //    }
        //}
 

        //#endregion

        //#region ParamNemes

        //private List<string> paramNemes;
        //public List<string> ParamNemes
        //{
        //    get
        //    {
        //        if (paramNemes == null)
        //        {
        //            this.CheckParamNames();

        //            paramNemes = new List<string>();

        //            this.BuildParamNames(paramNemes, this.ConditionList);
        //        }

        //        if(!CheckedParamNames)
        //        {
        //            this.CheckParamNames();

        //            paramNemes.Clear();

        //            this.BuildParamNames(paramNemes, this.ConditionList);
        //        }

        //        return paramNemes;
        //    }
        //}

        //#endregion

        #region ParamValue

        private Dictionary<string, object> paramList = new Dictionary<string, object>();

        public Dictionary<string, object> ParamList
        {
            get
            {
                this.BuildParamList();

                return paramList;
            }
        }

       
        private void BuildParamList( )
        {
            if (this.ConditionStatus == ConditionStatus.Default)
            {
                paramList.Clear();

                ParamNameIndex = 0;

                this.BuildParamList(paramList, this.ConditionList);

                this.ConditionStatus = ConditionStatus.BuildParam;
            }
        }

        private void BuildParamList(Dictionary<string, object> list, List<Condition> conds)
        {
            if (!conds.Any()) return;

            foreach (var cond in conds)
            {
                this.BuildParamList(list, cond.ConditionItems);

                this.BuildParamList(list, cond.ConditionList);
            }
        }

        private void BuildParamList(Dictionary<string, object> list, List<ConditionItem> items)
        {
            if (!items.Any()) return;

            foreach (var item in items)
            {
                if (item.CompareType == CompareType.IsNull || item.CompareType == CompareType.IsNotNull) continue;
                if (item.ParamValue == null) continue;
 
                if(list.Keys.IsContains(item.FieldName))
                {
                    item.ParamName = string.Format("{0}_{1}", item.FieldName, ParamNameIndex);  
                }
                list.Add(item.ParamName, item.ParamValue);

                ParamNameIndex++;
            }
        }



        #endregion


        #region Remove

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Condition RemoveItem(params string[] fieldNames)
        {
            if (!this.ConditionList.Any()) return this;

            if (fieldNames == null || !fieldNames.Any()) return this;

            foreach (var cond in ConditionList)
            {
                this.RemoveItem(cond.ConditionItems, fieldNames);

                this.RemoveItem(cond.ConditionList, fieldNames);
            }
            this.ConditionStatus = ConditionStatus.Default;

            return this;
        }

        private void RemoveItem(List<Condition> conds, params string[] fieldNames)
        {
            if (!conds.Any()) return;

            foreach (var cond in conds)
            {
                this.RemoveItem(cond.ConditionItems, fieldNames);

                this.RemoveItem(cond.ConditionList, fieldNames);
            }
        }

        private void RemoveItem(List<ConditionItem> items,params string[] fieldNames)
        {
            if (!items.Any()) return;

            int count = items.Count;

            for (int i = 0; i< count; i++ )
            {
                foreach(var fieldName in fieldNames)
                {
                    if (items[i].FieldName.IsEquals(fieldName))
                    {
                        items.Remove(items[i]);
                        i--;
                        count--;
                    }
                }
            }
        }



        public Condition ReplaceItem(string fieldName, object val)
        {
            if (!this.ConditionList.Any()) return this;
           
            foreach (var cond in ConditionList)
            {
                this.ReplaceItem(cond.ConditionItems, fieldName, val);

                this.ReplaceItem(cond.ConditionList,  fieldName, val);
            }

            this.ConditionStatus = ConditionStatus.Default;

            return this;
        }

        private void ReplaceItem(List<Condition> conds, string fieldName, object val)
        {
            if (!conds.Any()) return;

            foreach (var cond in conds)
            {
                this.ReplaceItem(cond.ConditionItems, fieldName,val);

                this.ReplaceItem(cond.ConditionList, fieldName,val);
            }
        }

        private void ReplaceItem(List<ConditionItem> items, string fieldName, object val)
        {
            if (!items.Any()) return;

            foreach(var item in items)
            {
                if(item.FieldName.IsEquals(fieldName))
                {
                    item.FieldValue = val;
                }
            }
        }

        #endregion

        ///public DynamicParameters  

    }


    public enum ConditionStatus
    {
        Default = 0,

        BuildParam = 1,

        BuildWhere = 2,

        //Finish  = 3
    }

    /// <summary>
    /// 分页查询参数
    /// <remarks>SQL Server 2012 以前的版本必须提供 <see cref="OrderBy"/> 的值。</remarks>
    /// </summary>
    public class QueryParams
    {

        public QueryParams()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        /// <summary>
        /// 查询表或者视图名称
        /// </summary>
        public string TableName { get; set; }


        /// <summary>
        /// 当前页码，默认为 1，从 1 开始。
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示条数，默认为10。
        /// </summary>
        public int PageSize { get; set; }


        public int StartIndex
        {
            get {
                return (PageIndex - 1) * PageSize;
            }
        }
        
        /// <summary>
        /// 排序字段和方式（示例：Id DESC, TID DESC） 分页必须提供此属性值
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        //public object WhereParam { get; set; }

        //public List<object> WhereParams { get; set; }


        public Dictionary<string, object> ParamList
        {
            get {
                return this.Condition.ParamList;
            }
        }

        public Condition Condition { get; set; } = new Condition();
 
        /// <summary>
        /// 查询字段名称，用逗号分开，如 ID,Name,Age, 默认为* 
        /// </summary>
        public string FieldNames { get; set; } = "*";


        public string WhereString
        {
            get
            {
                return this.Condition.WhereString;
            }
        }

        /// <summary>
        /// 查询数据量SQL语句
        /// </summary>
        public string CountSqlString
        {
            get
            {
                return string.Format("SELECT COUNT(1) FROM [{0}] WITH(NOLOCK) WHERE {1}", TableName, WhereString).Trim();
            }
        }

        /// <summary>
        /// 是否使用OFFSET FETCH NEXT 方式，适用于 Sql Server 2012 及以上版本，否则设置为 false
        /// </summary>
        private bool UseFetch { get; set; } = true;


        private string listSqlString;
        public string ListSqlString
        {
            get
            {
                if (string.IsNullOrEmpty(listSqlString))
                {
                    var sql = new System.Text.StringBuilder();
                    if (UseFetch)
                    {
                        // OFFSET FETCH NEXT 实现，适用于 Sql Server 2012 及以上版本
                        sql.AppendFormat("{0} ORDER BY {1}", InnerSql, OrderBy);
                        sql.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", StartIndex, PageSize);
                        sql.AppendFormat(" set statistics profile off");
                    }
                    else
                    {
                        // Row_Number 实现，适用于 SQL Server 2012 以前的版本
                        sql.AppendFormat("SELECT {0} FROM (", this.FieldNames);
                        sql.AppendFormat(" SELECT ROW_NUMBER() OVER(ORDER BY {0}) RN,{1} FROM ({2}) T", OrderBy, FieldNames, InnerSql);
                        sql.AppendFormat(" ) T WHERE T.RN BETWEEN {0} AND {1}", StartIndex + 1, PageIndex * PageSize);
                    }
                    listSqlString = sql.ToString().Trim();
                }
                return listSqlString;
            }
        }

        public string InnerSql
        {
            get
            {
                return string.Format("SELECT {0} FROM [{1}] WITH(NOLOCK) WHERE {2}", FieldNames, TableName, WhereString);
            }
        }
    }

    #endregion

}
