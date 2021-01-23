<template>
  <div class="app-container">
    <!--  查询条件  -->
    <div class="area">
      <el-form ref="queryForm" :model="query" label-width="120px">
        <el-row>
          <el-col :span="8">
            <el-form-item label="自编号">
              <el-input
                v-model="query.BusNo"
                placeholder="请输入自编号"
                style="width:210px;"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="类型">
              <el-select v-model="query.ReportType" style="width:210px;">
                <el-option key="all" label="全部" value />
                <el-option key="0" label="测试报警" value="0" />
                <el-option key="1" label="正常报警" value="1" />
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="是否接警">
              <el-select v-model="query.ReceivingAlarm" style="width:210px;">
                <el-option key="all" label="全部" value />
                <el-option key="0" label="未接警" value="0" />
                <el-option key="1" label="接警" value="1" />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="中心确认状态">
              <el-select v-model="query.Confirm" style="width:210px;">
                <el-option key="all" label="全部" value />
                <el-option key="0" label="未确认" value="0" />
                <el-option key="1" label="已确认" value="1" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="开始时间">
              <el-date-picker
                v-model="query.initStartDate"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择开始时间"
                style="width:210px;"
                :picker-options="pickerOptionsStart"
                @change="changeEnd"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="结束时间">
              <el-date-picker
                v-model="query.initEndDate"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择结束时间"
                style="width:210px;"
                :picker-options="pickerOptionsEnd"
                @change="changeStart"
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
      <el-button type="success" size="small" class="el-button--export" @click="btnExport">
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
        <el-table-column label="日期时间" width="200" property="LUTC" align="center" />
        <el-table-column label="车牌号" width="120" property="DBusCard" align="center" />
        <el-table-column label="自编号" width="180" property="BusNo" align="center" />
        <el-table-column label="经度" width="180" property="LLON" align="center" />
        <el-table-column label="纬度" width="180" property="LLAT" align="center" />
        <el-table-column label="速度" width="150" property="LSpeed" align="center" />
        <el-table-column label="方向" width="LDir" property="Status" align="center" />
        <el-table-column label="状态" width="120" property="LVFlag" align="center" />
        <el-table-column label="描述" width="150" property="LVDesc" align="center" />
        <el-table-column label="类型" width="150" property="ReportType" align="center" :formatter="formatter" />
        <el-table-column label="车辆异常状态描述" width="180" property="ReportDesc" align="center" />
        <el-table-column label="调度中心状态确认" width="180" property="Confirm" align="center" :formatter="formatter" />
        <el-table-column label="确认时间" width="150" property="ConfirmTime" align="center" />
        <el-table-column label="确认人" width="200" property="Confirmor" align="center" />
        <el-table-column label="是否接警" width="150" property="ReceivingAlarm" align="center" :formatter="formatter" />
        <el-table-column label="接警时间" width="150" property="ReceivingAlarmTime" align="center" />
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
        CurrentPage: 0,
        PageSize: 10,
        SortMethod: 'LUTC',
        OrderMethod: 'desc',
        initStartDate: null,
        initEndDate: null,
        StartDate: null,
        EndDate: null,
        ReportType: null,
        BusNo: null,
        ReceivingAlarm: null,
        Confirm: null
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {},
        table: {
          height: window.innerHeight - 366,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'BusAlarmDataList',
          listnode: 'BusAlarmData'
        },
        dialog: {
          width: '50%',
          showDialog: false,
          title: '',
          type: 'detail',
          id: null
        },
        sort: {
          SortMethod: 'LUTC',
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
  created: function() {
    //  初始化线路下拉数据
    // communicate("TransformData", vm.init.table.datatype, vm.query).then(
    //     response => {
    //       console.log(response);
    //       vm.data = response[vm.init.table.listnode];
    //       vm.pager.total = 100;
    //     }
    //   );
  },
  mounted: function() {
    // this.init.table.height =
    //   window.innerHeight - this.$refs.table.$el.offsetTop - 50;
    //  console.log(this.init.table.height);
    this.initData()
    this.getlist()
  },
  methods: {
    changeStart() {
      this.pickerOptionsStart = Object.assign({}, this.pickerOptionsStart, {
        disabledDate: (time) => {
          if (!this.query.initEndDate) return false
          return time.getTime() > new Date(this.query.initEndDate).getTime()
        }
      })
    },
    changeEnd() {
      this.pickerOptionsEnd = Object.assign({}, this.pickerOptionsEnd, {
        disabledDate: (time) => {
          if (!this.query.initStartDate) return false
          return time.getTime() < new Date(this.query.initStartDate).getTime()
        }
      })
    },
    initData() {
      this.query.initStartDate = this.helpers.date.getCurrentDate()
      this.query.initEndDate = this.helpers.date.getCurrentDate()
    },
    //  列format事件
    formatter: function(row, column, cellValue, index) {
      let value = null
      switch (column.label) {
        case '类型': {
          switch (cellValue) {
            case '0': {
              value = '测试报警'
              break
            }
            case '1': {
              value = '正常报警'
              break
            }
          }
          break
        }
        case '调度中心状态确认': {
          switch (cellValue) {
            case '0': {
              value = '未确认'
              break
            }
            case '1': {
              value = '已确认'
              break
            }
          }
          break
        }
        case '是否接警': {
          switch (cellValue) {
            case '0': {
              value = '未接警'
              break
            }
            case '1': {
              value = '接警'
              break
            }
          }
          break
        }
      }
      return value
    },
    //  获取数据
    getlist: function(pager) {
      var vm = this
      debugger
      if (this.$refs[this.init.table.key]) this.$refs[this.init.table.key].bodyWrapper.scrollTop = 0
      vm.loading = true

      //  接口对象处理
      if (!vm.query.initStartDate) {
        vm.$delete(vm.query, 'StartDate')
      } else {
        vm.query.StartDate = vm.query.initStartDate + ' 00:00:00.000'
      }
      if (!vm.query.initEndDate) {
        vm.$delete(vm.query, 'EndDate')
      } else {
        vm.query.EndDate = vm.query.initEndDate + ' 23:59:59.999'
      }

      if (pager === undefined) {
        vm.query.CurrentPage = 0
      } else {
        vm.query.CurrentPage = pager.page - 1
        vm.query.PageSize = pager.limit
        vm.pager = pager
      }

      communicate
        .TransformData(vm.init.table.datatype, vm.helpers.convert.null2empty(vm.query), 'cardata', 'cardata')
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

    //  查询按钮
    btnQuery: function() {
      this.pager.page = 1
      this.getlist()
    },
    //  导出按钮
    btnExport: function() {
      var vm = this

      //  接口对象处理
      if (!vm.query.initStartDate) {
        vm.$delete(vm.query, 'StartDate')
      } else {
        vm.query.StartDate = vm.query.initStartDate + ' 00:00:00.000'
      }
      if (!vm.query.initEndDate) {
        vm.$delete(vm.query, 'EndDate')
      } else {
        vm.query.EndDate = vm.query.initEndDate + ' 23:59:59.999'
      }

      const exportquery = {}
      Object.assign(exportquery, vm.helpers.convert.null2empty(vm.query))
      exportquery.CurrentPage = 0
      exportquery.PageSize = 999999

      const myload = vm.inloading('导出中，请稍后......')
      communicate
        .TransformData(vm.init.table.datatype, exportquery, 'cardata', 'cardata')
        .then(response => {
          if (response[vm.init.table.listnode]) {
            const columns = []
            const headers = []
            const exist = ['操作']
            const data = convert.object2array(response[vm.init.table.listnode])

            data.map(item => {
              switch (item.ReportType) {
                case '0': {
                  item.ReportType = '测试报警'
                  break
                }
                case '1': {
                  item.ReportType = '正常报警'
                  break
                }
              }
              switch (item.Confirm) {
                case '0': {
                  item.Confirm = '未确认'
                  break
                }
                case '1': {
                  item.Confirm = '已确认'
                  break
                }
              }
              switch (item.ReceivingAlarm) {
                case '0': {
                  item.ReceivingAlarm = '未接警'
                  break
                }
                case '1': {
                  item.ReceivingAlarm = '接警'
                  break
                }
              }
            })

            this.$refs.table.columns.map(item => {
              if (item.property && (item.type == 'default' || item.type == 'text') && columns.indexOf(item.property) < 0 && exist.indexOf(item.label) < 0) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })
            excelhelper.export('报警事件查询表', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
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
