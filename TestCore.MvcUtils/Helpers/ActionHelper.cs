using TestCore.Common.Helper;
using TestCore.Domain.Enums;

namespace TestCore.MvcUtils
{
    public class ActionHelper
    {
        public static RightTypeEnum GetRightType(string action)
        {
            if (string.IsNullOrEmpty(action) || view.IsContainsSubString(action))
            {
                return RightTypeEnum.view;
            }
            else if (add.IsContainsSubString(action))
            {
                return RightTypeEnum.create;
            }
            else if (audit.IsContainsSubString(action))
            {
                return RightTypeEnum.audit;
            }
            else if (edit.IsContainsSubString(action))
            {
                return RightTypeEnum.edit;
            }
            else if (delete.IsContainsSubString(action))
            {
                return RightTypeEnum.delete;
            }
            else if (payment.IsContainsSubString(action))
            {
                return RightTypeEnum.payment;
            }
            else
            {
                return RightTypeEnum.view;
            }
        }

        private static string[] view = new string[] { "index", "details", "get", "list", "main", "search", "find" };

        private static string[] login = new string[] { "login", };
        private static string[] logout = new string[] { "logout", };
        private static string[] updatepwd = new string[] { "updatepwd" };

        private static string[] add = new string[] { "create", "add", "insert" };
        private static string[] edit = new string[] { "edit", "change" };
        private static string[] delete = new string[] { "delete", "remove", "clear", "del" };
        private static string[] audit = new string[] { "audit", "auth", "status" };
        private static string[] payment = new string[] { "payment", };

        private static string[] apply = new string[] { "apply", };

        public static ActionTypeEnum GetActionType(string action)
        {
            if ( login.IsContainsSubString(action))
            {
                return ActionTypeEnum.Login;
            }
            else if (add.IsContainsSubString(action))
            {
                return ActionTypeEnum.Create;
            }
            else if (edit.IsContainsSubString(action))
            {
                return ActionTypeEnum.Edit;
            }
            else if (audit.IsContainsSubString(action))
            {
                return ActionTypeEnum.Audit;
            }
            else if (delete.IsContainsSubString(action))
            {
                return ActionTypeEnum.Delete;
            }
            else if (payment.IsContainsSubString(action))
            {
                return ActionTypeEnum.Payment;
            }
            else if (apply.IsContainsSubString(action))
            {
                return ActionTypeEnum.Apply;
            }
            else if (logout.IsContainsSubString(action))
            {
                return ActionTypeEnum.Logout;
            }
            else if (view.IsContainsSubString(action))
            {
                return ActionTypeEnum.View;
            }
            else if(updatepwd.IsContains(action))
            {
                return ActionTypeEnum.UpdatePwd;
            }

            return ActionTypeEnum.None;
        }

    }
}
