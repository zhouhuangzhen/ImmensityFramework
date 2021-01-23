using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.Entity.ViewModels
{
    /// <summary>
    /// 视图模型--任务生成表
    /// </summary>
    public class TaskJobViewModel
    {
        /// <summary>
        /// 数据唯一标识
        /// </summary>
        public string Oid { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public int Enable { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 数据库连接配置
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 数据库类型(DatabaseType : 1:Oracle , 2:MySQL , 3:Oracle , 4:SQLite , 5:Postgre)
        /// </summary>
        public int SystemType { get; set; }

        /// <summary>
        /// 解决方案名称
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// 数据库表Schema
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 框架Id
        /// </summary>
        public string FrameId { get; set; }

        /// <summary>
        /// 框架编码
        /// </summary>
        public string FrameCode { get; set; }

        /// <summary>
        /// 框架名称
        /// </summary>
        public string FrameName { get; set; }

        /// <summary>
        /// 结构Id
        /// </summary>
        public string StructId { get; set; }

        /// <summary>
        /// 结构编码
        /// </summary>
        public string StructCode { get; set; }

        /// <summary>
        /// 结构名称
        /// </summary>
        public string StructName { get; set; }

        /// <summary>
        /// 模块Id
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Dll名称
        /// </summary>
        public string Dll { get; set; }

        /// <summary>
        /// Dll执行方法
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// 保存Dll的文件Id
        /// </summary>
        public string DllId { get; set; }

        /// <summary>
        /// Dll版本号
        /// </summary>
        public string DllVersion { get; set; }


    }
}
