<template>
  <div class="app-container">
    <!--  查询条件  -->
    <div class="area">
      <el-form ref="queryForm" :model="query" label-width="100px">
        <el-row>
          <el-col :span="6">
            <el-form-item label="驾驶员姓名">
              <el-input
                v-model="query.EName"
                placeholder="请输入驾驶员姓名"
                style="width:210px;"
                size="small"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="驾驶员工号">
              <el-input
                v-model="query.EmpNo"
                placeholder="请输入驾驶员工号"
                style="width:210px;"
                size="small"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="所属部门">
              <el-input
                v-model="query.Department"
                placeholder="请输入所属部门"
                style="width:210px;"
                size="small"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="日期">
              <el-date-picker
                v-model="query.SearchMonth"
                size="small"
                type="month"
                format="yyyyM"
                value-format="yyyyM"
                placeholder="请选择日期"
                style="width:210px;"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </div>
    <!--  按钮区域  -->
    <div style="text-align:right;" class="area">
      <el-button type="primary" size="small" @click="btnQuery">
        <i class="el-icon-search" />
        查询
      </el-button>
      <el-button type="info" size="small" class="el-button--export" @click="btnExport">
        <i class="el-icon-download" />
        导出
      </el-button>
    </div>
    <!--  数据表格  -->
    <div class="area">
      <el-table
        :ref="init.table.key"
        :key="init.table.key"
        v-loading="loading"
        :border="true"
        :data="data"
        style="width:100%;"
        :fit="true"
        :height="init.table.height"
        :stripe="true"
        :empty-text="init.table.emptytext"
      >
        <el-table-column :label="$t('detail.index')" align="center" width="70">
          <template scope="scope"><span>{{ scope.$index+(query.CurrentPage) * query.PageSize + 1 }} </span></template>
        </el-table-column>
        <el-table-column width="150" label="所属部门" property="Department" align="center" />
        <el-table-column width="100" label="月份" property="SearchMonth" align="center" />
        <el-table-column width="150" label="工号" property="EmpNo" align="center" />
        <el-table-column width="100" label="姓名" property="EName" align="center" />

        <el-table-column label="日期">
          <el-table-column width="50" label="1" property="WorkDate1" align="center" />
          <el-table-column width="50" label="2" property="WorkDate2" align="center" />
          <el-table-column width="50" label="3" property="WorkDate3" align="center" />
          <el-table-column width="50" label="4" property="WorkDate4" align="center" />
          <el-table-column width="50" label="5" property="WorkDate5" align="center" />
          <el-table-column width="50" label="6" property="WorkDate6" align="center" />
          <el-table-column width="50" label="7" property="WorkDate7" align="center" />
          <el-table-column width="50" label="8" property="WorkDate8" align="center" />
          <el-table-column width="50" label="9" property="WorkDate9" align="center" />
          <el-table-column width="50" label="10" property="WorkDate10" align="center" />
          <el-table-column width="50" label="11" property="WorkDate11" align="center" />
          <el-table-column width="50" label="12" property="WorkDate12" align="center" />
          <el-table-column width="50" label="13" property="WorkDate13" align="center" />
          <el-table-column width="50" label="14" property="WorkDate14" align="center" />
          <el-table-column width="50" label="15" property="WorkDate15" align="center" />
          <el-table-column width="50" label="16" property="WorkDate16" align="center" />
          <el-table-column width="50" label="17" property="WorkDate17" align="center" />
          <el-table-column width="50" label="18" property="WorkDate18" align="center" />
          <el-table-column width="50" label="19" property="WorkDate19" align="center" />
          <el-table-column width="50" label="20" property="WorkDate20" align="center" />
          <el-table-column width="50" label="21" property="WorkDate21" align="center" />
          <el-table-column width="50" label="22" property="WorkDate22" align="center" />
          <el-table-column width="50" label="23" property="WorkDate23" align="center" />
          <el-table-column width="50" label="24" property="WorkDate24" align="center" />
          <el-table-column width="50" label="25" property="WorkDate25" align="center" />
          <el-table-column width="50" label="26" property="WorkDate26" align="center" />
          <el-table-column width="50" label="27" property="WorkDate27" align="center" />
          <el-table-column width="50" label="28" property="WorkDate28" align="center" />
          <el-table-column width="50" label="29" property="WorkDate29" align="center" />
          <el-table-column width="50" label="30" property="WorkDate30" align="center" />
          <el-table-column width="50" label="31" property="WorkDate31" align="center" />
        </el-table-column>

        <el-table-column label="累计上班天数">
          <el-table-column width="50" label="早" property="ZAO" align="center" />
          <el-table-column width="50" label="中" property="ZHONG" align="center" />
          <el-table-column width="50" label="二" property="ER" align="center" />
          <el-table-column width="50" label="吃" property="CHI" align="center" />
          <el-table-column width="50" label="夜" property="YE" align="center" />
          <el-table-column width="50" label="机" property="JIDONG" align="center" />
          <el-table-column width="50" label="休" property="XIU" align="center" />
        </el-table-column>

        <el-table-column width="150" label="合计" property="DataTotal" align="center" />
        <el-table-column width="150" label="合计工作小时" property="DataHourTotal" align="center" />
      </el-table>

      <pagination
        v-show="total>0"
        :total="total"
        :page.sync="pager.page"
        :limit.sync="pager.limit"
        @pagination="getlist"
      />
    </div>
  </div>
</template>

<script>
import request from '@/utils/action.js'
import communicate from '@/utils/communicate.js'
import excelhelper from '@/utils/excelhelper.js'
import convert from '@/utils/convert.js'

export default {
  name: 'List',
  components: {},
  data: function() {
    return {
      // 遮罩层
      loading: false,
      // 合计数量
      total: 0,
      //  查询绑定参数
      query: {
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'PlanTime',
        OrderMethod: 'asc',
        Department: null,
        SearchMonth: null,
        EName: null,
        EmpNo: null
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {},
        table: {
          height: window.innerHeight - 308,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'AttendanceDataReport',
          listnode: 'AttendanceData'
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
      data: [],
      pager: {
        page: 1,
        limit: 10
      }
    }
  },
  mounted: function() {
    // this.init.table.height =
    //   window.innerHeight - this.$refs.table.$el.offsetTop - 50;
    //  this.getlist()
  },
  methods: {
    //  列format事件
    formatter: function(row, column, cellValue, index) {},
    //  页数量变化事件
    sizechange: function(size) {
      this.pager.size = size
      this.getlist()
    },
    //  获取数据
    getlist: function(pager) {
      if (this.$refs[this.init.table.key]) this.$refs[this.init.table.key].bodyWrapper.scrollTop = 0
      var vm = this
      vm.loading = true
      //  接口对象处理
      if (!vm.query.EName) {
        vm.$delete(vm.query, 'EName')
      }
      if (!vm.query.EmpNo) {
        vm.$delete(vm.query, 'EmpNo')
      }
      if (!vm.query.Department) {
        vm.$delete(vm.query, 'Department')
      }
      if (!vm.query.SearchMonth) {
        vm.$delete(vm.query, 'SearchMonth')
      }
      if (pager === undefined) {
        vm.query.CurrentPage = 0
      } else {
        vm.query.CurrentPage = pager.page - 1
        vm.query.PageSize = pager.limit
        vm.pager = pager
      }

      communicate
        .TransformData(vm.init.table.datatype, vm.query)
        .then(response => {
          vm.data = []
          if (response[vm.init.table.listnode]) {
            if (Array.isArray(response[vm.init.table.listnode])) {
              vm.data = response[vm.init.table.listnode]
            } else {
              vm.data.push(response[vm.init.table.listnode])
            }
            vm.total = response._Total
            vm.loading = false
          } else {
            vm.data = []
            vm.total = 0
            vm.loading = false
          }
        }).catch(error => {
          console.log(error)
          vm.loading = false
          this.msgError(this.errorMessage['Commmons']['1'])
        })
      // communicate.TransformData(vm.init.table.datatype, vm.query).then(
      //   response => {
      //     vm.data = response[vm.init.table.listnode]
      //     vm.pager.total = 100
      //   }
      // )
    },

    //  查询按钮
    btnQuery: function() {
      debugger
      this.helpers.info.userinfo.getLines().then(data => {
        console.log(data)
      })
      this.pager.page = 1
      this.getlist()
    },
    //  导出按钮
    btnExport: function() {
      var vm = this
      //  接口对象处理
      if (!vm.query.EName) {
        vm.$delete(vm.query, 'EName')
      }
      if (!vm.query.EmpNo) {
        vm.$delete(vm.query, 'EmpNo')
      }
      if (!vm.query.Department) {
        vm.$delete(vm.query, 'Department')
      }
      if (!vm.query.SearchMonth) {
        vm.$delete(vm.query, 'SearchMonth')
      }

      const exportquery = {}
      Object.assign(exportquery, vm.query)
      exportquery.CurrentPage = 0
      exportquery.PageSize = 999999

      const myload = vm.inloading('导出中，请稍后......')
      communicate
        .TransformData(vm.init.table.datatype, exportquery)
        .then(response => {
          if (response[vm.init.table.listnode]) {
            const firstrow = ['所属部门', '月份', '工号', '姓名', '日期', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '累计上班天数', '', '', '', '', '', '', '合计', '合计工作小时']
            const lastrow = ['', '', '', '', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', '29', '30', '31', '早', '中', '二', '吃', '夜', '机', '休', '', '']
            const merges = ['A1:A2', 'B1:B2', 'C1:C2', 'D1:D2', 'E1:AI1', 'AJ1:AP1', 'AQ1:AQ2', 'AR1:AR2']
            const columns = []
            this.$refs.table.columns.map(item => {
              if (item.property && (item.type == 'default' || item.type == 'text') && columns.indexOf(item.property) < 0) {
                columns.push(item.property)
              }
            })
            const data = convert.object2array(response[vm.init.table.listnode])
            excelhelper.export('考勤表', lastrow, columns, data, [firstrow], merges)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
        }).catch(error => {
          console.log(error)
          vm.unloading(myload)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
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
.el-select .el-input {
  width: 130px;
}
</style>
