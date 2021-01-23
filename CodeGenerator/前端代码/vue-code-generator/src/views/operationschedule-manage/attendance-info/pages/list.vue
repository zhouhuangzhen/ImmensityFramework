<template>
  <div class="app-container">
    <!--  查询条件  -->
    <div class="area">
      <el-form ref="queryForm" :model="query" label-width="120px">
        <el-row>
          <el-col :span="8">
            <el-form-item label="签到开始日期">
              <el-date-picker
                v-model="query.initStartTime"
                size="small"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择开始日期"
                style="width:210px;"
                :picker-options="pickerOptionsStart"
                @change="changeEnd"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="签到结束日期">
              <el-date-picker
                v-model="query.initEndTime"
                size="small"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择结束日期"
                style="width:210px;"
                :picker-options="pickerOptionsEnd"
                @change="changeStart"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="状态">
              <el-select
                v-model="query.Status"
                :placeholder="$t('search.atttype')"
                clearable
                size="small"
                style="width:210px;"
              >
                <el-option key="all" label="全部" value />
                <el-option key="login" label="签到" value="0" />
                <el-option key="logout" label="签退" value="1" />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="工号">
              <el-input
                v-model="query.EmpNo"
                placeholder="请输入工号"
                clearable
                size="small"
                style="width:210px;"
                @keyup.enter.native="handleQuery"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="姓名">
              <el-input
                v-model="query.EName"
                placeholder="请输入姓名"
                clearable
                size="small"
                style="width:210px;"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </div>
    <div style="text-align:right;" class="area">
      <el-button
        class="filter-item"
        type="primary"
        icon="el-icon-search"
        size="mini"
        @click.native="handleQuery"
      >搜索</el-button>
      <el-button type="info" icon="el-icon-download" class="el-button--export" size="mini" @click="btnExport">导出</el-button>
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
        :height="init.table.height"
        :fit="true"
        :stripe="true"
        :empty-text="init.table.emptytext"
      >
        <el-table-column :label="$t('detail.index')" align="center" width="70">
          <template scope="scope"><span>{{ scope.$index+(query.CurrentPage) * query.PageSize + 1 }} </span></template>
        </el-table-column>
        <el-table-column label="工号" width="80" align="center" prop="EmpNo" />
        <el-table-column label="日期" width="150" align="center" prop="SignTime" :formatter="formatter" />
        <el-table-column label="姓名" width="100" align="center" prop="EName" />
        <el-table-column label="实际签到时间" width="250" align="center" prop="SignTime" />
        <el-table-column label="状态" width="150" align="center" prop="Status" :formatter="formatter" />
        <el-table-column label="备注" align="center" prop="Remark" :show-overflow-tooltip="true" />
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
      pickerOptionsStart: {},
      pickerOptionsEnd: {},
      // 遮罩层
      loading: false,
      // 合计数量
      total: 0,
      // 时间区间
      Time: [],
      //  查询绑定参数
      query: {
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'SignTime',
        OrderMethod: 'desc',
        initStartTime: null,
        initEndTime: null,
        StartTime: null,
        EndTime: null,
        EName: null,
        EmpNo: null,
        Status: null
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {},
        table: {
          height: window.innerHeight - 366,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'AttendanceDataList',
          listnode: 'AttendanceData'
        },
        dialog: {
          width: '50%',
          showDialog: false,
          title: '',
          type: 'detail',
          Id: null
        },
        sort: {
          SortMethod: 'SignTime',
          OrderMethod: 'desc'
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
  created() {
    //  this.getlist()
  },
  mounted() {
    this.initData()
  },
  methods: {
    changeStart() {
      this.pickerOptionsStart = Object.assign({}, this.pickerOptionsStart, {
        disabledDate: (time) => {
          if (!this.query.initEndTime) return false
          return time.getTime() > new Date(this.query.initEndTime).getTime()
        }
      })
    },
    changeEnd() {
      this.pickerOptionsEnd = Object.assign({}, this.pickerOptionsEnd, {
        disabledDate: (time) => {
          if (!this.query.initStartTime) return false
          return time.getTime() < new Date(this.query.initStartTime).getTime()
        }
      })
    },
    initData() {
      this.query.initStartTime = this.helpers.date.fromToday(-1)
      this.query.initEndTime = this.helpers.date.fromToday(-1)
    },
    //  列format事件
    formatter: function(row, column, cellValue, index) {
      let value = null
      switch (column.label) {
        case '日期': {
          value = this.parseTime(new Date(cellValue), '{y}-{m}-{d}')
          break
        }
        case '状态': {
          if (cellValue == 0) {
            value = '签到'
          } else if (cellValue == 1) {
            value = '签退'
          } else {
            value = ''
          }
          break
        }
      }
      return value
    },
    //  获取数据
    getlist: function(pager) {
      debugger
      var vm = this
      if (this.$refs[this.init.table.key]) this.$refs[this.init.table.key].bodyWrapper.scrollTop = 0
      vm.loading = true
      //  接口对象处理
      if (!vm.query.initStartTime) {
        vm.$delete(vm.query, 'StartTime')
      } else {
        vm.query.StartTime = vm.query.initStartTime + ' 00:00:00.000'
      }
      if (!vm.query.initEndTime) {
        vm.$delete(vm.query, 'EndTime')
      } else {
        vm.query.EndTime = vm.query.initEndTime + ' 23:59:59.999'
      }
      if (!vm.query.EName) {
        vm.$delete(vm.query, 'EName')
      }
      if (!vm.query.EmpNo) {
        vm.$delete(vm.query, 'EmpNo')
      }
      if (!vm.query.Status) {
        vm.$delete(vm.query, 'Status')
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
    },
    //  查询按钮
    handleQuery() {
      if (
        !this.query.initStartTime ||
        this.query.initStartTime.length <= 0 ||
        !this.query.initEndTime ||
        this.query.initEndTime.length <= 0) {
        this.msgError('开始时间和结束时间不能为空')
        return
      }
      this.pager.page = 1
      this.getlist()
    },
    //  导出按钮
    btnExport() {
      var vm = this
      //  接口对象处理
      if (!vm.query.initStartTime) {
        vm.$delete(vm.query, 'StartTime')
      } else {
        vm.query.StartTime = vm.query.initStartTime + ' 00:00:00.000'
      }
      if (!vm.query.initEndTime) {
        vm.$delete(vm.query, 'EndTime')
      } else {
        vm.query.EndTime = vm.query.initEndTime + ' 23:59:59.999'
      }
      if (!vm.query.EName) {
        vm.$delete(vm.query, 'EName')
      }
      if (!vm.query.EmpNo) {
        vm.$delete(vm.query, 'EmpNo')
      }
      if (!vm.query.Status) {
        vm.$delete(vm.query, 'Status')
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
            const columns = []
            const headers = []
            const exist = []
            const data = convert.object2array(response[vm.init.table.listnode])
            this.$refs.table.columns.map(item => {
              if (item.property && (item.type == 'default' || item.type == 'text') && columns.indexOf(item.property) < 0 && exist.indexOf(item.label) < 0) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })
            data.map(item => {
              //  日期格式化
              item.SignTime = this.parseTime(new Date(item.SignTime), '{y}-{m}-{d}')
              // 状态格式化
              switch (item.Status) {
                case '0': {
                  item.Status = '签到'
                  break
                } case '1': {
                  item.Status = '签退'
                  break
                } default: {
                  item.Status = ''
                  break
                }
              }
            })
            excelhelper.export('考勤信息表', headers, columns, data)
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
</style>
