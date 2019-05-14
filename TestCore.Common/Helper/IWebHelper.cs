using Microsoft.AspNetCore.Http;
using TestCore.Common.Ioc;

namespace TestCore.Common.Helper
{
    /// <summary>
    /// ���ֲ��һ��Web�����ӿ�
    /// </summary>
    public partial interface IWebHelper : ISingletonDependency
    {
        /// <summary>
        /// ����������ȡURL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        string GetUrlReferrer();

        /// <summary>
        /// ����HTTP�����Ļ�ȡIP��ַ
        /// </summary>
        /// <returns>IP��ַ�ַ���</returns>
        string GetCurrentIpAddress();

        /// <summary>
        /// ��ȡһ��ֵ����ֵָʾ��ǰ�����Ƿ��ѱ�����
        /// </summary>
        /// <returns>������ѱ���������Ϊtrue������Ϊfalse</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// �õ��Ĵ���������λ�� 
        /// </summary>
        /// <param name="useSsl">�Ƿ�ʹ���˰�ȫSSL����</param>
        /// <returns>�洢����λ��</returns>
        string GetStoreHost(bool useSsl);

        /// <summary>
        /// ����������Դ�����治��Ҫ����ĵ�����Դ֮һ���򷵻�TRUE
        /// </summary>
        /// <returns>���������Ծ�̬��Դ�ļ�����Ϊtrue</returns>
        bool IsStaticResource();

        /// <summary>
        /// �޸������ѯ�ַ���
        /// </summary>
        /// <param name="url">Ҫ�޸ĵ�URL</param>
        /// <param name="queryStringModification">�޸Ĳ�ѯ�ַ���</param>
        /// <param name="anchor">Anchor</param>
        /// <returns>��URL</returns>
        string ModifyQueryString(string url, string queryStringModification, string anchor);

        /// <summary>
        /// ��URL��ɾ����ѯ�ַ���
        /// </summary>
        /// <param name="url">Ҫ�޸ĵ�URL</param>
        /// <param name="queryString">�Ƴ���ѯ�ַ���</param>
        /// <returns>û�д��ݲ�ѯ�ַ�������URL</returns>
        string RemoveQueryString(string url, string queryString);

        /// <summary>
        /// �����ƻ�ȡ��ѯ�ַ���ֵ
        /// </summary>
        /// <typeparam name="T">����ֵ����</typeparam>
        /// <param name="name">��ѯ������</param>
        /// <returns>��ѯ�ַ���ֵ</returns>
        T QueryString<T>(string name);

        /// <summary>
        /// ��ȡһ��ֵ����ֵָʾ�Ƿ񽫿ͻ����ض�����λ��
        /// </summary>
        bool IsRequestBeingRedirected { get; }

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ�ͻ����Ƿ�ʹ�������ض�����λ��
        /// </summary>
        bool IsPostBeingDone { get; set; }

        /// <summary>
        /// ��ȡ��ǰHTTP����Э��
        /// </summary>
        string CurrentRequestProtocol { get; }

        /// <summary>
        /// ��ȡָ����HTTP����URI�Ƿ����ñ�������
        /// </summary>
        /// <param name="req">HTTP����</param>
        /// <returns>���HTTP����URI���õ�������������ΪTrue</returns>
        bool IsLocalRequest(HttpRequest req);

        /// <summary>
        /// ��ȡ�����ԭʼ·����������ѯ
        /// </summary>
        /// <param name="request">HTTP����</param>
        /// <returns>ԭʼURL</returns>
        string GetRawUrl(HttpRequest request);

        /// <summary>
        /// �ж��Ƿ�Ajax����
        /// </summary>
        /// <returns></returns>
        bool IsAjax();

        #region Session Methods

        /// <summary>
        /// дSession
        /// </summary>
        /// <typeparam name="T">Session��ֵ������</typeparam>
        /// <param name="key">Session�ļ���</param>
        /// <param name="value">Session�ļ�ֵ</param>
        void WriteSession<T>(string key, T value);

        /// <summary>
        /// дSession
        /// </summary>
        /// <param name="key">Session�ļ���</param>
        /// <param name="value">Session�ļ�ֵ</param>
        void WriteSession(string key, string value);

        /// <summary>
        /// ��ȡSession��ֵ
        /// </summary>
        /// <typeparam name="T">ת��������</typeparam>
        /// <param name="key">Session�ļ���</param>
        /// <returns>��������Tʵ��</returns>
        T GetSession<T>(string key);

        /// <summary>
        /// ��ȡSession��ֵ
        /// </summary>
        /// <param name="key">Session�ļ���</param>   
        string GetStrSession(string key);

        /// <summary>
        /// ɾ��ָ��Session
        /// </summary>
        /// <param name="key">Session�ļ���</param>
        void RemoveSession(string key);

        #endregion

    }
}
