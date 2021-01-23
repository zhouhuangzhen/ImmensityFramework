<template>
  <div class="app-container">
    <!--  查询条件  -->
    <el-form ref="queryForm" :model="listQuery" :inline="true">
      <el-form-item label="日期">
        <el-date-picker v-model="listQuery.Date" type="date" placeholder="请选择日期" :picker-options="pickerOptions" value-format="yyyy-MM-dd" />
      </el-form-item>
      <el-form-item label="线路番号">
        <el-select v-model="listQuery.LName" style="width:210px;">
          <el-option
            v-for="item in init.search.lineNumber"
            :key="item.key"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="班次">
        <el-input v-model="listQuery.LineNO" placeholder="请输入班次" style="width:200px;" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" icon="el-icon-search" size="mini" @click="btnlistQuery">搜索</el-button>
        <el-button type="info" size="small" class="el-button--export" @click="btnExport">
          <i class="el-icon-download" />
          导出
        </el-button>
      </el-form-item>
    </el-form>
    <!--  图标  -->
    <div class="area">
      <el-tabs v-model="listQuery.SortMethod" type="card" @tab-click="handleClick">
        <el-tab-pane label="分公司" name="CompName" />
        <el-tab-pane label="线路" name="LName" />
        <el-tab-pane label="班次" name="LineNO" />
      </el-tabs>
      <el-table
        :ref="init.table.key"
        :key="init.table.key"
        v-loading="loading"
        :border="true"
        :data="data"
        style="width:100%;"
        :height="init.table.height"
        :fit="true"
        :stripe="true"
        :empty-text="init.table.emptytext"
      >
        <el-table-column :label="$t('detail.index')" align="center" width="70">
          <template scope="scope"><span>{{ scope.$index+(listQuery.CurrentPage - 1) * listQuery.PageSize + 1 }} </span></template>
        </el-table-column>
        <el-table-column
          prop="Date"
          width="180"
          :label="$t('detail.date')"
          :resizable="true"
          align="center"
        />
        <el-table-column
          v-if="listQuery.SortMethod =='CompName'"
          prop="CompName"
          :label="$t('detail.company')"
          :resizable="true"
          align="center"
        />
        <el-table-column v-if="listQuery.SortMethod =='LName'" prop="LName" label="线路" :resizable="true" align="center" />
        <el-table-column v-if="listQuery.SortMethod =='LineNO'" prop="LineNO" label="班次" :resizable="true" align="center" />
        <el-table-column prop="PBNum" width="180" label="计划车辆数" :resizable="true" align="center">
          <template slot-scope="scope">
            <div style="color:red;text-decoration:underline;cursor:pointer;" @click="clickDetail(scope.row,0)">{{ scope.row.PBNum }}</div>
          </template>
        </el-table-column>
        <el-table-column prop="ABNum" width="180" label="实际车辆数" :resizable="true" align="center">
          <template slot-scope="scope">
            <div style="color:red;text-decoration:underline;cursor:pointer;" @click="clickDetail(scope.row,1)">{{ scope.row.ABNum }}</div>
          </template>
        </el-table-column>
        <el-table-column prop="PSNum" width="180" label="计划完成数" :resizable="true" align="center" />
        <el-table-column prop="ASNum" width="180" label="实际完成数" :resizable="true" align="center" />
      </el-table>

      <pagination
        v-show="total>0"
        :total="total"
        :page.sync="listQuery.CurrentPage"
        :limit.sync="listQuery.PageSize"
        @pagination="getlist"
      />
    </div>
    <el-dialog title="车辆详情" :visible.sync="open" width="1000px" height="500px">
      <el-table
        :ref="init.table.key"
        :key="init.table.key"
        v-loading="detailloading"
        :border="true"
        :data="detaildata"
        style="width:100%;"
        :height="init.table.height"
        :fit="true"
        :stripe="true"
        :empty-text="init.table.emptytext"
      >
        <el-table-column prop="DBusCard" width="180" label="车号" :resizable="true" align="center" />
        <el-table-column prop="BusNo" width="180" label="自编号" :resizable="true" align="center" />
      </el-table>

      <pagination
        v-show="detailtotal>0"
        :total="detailtotal"
        :page.sync="detailQuery.CurrentPage"
        :limit.sync="detailQuery.PageSize"
        @pagination="getDetail"
      />
    </el-dialog>
  </div>
</template>

<script>
import communicate from '@/utils/communicate.js'
import excelhelper from '@/utils/excelhelper.js'
import convert from '@/utils/convert.js'

export default {
  name: 'List',
  data: function() {
    return {
      // 遮罩层
      loading: false,
      detailloading: false,
      // 是否显示弹出层
      open: false,
      // 合计数量
      total: 0,
      detailtotal: 0,
      //  查询绑定参数
      listQuery: {
        LName: '',
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'CompName'
      },
      detailQuery: {
        LName: '',
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'BusNo'
      },
      pickerOptions: {
        disabledDate(time) {
          return time.getTime() > Date.now() - 8.64e6
        }
      },
      // 日期范围
      dateRange: [],
      // 初始化参数
      init: {
        //  查询条件初始化参数
        search: {
          lineNumber: [
            {
              key: 'empty',
              value: '',
              label: '--请选择--'
            },
            {
              key: '1',
              value: '1',
              label: '1'
            },
            {
              key: '10',
              value: '10',
              label: '10'
            },
            {
              key: '900',
              value: '900',
              label: '900'
            }
          ],
          lineDirect: [
            {
              key: 'empty',
              value: '',
              label: '--请选择--'
            }
          ],
          corp: [],
          line: [],
          shift: []
        },
        table: {
          height: window.innerHeight - 350,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'QueryPlanExectue',
          datadetailtype: 'QueryBusPlanDetail',
          url: 'http://localhost:8016/User/GetList',
          listnode: 'Data',
          type: 'post'
        }
      },
      //    表格数据
      data: []
    }
  },
  methods: {
    //  获取数据
    getlist: function() {
      var vm = this
      if (this.$refs[this.init.table.key]) this.$refs[this.init.table.key].bodyWrapper.scrollTop = 0
      if (!vm.listQuery.Date) {
        vm.$message.error('请选择日期！')
        return
      }
      vm.loading = true
      communicate
        .TransformData(vm.init.table.datatype, vm.listQuery, 'busCompany', 'business')
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
        })
    },
    clickDetail(row, type) {
      this.getDetilQuery(row, type)
      this.getDetail()
    },
    getDetilQuery(row, type) {
      this.open = true
      this.detailQuery.CurrentPage = 1
      this.detailQuery.Date = row.Date
      if (row.CompName) {
        this.detailQuery.CompName = row.CompName
        this.$delete(this.detailQuery, 'LName')
        this.$delete(this.detailQuery, 'LineNO')
      }
      if (row.LName) {
        this.detailQuery.LName = row.LName
        this.$delete(this.detailQuery, 'CompName')
        this.$delete(this.detailQuery, 'LineNO')
      }
      if (row.LineNO) {
        this.detailQuery.LineNO = row.LineNO
        this.$delete(this.detailQuery, 'CompName')
        this.$delete(this.detailQuery, 'LName')
      }
      if (type === 1) {
        this.detailQuery.Type = '1'
      } else {
        this.$delete(this.detailQuery, 'Type')
      }
    },
    //  获取数据
    getDetail: function() {
      var vm = this
      vm.detailloading = true
      communicate
        .TransformData(vm.init.table.datadetailtype, vm.detailQuery, 'busCompany', 'business')
        .then(response => {
          vm.detaildata = []
          if (response[vm.init.table.listnode]) {
            if (Array.isArray(response[vm.init.table.listnode])) {
              vm.detaildata = response[vm.init.table.listnode]
            } else {
              vm.detaildata.push(response[vm.init.table.listnode])
            }
            vm.detailtotal = response._Total
            vm.detailloading = false
          } else {
            vm.detaildata = []
            vm.detailtotal = 0
            vm.detailloading = false
          }
        })
    },
    //  查询按钮
    btnlistQuery: function() {
      this.listQuery.CurrentPage = 1
      this.getlist()
    },
    btnExport: function() {
      var vm = this
      //  接口对象处理
      if (!vm.dateRange) {
        vm.$delete(vm.listQuery, 'StartTime')
        vm.$delete(vm.listQuery, 'EndTime')
      }

      const exportquery = {}
      Object.assign(exportquery, vm.listQuery)
      exportquery.CurrentPage = 0
      exportquery.PageSize = 999999

      const myload = vm.inloading('导出中，请稍后......')
      communicate
        .TransformData(vm.init.table.datatype, this.addDateRange(exportquery, vm.dateRange), 'busCompany', 'business')
        .then(response => {
          if (response[vm.init.table.listnode]) {
            const columns = []
            const headers = []
            const exist = ['操作']
            const data = convert.object2array(response[vm.init.table.listnode])
            console.log(this.$refs.table.columns)
            this.$refs.table.columns.map(item => {
              if (item.property && (item.type === 'default' || item.type === 'text') && columns.indexOf(item.property) < 0 && exist.indexOf(item.label) < 0) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })
            vm.unloading(myload)
            excelhelper.export('车辆计划完成情况', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
        })
    },
    //  切换按钮
    handleClick: function() {
      this.btnlistQuery()
    }
  }
}
</script>

<style scoped>
.area {
  margin: 5px auto;
}
.el-row {
  margin: 5px auto;
}

.label {
  text-align: center;
  line-height: 36px;
  font-size: 14px;
}
</style>
