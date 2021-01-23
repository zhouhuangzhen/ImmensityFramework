<template>
  <div class="app-container">
    <!--  查询条件  -->
    <!--  按钮区域  -->
    <div style="text-align:right;" class="area">
      <el-button type="success" size="small" @click="btnAdd">
        <i class="el-icon-upload"></i>
        新增
      </el-button>
      <el-button type="warning" size="small" @click="btnEdit">
        <i class="el-icon-upload"></i>
        修改
      </el-button>
      <el-button type="danger" size="small" @click="btnDelete">
        <i class="el-icon-upload"></i>
        删除
      </el-button>
    </div>
    <!--  数据表格  -->
    <div class="area">
      <el-table
        :border="true"
        :data="this.data"
        style="width:100%;"
        :ref="this.init.table.key"
        :key="this.init.table.key"
        :fit="true"
        :height="this.init.table.height"
        :stripe="true"
        :empty-text="this.init.table.emptytext"
        @select="this.select"
        @select-all="this.selectall"
        @cell-click="this.cellclick"
        @row-click="this.rowclick"
      >
        <el-table-column key="Id" type="selection" label="" property="Id" align="center"></el-table-column>
        <el-table-column type="index" label="序号" align="center" width="80"></el-table-column>
        <el-table-column
          key="Content"
          type="text"
          width="150"
          label="内容"
          property="Content"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        ></el-table-column>
        <el-table-column
          key="Type"
          type="text"
          width="150"
          label="类别"
          property="Type"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        ></el-table-column>
        <el-table-column
          key="Order"
          type="text"
          width="150"
          label="顺序"
          property="Order"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        ></el-table-column>
        <el-table-column
          key="ModifyTime"
          type="text"
          width="150"
          label="维护时间"
          property="ModifyTime"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        ></el-table-column>
        <el-table-column
          key="ModifyUser"
          type="text"
          width="150"
          label="维护人员"
          property="ModifyUser"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        ></el-table-column>
        <el-table-column
          key="Remark"
          type="text"
          label="备注"
          property="Remark"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        ></el-table-column>
      </el-table>

      <el-pagination
        layout="prev, pager, next,sizes,total,jumper"
        :background="true"
        :page-size="this.pager.size"
        :current-page="this.pager.index"
        :page-sizes="this.pager.sizes"
        :total="this.pager.total"
        @size-change="this.sizechange"
        @current-change="this.currentchange"
        @prev-click="this.prev"
        @next-click="this.next"
      ></el-pagination>
    </div>
    <!--  编辑页  -->

    <el-dialog
      :visible.sync="init.dialog.showDialog"
      :title="init.dialog.title"
      :width="init.dialog.width"
      :destroy-on-close="true"
      :show-close="true"
      :close-on-click-modal="false"
      @opened="opened"
    >
      <Edit :options="init.dialog" @refresh="refresh" ref="edit"></Edit>
    </el-dialog>
  </div>
</template>

<script>
import request from "@/utils/action.js";
import Edit from "@/views/system-manage/checkcontent/pages/edit.vue";

export default {
  name: "List",
  data: function() {
    return {
      //  查询绑定参数
      query: {
        code: "",
        name: null,
        type: null,
        pageIndex: 1,
        pageSize: 1,
        sort: ""
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {
          Type: [
            {
              key: "empty",
              value: "",
              label: "--请选择--"
            },
            {
              key: "system",
              value: "1",
              label: "系统参数"
            },
            {
              key: "corperation",
              value: "2",
              label: "企业参数"
            }
          ]
        },
        table: {
          height: window.innerHeight - 214,
          key: "table",
          emptytext: "暂无数据",
          url: "http://localhost:8016/User/GetList",
          type: "post"
        },
        dialog: {
          width: "50%",
          showDialog: false,
          title: "",
          type: "detail",
          Id: null
        }
      },
      //    表格数据
      data: [],
      //    分页控件参数
      pager: {
        size: 10,
        index: 1,
        sizes: [5, 10, 15, 20, 30, 40, 50],
        total: 0
      }
    };
  },
  components: {
    Edit: Edit
  },
  mounted: function() {
    // this.init.table.height =
    //   window.innerHeight - this.$refs.table.$el.offsetTop - 50;
    //  console.log(this.init.table.height);
    this.getlist();
  },
  methods: {
    //  选中事件
    select: function(selection, row) {},
    //  全选事件
    selectall: function(selection) {},
    //  单元格点击事件
    cellclick: function(row, column, cell, event) {},
    //  行点击事件
    rowclick: function(row, column, event) {},
    //  列format事件
    formatter: function(row, column, cellValue, index) {},
    //  页数量变化事件
    sizechange: function(size) {
      this.pager.size = size;
      this.getlist();
    },
    //  页变化事件
    currentchange: function(value) {
      this.pager.index = value;
      this.getlist();
    },
    //  上一页
    prev: function(value) {
      this.pager.index = value;
    },
    //  下一页
    next: function(value) {
      this.pager.index = value;
    },
    //  获取数据
    getlist: function() {
      var vm = this;
      var url = vm.init.table.url;
      if (!url || url.length <= 0) {
        return;
      }
      vm.query.pageIndex = vm.pager.index;
      vm.query.pageSize = vm.pager.size;
      request({
        url: vm.init.table.url,
        type: vm.init.table.type,
        data: vm.query,
        success: function(data) {
          if (data.success) {
            vm.data = data.data;
            vm.pager.total = data.count;
          }
        }
      });
    },
    //  是否刷新数据
    refresh: function(bRefresh) {
      if (bRefresh) {
        this.getlist();
      }
      this.init.dialog.showDialog = false;
      this.init.dialog.Id = null;
      this.init.dialog.title = "";
    },
    //  窗体打开
    opened: function() {
      this.$refs.edit.initData();
    },

    //  查询按钮
    btnQuery: function() {
      this.getlist();
    },
    //  新增按钮
    btnAdd: function() {
      this.init.dialog.title = "新增用户";
      this.init.dialog.showDialog = true;
    },
    //  编辑按钮
    btnEdit: function() {
      let selections = this.$refs.table.selection;
      if (selections.length < 1) {
        this.$alert("请选择一条数据", "信息提示", {
          confirmButtonText: "确定"
        });
      } else if (selections.length > 1) {
        this.$alert("有且只能选择一条数据", "信息提示", {
          confirmButtonText: "确定"
        });
      } else {
        this.init.dialog.title = "编辑用户";
        this.init.dialog.Id = selections[0].Id;
        this.init.dialog.showDialog = true;
        this.init.dialog.type = "edit";
      }
    },
    //  删除按钮
    btnDelete: function() {
      var ids = this.$refs.table.selection.map(x => x.Id);
      request({
        url: "http://localhost:8016/User/DeleteUser",
        type: "post",
        data: {
          ids
        },
        success: data => {
          if (data.success) {
            this.getlist();
          }
        }
      });
    },
    //  查看详情按钮
    btnDetail: function() {
      let selections = this.$refs.table.selection;
      if (selections.length < 1) {
        this.$alert("请选择一条数据", "信息提示", {
          confirmButtonText: "确定"
        });
      } else if (selections.length > 1) {
        this.$alert("有且只能选择一条数据", "信息提示", {
          confirmButtonText: "确定"
        });
      } else {
        this.init.dialog.title = "编辑用户";
        this.init.dialog.Id = selections[0].Id;
        this.init.dialog.showDialog = true;
        this.init.dialog.type = "detail";
      }
    },
    //  导出按钮
    btnExport: function() {
      var vm = this;
      request({
        url: "http://localhost:8016/User/Export",
        type: "post",
        data: vm.query,
        success: function(data) {
          if (data.success) {
            import("@/vendor/Export2Excel").then(excel => {
              const tHeader = ["姓名", "年龄", "昵称", "身份证号", "数据类型"];
              const filterVal = ["Name", "Age", "NickName", "IdCard", "Type"];
              const list = data.data;
              list.map(x => {
                x.Type = x.Type == 1 ? "管理员" : "普通用户";
                return { x };
              });
              const exportdata = vm.formatJson(filterVal, list);
              excel.export_json_to_excel({
                header: tHeader,
                data: exportdata,
                filename: "111"
              });
            });
          }
        }
      });
    },
    //  导入按钮
    btnImport: function() {},
    //  下载模板
    btnTemplate: function() {
      console.log(window.location.host);
      document.location.href = "http://" + window.location.host + "/1.txt";
    },

    // json转换
    formatJson(filterVal, jsonData) {
      return jsonData.map(v =>
        filterVal.map(j => {
          if (j === "timestamp") {
            return parseTime(v[j]);
          } else {
            return v[j];
          }
        })
      );
    }
  }
};
</script>

<style scoped>
.area {
  margin: 5px auto;
}
.label {
  text-align: center;
  line-height: 36px;
  font-size: 14px;
}
</style>