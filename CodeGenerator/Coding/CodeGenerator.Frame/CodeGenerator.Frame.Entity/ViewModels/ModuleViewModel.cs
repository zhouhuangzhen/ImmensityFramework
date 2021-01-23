using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.Entity.ViewModels
{
    /// <summary>
    /// 视图模型--结构模块表
    /// </summary>
    public class ModuleViewModel
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
        /// 结构所属框架Id
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
        /// 所属结构Id
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

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
    }
}
