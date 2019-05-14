using TestCore.Common.Ioc;

namespace TestCore.Common.Captch
{
    public interface IVerifyCode: ISingletonDependency
    {
        byte[] GetCaptch();
    }
}
