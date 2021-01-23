<template>
  <div class="app-container">
    <!--  查询条件  -->
    <div class="area">
      <el-row>
        <el-col :span="2" class="label">姓名 :</el-col>
        <el-col :span="6">
          <el-input v-model="query.name" placeholder="请输入姓名" />
        </el-col>
        <el-col :span="2" class="label">年龄 :</el-col>
        <el-col :span="6">
          <el-input v-model="query.age" placeholder="请输入年龄" />
        </el-col>
        <el-col :span="2" class="label">用户类型 :</el-col>
        <el-col :span="6">
          <el-select v-model="query.type" placeholder="请选择">
            <el-option
              v-for="item in this.init.search.Type"
              :key="item.key"
              :label="item.label"
              :value="item.value"
            />
          </el-select>
        </el-col>
      </el-row>
    </div>
    <!--  按钮区域  -->
    <div style="text-align:right;" class="area">
      <el-button type="primary" size="small" @click="btnQuery">
        <i class="el-icon-upload" />
        查询
      </el-button>
      <el-button type="success" size="small" @click="btnAdd">
        <i class="el-icon-upload" />
        新增
      </el-button>
      <el-button type="warning" size="small" @click="btnEdit">
        <i class="el-icon-upload" />
        修改
      </el-button>
      <el-button type="danger" size="small" @click="btnDelete">
        <i class="el-icon-upload" />
        删除
      </el-button>
      <el-button type="info" size="small" @click="btnDetail">
        <i class="el-icon-upload" />
        查看详情
      </el-button>
      <!-- <el-button type="info" size="small" class="el-button--import" @click="btnImport">
        <i class="el-icon-upload"></i>
        导入
      </el-button>-->

      <el-upload
        style="display:inline-block;margin:0px 10px;"
        action="http://localhost:8016/User/Import"
        multiple
        :limit="1"
      >
        <el-button type="info" size="small" class="el-button--import" @click="btnImport">
          <i class="el-icon-upload" />
          导入
        </el-button>
      </el-upload>

      <el-button type="info" size="small" class="el-button--export" @click="btnExport">
        <i class="el-icon-upload" />
        导出
      </el-button>
      <el-button type="info" size="small" class="el-button--template" @click="btnTemplate">
        <i class="el-icon-upload" />
        下载模板
      </el-button>
    </div>
    <!--  数据表格  -->
    <div class="area">
      <el-table
        :ref="this.init.table.key"
        :key="this.init.table.key"
        :border="true"
        :data="this.data"
        style="width:100%;"
        :fit="true"
        :height="this.init.table.height"
        :stripe="true"
        :empty-text="this.init.table.emptytext"
        @select="this.select"
        @select-all="this.selectall"
        @cell-click="this.cellclick"
        @row-click="this.rowclick"
      >
        <el-table-column type="index" label="序号" align="center" width="80" />
        <el-table-column
          key="Name"
          type="text"
          width="150"
          label="姓名"
          property="Name"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
        <el-table-column
          key="Age"
          type="text"
          width="150"
          label="年龄"
          property="Age"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
        <el-table-column
          key="NickName"
          type="text"
          width="150"
          label="昵称"
          property="NickName"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
        <el-table-column
          key="IdCard"
          type="text"
          width="150"
          label="身份证号"
          property="IdCard"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
        <el-table-column
          key="Type"
          type="text"
          label="用户类型"
          property="Type"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
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
      />
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
      <Edit ref="edit" :options="init.dialog" @refresh="refresh" />
    </el-dialog>
  </div>
</template>

<script>
import request from '@/utils/action.js'
import Edit from '@/views/modify-manage/usermanage2/pages/edit.vue'

export default {
  name: 'List',
  components: {
    Edit: Edit
  },
  data: function() {
    return {
      //  查询绑定参数
      query: {
        name: '',
        age: null,
        type: null,
        pageIndex: 1,
        pageSize: 1,
        sort: ''
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {
          Type: [
            {
              key: 'empty',
              value: '',
              label: '--请选择--'
            },
            {
              key: 'admin',
              value: '1',
              label: '管理员'
            },
            {
              key: 'user',
              value: '2',
              label: '用户'
            }
          ]
        },
        table: {
          height: window.innerHeight - 250,
          key: 'table',
          emptytext: '暂无数据',
          url: 'http://localhost:8016/User/GetList',
          type: 'post'
        },
        dialog: {
          width: '50%',
          showDialog: false,
          title: '',
          type: 'detail',
          id: null
        }
      },
      //    表格数据
      data: [{ Name: '施骏成', Age: '26' }, { Name: '施骏成1', Age: '262' }],
      //    分页控件参数
      pager: {
        size: 10,
        index: 1,
        sizes: [5, 10, 15, 20, 30, 40, 50],
        total: 0
      }
    }
  },
  mounted: function() {
    // this.init.table.height =
    //   window.innerHeight - this.$refs.table.$el.offsetTop - 50;
    //  console.log(this.init.table.height);
    this.getlist()
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
      this.pager.size = size
      this.getlist()
    },
    //  页变化事件
    currentchange: function(value) {
      this.pager.index = value
      this.getlist()
    },
    //  上一页
    prev: function(value) {
      this.pager.index = value
    },
    //  下一页
    next: function(value) {
      this.pager.index = value
    },
    //  获取数据
    getlist: function() {
      var vm = this
      vm.query.pageIndex = vm.pager.index
      vm.query.pageSize = vm.pager.size
      request({
        url: vm.init.table.url,
        type: vm.init.table.type,
        data: vm.query,
        success: function(data) {
          if (data.success) {
            vm.data = data.data
            vm.pager.total = data.count
          }
        }
      })
    },
    //  是否刷新数据
    refresh: function(bRefresh) {
      if (bRefresh) {
        this.getlist()
      }
      this.init.dialog.showDialog = false
      this.init.dialog.id = null
      this.init.dialog.title = ''
    },
    //  窗体打开
    opened: function() {
      this.$refs.edit.initData()
    },

    //  查询按钮
    btnQuery: function() {
      this.getlist()
    },
    //  新增按钮
    btnAdd: function() {
      this.init.dialog.title = '新增用户'
      this.init.dialog.showDialog = true
    },
    //  编辑按钮
    btnEdit: function() {
      const selections = this.$refs.table.selection
      if (selections.length < 1) {
        this.$alert('请选择一条数据', '信息提示', {
          confirmButtonText: '确定'
        })
      } else if (selections.length > 1) {
        this.$alert('有且只能选择一条数据', '信息提示', {
          confirmButtonText: '确定'
        })
      } else {
        this.init.dialog.title = '编辑用户'
        this.init.dialog.id = selections[0].Id
        this.init.dialog.showDialog = true
        this.init.dialog.type = 'edit'
      }
    },
    //  删除按钮
    btnDelete: function() {
      var ids = this.$refs.table.selection.map(x => x.Id)
      request({
        url: 'http://localhost:8016/User/DeleteUser',
        type: 'post',
        data: {
          ids
        },
        success: data => {
          if (data.success) {
            this.getlist()
          }
        }
      })
    },
    //  查看详情按钮
    btnDetail: function() {
      const selections = this.$refs.table.selection
      if (selections.length < 1) {
        this.$alert('请选择一条数据', '信息提示', {
          confirmButtonText: '确定'
        })
      } else if (selections.length > 1) {
        this.$alert('有且只能选择一条数据', '信息提示', {
          confirmButtonText: '确定'
        })
      } else {
        this.init.dialog.title = '编辑用户'
        this.init.dialog.id = selections[0].Id
        this.init.dialog.showDialog = true
        this.init.dialog.type = 'detail'
      }
    },
    //  导出按钮
    btnExport: function() {
      var vm = this
      request({
        url: 'http://localhost:8016/User/Export',
        type: 'post',
        data: vm.query,
        success: function(data) {
          if (data.success) {
            import('@/vendor/Export2Excel').then(excel => {
              const tHeader = ['姓名', '年龄', '昵称', '身份证号', '数据类型']
              const filterVal = ['Name', 'Age', 'NickName', 'IdCard', 'Type']
              const list = data.data
              list.map(x => {
                x.Type = x.Type == 1 ? '管理员' : '普通用户'
                return { x }
              })
              const exportdata = vm.formatJson(filterVal, list)
              excel.export_json_to_excel({
                header: tHeader,
                data: exportdata,
                filename: '111'
              })
            })
          }
        }
      })
    },
    //  导入按钮
    btnImport: function() {},
    //  下载模板
    btnTemplate: function() {
      console.log(window.location.host)
      document.location.href = 'http://' + window.location.host + '/1.txt'
    },

    // json转换
    formatJson(filterVal, jsonData) {
      return jsonData.map(v =>
        filterVal.map(j => {
          if (j === 'timestamp') {
            return parseTime(v[j])
          } else {
            return v[j]
          }
        })
      )
    }
  }
}
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
