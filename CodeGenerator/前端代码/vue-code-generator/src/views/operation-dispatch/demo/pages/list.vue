<template>
  <div class="app-container">
    <!--  查询条件  -->
    <el-row>
      <el-col :span="4" class="label" v-text="$t('search.drivername')" />
      <el-col :span="8">
        <el-select v-model="query.type" placeholder="请选择">
          <el-option key="empty" label="--请选择--" value />
          <el-option key="corp" label="分公司" value="1" />
          <el-option key="line" label="线路" value="2" />
          <el-option key="shift" label="班次" value="3" />
        </el-select>
      </el-col>
      <el-col :span="4" class="label">分公司 :</el-col>
      <el-col :span="8">
        <el-select v-model="query.corp" :filterable="true" placeholder="请选择">
          <el-option
            v-for="item in init.search.corp"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="4" class="label">线路 :</el-col>
      <el-col :span="8">
        <el-select v-model="query.line" :filterable="true" placeholder="请选择">
          <el-option
            v-for="item in init.search.line"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-col>
      <el-col :span="4" class="label">班次 :</el-col>
      <el-col :span="8">
        <el-select v-model="query.shift" :filterable="true" placeholder="请选择">
          <el-option
            v-for="item in init.search.shift"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-col>
    </el-row>
    <!--  按钮区域  -->
    <div style="text-align:right;" class="area">
      <el-button type="primary" size="small" @click="btnQuery">
        <i class="el-icon-upload" />
        查询
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
        <el-table-column type="index" label="序号" align="center" width="80" />
        <el-table-column
          key="Type"
          type="text"
          width="150"
          label="查询类别"
          property="Type"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
        <el-table-column
          key="WarnTime"
          type="text"
          width="150"
          label="日期"
          property="WarnTime"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
        <el-table-column
          key="No"
          type="text"
          label="计划车辆数"
          property="No"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
        <el-table-column
          key="CardNo"
          type="text"
          label="实际车辆数"
          property="CardNo"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
        <el-table-column
          key="DealStatus"
          type="text"
          label="计划完成数"
          property="DealStatus"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
        <el-table-column
          key="DealStatus"
          type="text"
          label="实际完成数"
          property="DealStatus"
          sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        />
        <el-table-column
          key="DealStatus"
          type="text"
          label="完成比例"
          property="DealStatus"
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
import request from '@/utils/request.js'
import Edit from '@/views/operation-dispatch/attendancelist/pages/edit.vue'

export default {
  name: 'List',
  components: {
    Edit: Edit
  },
  data: function() {
    return {
      //  查询绑定参数
      query: {
        type: null,
        corp: null,
        line: null,
        shift: null,
        pageIndex: 1,
        pageSize: 1,
        sort: ''
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {
          corp: [],
          line: [],
          shift: []
        },
        table: {
          height: window.innerHeight - 286,
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
      var url = vm.init.table.url
      if (!url || url.length <= 0) {
        return
      }
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
      this.init.dialog.Id = null
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
        this.init.dialog.Id = selections[0].Id
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
        this.init.dialog.Id = selections[0].Id
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
                x.Type = x.Type === 1 ? '管理员' : '普通用户'
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
