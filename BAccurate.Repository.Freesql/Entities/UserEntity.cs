using BAccurate.Enums;

namespace BAccurate.Repository.Freesql.Entities
{
    public class UserEntity:BaseEntity<int>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户帐号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Slat
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// 帐户类型
        /// </summary>
        public AccountType AccountType { get; set; } = AccountType.User;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 扩展配置
        /// </summary>
        public string Config { get; set; }
 
    }
}
